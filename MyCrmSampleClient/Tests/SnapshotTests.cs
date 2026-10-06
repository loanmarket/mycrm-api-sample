using System.Net;
using System.Text;
using System.Text.Json;
using ClosedXML.Excel;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Http.HttpClientLibrary;
using MyCrmSampleClient.Console;
using MyCrmSampleClient.FinancialSnapshots;
using MyCrmSampleClient.Kiota;
using MyCrmSampleClient.Kiota.Models;
using Xunit;

namespace MyCrmSampleClient.Tests;

public sealed class SnapshotTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), "mycrm-snapshot-tests-" + Guid.NewGuid().ToString("N"));
    private static readonly Contact[] Contacts = [
        new() { Type = "contacts", Id = "1001", Attributes = new() { FirstName = "Alex", Role = ContactAttributes_role.Adult } },
        new() { Type = "contacts", Id = "1002", Attributes = new() { FirstName = "Sam", Role = ContactAttributes_role.Adult } }
    ];
    private static SnapshotWorkbook Example(string country = "AU") => SnapshotExamples.Create(5001, Contacts, SnapshotExampleData.MinimalControls(), country, seed: 12345);
    public SnapshotTests() => Directory.CreateDirectory(_directory);
    public void Dispose() => Directory.Delete(_directory, true);

    [Fact]
    public void SavingWithoutOverwriteKeepsExistingFile()
    {
        var path = SaveExample();
        var original = File.ReadAllBytes(path);
        var replacement = Example();
        replacement.Source = "Updated export";

        Assert.Throws<IOException>(() => replacement.Save(path));

        Assert.Equal(original, File.ReadAllBytes(path));
        Assert.Single(Directory.GetFiles(_directory));
    }

    [Theory]
    [InlineData("NULL")]
    [InlineData(" null ")]
    [InlineData(null)]
    public void ExportedNullPlaceholdersBecomeBlankCellsAcrossAllDataSheets(string missing)
    {
        var snapshot = new SnapshotWorkbook { ContactGroupId = 5001 };
        var source = new { Lid = "record", Description = missing, CreditorName = missing, EmployerName = missing, StreetAddress = missing, AccountName = "NULL Bank", AccountNumber = "00123456" };
        snapshot.Attributes.Assets.Add(SnapshotApi.Copy<FinancialSnapshotAsset>(source));
        snapshot.Attributes.Expenses.Add(SnapshotApi.Copy<FinancialSnapshotExpense>(source));
        snapshot.Attributes.Incomes.Add(SnapshotApi.Copy<FinancialSnapshotIncome>(source));
        snapshot.Attributes.Liabilities.Add(SnapshotApi.Copy<FinancialSnapshotLiability>(source));
        snapshot.Attributes.Employments.Add(SnapshotApi.Copy<FinancialSnapshotEmployment>(source));
        snapshot.Attributes.Addresses.Add(SnapshotApi.Copy<FinancialSnapshotAddress>(source));

        var path = Path.Combine(_directory, "missing-text.xlsx");
        snapshot.Save(path);

        using var excel = new XLWorkbook(path);
        var imported = SnapshotWorkbook.Load(path);
        foreach (var (sheet, header) in new[] { ("Assets", "description"), ("Expenses", "description"), ("Incomes", "description"),
            ("Liabilities", "creditorName"), ("Employments", "employerName"), ("Addresses", "streetAddress") })
        {
            var cell = excel.Worksheet(sheet).Row(1).CellsUsed().Single(c => c.GetString() == header).CellBelow();
            Assert.True(cell.IsEmpty());
            Assert.Equal("", cell.GetString());
            Assert.Null(SnapshotWorkbook.Columns(sheet).Single(column => column.Header == header).Get(imported.Rows(sheet)[0]));
        }

        Assert.Equal("NULL Bank", imported.Attributes.Assets[0].AccountName);
        Assert.Equal("00123456", imported.Attributes.Assets[0].AccountNumber);
    }

    [Fact]
    public void SavingWithOverwriteReplacesExistingWorkbook()
    {
        var path = SaveExample();
        var replacement = Example();
        replacement.Source = "Updated export";
        replacement.Attributes.Assets[0].Value = 850000;

        replacement.Save(path, overwrite: true);

        var loaded = SnapshotWorkbook.Load(path);
        Assert.Equal("Updated export", loaded.Source);
        Assert.Equal(850000, loaded.Attributes.Assets[0].Value);
        Assert.Single(Directory.GetFiles(_directory));
    }

    [Theory]
    [InlineData("AU", false)]
    [InlineData("NZ", false)]
    [InlineData("AU", true)]
    [InlineData("NZ", true)]
    public async Task ExamplesRoundTripEverySubmittedField(string country, bool omitEmploymentType)
    {
        // Instantiates Kiota's serializers as normal application startup does.
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider());
        _ = new MyCrmApiClient(adapter);
        var source = Example(country);
        source.Attributes.Assets[1].AssetId = 8101;
        source.Attributes.Employments[0].EmploymentId = 8201;
        if (omitEmploymentType) source.Attributes.Employments[0].EmploymentType = null;
        source.Attributes.Incomes[0].IncomeId = 8301;
        source.Attributes.Incomes[0].Employment = new() { Id = 8201 };
        source.Attributes.Liabilities[0].LiabilityId = 8401;
        source.Attributes.Expenses[0].ExpenseId = 8501;
        source.SetMode("Assets", "Replace");
        source.Attributes.Employments[0].EmployerName = "=This is literal text";
        var path = Path.Combine(_directory, "example.xlsx");
        source.Save(path);
        var loaded = SnapshotWorkbook.Load(path);
        Assert.Equal(await KiotaJsonSerializer.SerializeAsStringAsync(source.ToDocument()), await KiotaJsonSerializer.SerializeAsStringAsync(loaded.ToDocument()));
        Assert.Null(loaded.Attributes.Assets[0].Description);
        Assert.False(string.IsNullOrWhiteSpace(loaded.Attributes.Assets[1].Description));
        Assert.StartsWith("00", loaded.Attributes.Assets[1].AccountNumber);
        if (country == "AU") Assert.Matches(@"^\d{3}-\d{3}$", loaded.Attributes.Assets[1].Bsb);
        else Assert.Null(loaded.Attributes.Assets[1].Bsb);
        using var excel = new XLWorkbook(path);
        Assert.True(Cell(excel, "Assets", "description").IsEmpty());
        var accountColumn = SnapshotWorkbook.Columns("Assets").Select((c, i) => (c, i)).Single(x => x.c.Header == "accountNumber").i + 1;
        Assert.Equal(XLDataType.Text, excel.Worksheet("Assets").Cell(3, accountColumn).DataType);
        Assert.All(excel.Worksheets, sheet => Assert.DoesNotContain(sheet.CellsUsed(), c => c.HasFormula));
    }

    [Theory]
    [InlineData("Assets", "owner1ContactId", "9999")]
    [InlineData("Assets", "assetTypeId", "")]
    [InlineData("Assets", "value", "-10")]
    [InlineData("Incomes", "employment.lid", "missing")]
    [InlineData("Liabilities", "linkedAsset.id", "-42")]
    [InlineData("Expenses", "frequency", "")]
    [InlineData("Employments", "contactId", "0")]
    [InlineData("Addresses", "lid", "")]
    [InlineData("Assets", "description", "  NULL  ")]
    [InlineData("Assets", "lid", "  asset with spaces  ")]
    [InlineData("Assets", "address.lid", "  missing address  ")]
    public void ImportPreservesEnteredValuesForServerValidation(string sheet, string header, string value)
    {
        var path = SaveExample();
        Edit(path, book => Cell(book, sheet, header).Value = value);
        var loaded = SnapshotWorkbook.Load(path);
        var submitted = SnapshotWorkbook.Columns(sheet).Single(column => column.Header == header).Get(loaded.Rows(sheet)[0]);

        Assert.Equal(value, submitted?.ToString() ?? "");
    }

    [Fact]
    public void ImportPreservesDuplicateIdsAndLids()
    {
        var book = Example();
        book.Attributes.Assets.ForEach(a => a.AssetId = 12);
        book.Attributes.Assets[1].Lid = book.Attributes.Assets[0].Lid;
        var path = Path.Combine(_directory, "duplicates.xlsx");
        book.Save(path);

        var loaded = SnapshotWorkbook.Load(path);
        Assert.Equal(2, loaded.Attributes.Assets.Count);
        Assert.All(loaded.Attributes.Assets, asset => Assert.Equal(12, asset.AssetId));
        Assert.Equal(loaded.Attributes.Assets[0].Lid, loaded.Attributes.Assets[1].Lid);
    }

    [Fact]
    public void MissingSheetIsNotAnEmptyReplacement()
    {
        var path = SaveExample();
        Edit(path, book => book.Worksheet("Assets").Delete());
        Assert.Contains("Missing required worksheet", Assert.Throws<FormatException>(() => SnapshotWorkbook.Load(path)).Message);
    }

    [Fact]
    public void RejectsUnreadableFormulaAndUnknownColumnButConvertsNumericAccountToText()
    {
        var path = SaveExample();
        Edit(path, book => Cell(book, "Assets", "value").FormulaA1 = "1+2");
        Assert.Contains("Formulas", Assert.Throws<FormatException>(() => SnapshotWorkbook.Load(path)).Message);
        Edit(path, book => { Cell(book, "Assets", "value").Value = 780000; Cell(book, "Assets", "accountNumber").Value = 1234; });
        Assert.Equal("1234", SnapshotWorkbook.Load(path).Attributes.Assets[0].AccountNumber);
        Edit(path, book => { Cell(book, "Assets", "accountNumber").Clear(); book.Worksheet("Assets").Cell(1, 1).Value = "typo"; });
        Assert.Contains("headers", Assert.Throws<FormatException>(() => SnapshotWorkbook.Load(path)).Message);
    }

    [Theory]
    [InlineData("Expenses", "frequency", "monthly")]
    [InlineData("Assets", "value", "NaN")]
    [InlineData("Assets", "assetId", "1.5")]
    [InlineData("Employments", "dateStarted", "03/04/2020")]
    [InlineData("Employments", "employmentType", "UnknownEmploymentType")]
    public void CellErrorsIncludeLocation(string sheet, string header, string value)
    {
        var path = SaveExample();
        Edit(path, book => Cell(book, sheet, header).Value = value);
        Assert.Contains(sheet + "!", Assert.Throws<FormatException>(() => SnapshotWorkbook.Load(path)).Message);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void EmptyReplacementAndExistingIdLinksAllowServerMatching(bool employmentLink)
    {
        var book = Example();
        book.SetMode("Expenses", "Replace");
        book.Attributes.Expenses.Clear();

        // A target without an explicit ID can match an existing record on the server.
        if (employmentLink)
        {
            book.SetMode("Employments", "Replace");
            book.Attributes.Incomes.First().Employment = new() { Id = 99 };
        }
        else
        {
            book.SetMode("Assets", "Replace");
            book.Attributes.Incomes.Last().LinkedAsset = new() { Id = 99 };
        }

        var path = Path.Combine(_directory, "matched-links.xlsx");
        book.Save(path);
        var loaded = SnapshotWorkbook.Load(path);

        Assert.Empty(loaded.Attributes.Expenses);
        Assert.Equal(99, employmentLink ? loaded.Attributes.Incomes.First().Employment.Id : loaded.Attributes.Incomes.Last().LinkedAsset.Id);
        Assert.Null(employmentLink ? loaded.Attributes.Employments[0].EmploymentId : loaded.Attributes.Assets[0].AssetId);
    }

    [Theory]
    [InlineData("api.contactgroups.financialsnapshot", true)]
    [InlineData("api.contactgroups", true)]
    [InlineData("api.financialsnapshot", true)]
    [InlineData("api", true)]
    [InlineData("api.contact-groups.update api.assets.update", false)]
    public void SnapshotRequiresItsDedicatedScope(string scopes, bool allowed) =>
        Assert.Equal(allowed, new SampleScopes(scopes).HasScope("api.contactgroups.financialsnapshot"));

    [Fact]
    public void OwnershipFilterUsesDistinctContactIdsAndRejectsEmptyGroups()
    {
        Assert.Equal("has(ownership,any(contact.id,'1001','1002'))", SnapshotApi.OwnershipFilter([1002, 1001, 1002]));
        Assert.Throws<ArgumentException>(() => SnapshotApi.OwnershipFilter([]));
    }

    [Fact]
    public async Task GeneratedClientPostsJsonApiSnapshotAndReadsOperations()
    {
        using var handler = new RecordingHandler(request =>
        {
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.EndsWith("/jsonapi/contact-groups/5001/financial-snapshot", request.RequestUri!.AbsolutePath);
            Assert.Equal("application/vnd.api+json", request.Content!.Headers.ContentType!.MediaType);
            var json = JsonDocument.Parse(request.Content.ReadAsStringAsync().GetAwaiter().GetResult());
            var data = json.RootElement.GetProperty("data");
            Assert.Equal("financial-snapshots", data.GetProperty("type").GetString());
            Assert.Equal("Replace", data.GetProperty("attributes").GetProperty("assetsMode").GetString());
            Assert.Equal(8101, data.GetProperty("attributes").GetProperty("assets")[0].GetProperty("assetId").GetInt32());
            return """{"data":{"type":"financial-snapshots","id":"test","attributes":{"operations":{"created":[],"updated":[{"type":"assets","id":"8101","meta":{"lids":["investment-property"]}}],"deleted":[]}}}}""";
        });
        using var http = new HttpClient(handler);
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: http) { BaseUrl = "https://example.test" };
        var client = new MyCrmApiClient(adapter);
        var book = Example();
        book.SetMode("Assets", "Replace"); book.Attributes.Assets[0].AssetId = 8101;
        var result = await client.Jsonapi.ContactGroups[5001].FinancialSnapshot.PostAsync(book.ToDocument());
        Assert.Equal("investment-property", Assert.Single(Assert.Single(result!.Data!.Attributes!.Operations!.Updated!).Meta!.Lids!));
    }

    private string SaveExample()
    {
        var path = Path.Combine(_directory, Guid.NewGuid() + ".xlsx");
        Example().Save(path);
        return path;
    }
    private static IXLCell Cell(XLWorkbook book, string sheet, string header) => book.Worksheet(sheet).Cell(2,
        book.Worksheet(sheet).Row(1).CellsUsed().Single(c => c.GetString() == header).Address.ColumnNumber);
    private static void Edit(string path, Action<XLWorkbook> edit) { using var book = new XLWorkbook(path); edit(book); book.Save(); }

    internal sealed class RecordingHandler(Func<HttpRequestMessage, string> respond, HttpStatusCode status = HttpStatusCode.OK) : HttpMessageHandler
    {
        public List<string> Requests { get; } = [];
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Requests.Add(Uri.UnescapeDataString(request.RequestUri!.AbsoluteUri));
            return Task.FromResult(new HttpResponseMessage(status) { Content = new StringContent(respond(request), Encoding.UTF8, "application/vnd.api+json") });
        }
    }
}
