using System.IO.Compression;
using System.Net;
using System.Text.Json;
using System.Xml.Linq;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Validation;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using MyCrmSampleClient.FinancialSnapshots;
using MyCrmSampleClient.Kiota;
using MyCrmSampleClient.Kiota.Models;
using Xunit;

namespace MyCrmSampleClient.Tests;

public sealed class SnapshotControlDataTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), "mycrm-controls-" + Guid.NewGuid().ToString("N"));
    public SnapshotControlDataTests() => Directory.CreateDirectory(_directory);
    public void Dispose() => Directory.Delete(_directory, true);
    private static readonly Contact[] Contacts = [new() { Id = "1001", Type = "contacts", Attributes = new() { Role = ContactAttributes_role.Adult } }];

    private static List<SnapshotLookup> Controls() => [
        new("asset-categories", 1, "Property"), new("asset-types", 1, "Real Estate"), new("asset-types", 15, "Savings"),
        new("asset-sub-types", 2, "Sedan"), new("property-types", 13, "Fully Detached House"),
        new("liability-categories", 1, "Secured"), new("liability-categories", 2, "Unsecured"),
        new("liability-types", 21, "Mortgage", "liability-categories", 1), new("liability-types", 7, "Credit Card", "liability-categories", 2),
        new("liability-sub-types", 4, "Visa", "liability-types", 7),
        new("expense-categories", 1, "Living"), new("expense-types", 17, "Groceries", "expense-categories", 1), new("expense-types", 31, "Mortgage Repayments"),
        new("income-categories", 1, "Earnings"), new("income-categories", 2, "Addback"),
        new("income-types", 19, "Salary / Wages", "income-categories", 1), new("income-types", 23, "Rental Income", "income-categories", 1),
        new("income-types", 100, "Some addback", "income-categories", 2), new("income-types", 101, "Net Profit Before Tax"),
        new("income-verification-types", 1, "Payslip, employer letter"), new("rental-verification-types", 2, "Lease")
    ];

    private SnapshotWorkbook Example()
    {
        var book = SnapshotExamples.Create(5001, Contacts, Controls(), "AU", seed: 12345);
        return book;
    }

    [Fact]
    public void DropdownsUseNamedRangesAndSaveAsValidExcel()
    {
        var snapshot = Example();
        // A range-backed list supports commas and more than 255 characters of combined choices.
        snapshot.Lookups.AddRange(Enumerable.Range(1000, 40).Select(id => new SnapshotLookup("asset-sub-types", id, "Subtype, detailed label " + id)));
        var path = Path.Combine(_directory, "controls.xlsx"); snapshot.Save(path);
        using (var excel = new XLWorkbook(path))
        {
            Assert.Equal("21 — Secured / Mortgage", Cell(excel, "Liabilities", "liabilityTypeId").GetString());
            Assert.Equal(XLWorksheetVisibility.VeryHidden, excel.Worksheet("Dropdown lists").Visibility);
            foreach (var sheet in SnapshotWorkbook.DataSheets)
            {
                foreach (var column in SnapshotWorkbook.Columns(sheet).Where(c => SnapshotControlData.Columns.ContainsKey(c.Header)))
                {
                    var cell = Cell(excel, sheet, column.Header);
                    var validation = cell.GetDataValidation();
                    Assert.Equal(XLAllowedValues.List, validation.AllowedValues);
                    Assert.True(validation.InCellDropdown);
                    Assert.True(validation.ShowErrorMessage);
                    Assert.Equal(XLErrorStyle.Warning, validation.ErrorStyle);
                    Assert.False(cell.Style.Alignment.WrapText);
                    Assert.True(cell.Style.Alignment.ShrinkToFit);
                    Assert.Contains(validation.Ranges, r => r.Contains(excel.Worksheet(sheet).Cell(1000, cell.Address.ColumnNumber)));
                    Assert.DoesNotContain(validation.Ranges, r => r.Contains(excel.Worksheet(sheet).Cell(1001, cell.Address.ColumnNumber)));
                    if (column.Header != "liabilitySubTypeId") Assert.NotEmpty(excel.DefinedName(validation.Value).Ranges);
                }
            }
            var subtypes = Cell(excel, "Liabilities", "liabilitySubTypeId").GetDataValidation().Value;
            Assert.StartsWith("INDIRECT(", subtypes);
            Assert.Contains("control_liability_sub_types_", subtypes);
            Assert.Equal("4 — Credit Card / Visa", excel.DefinedName("control_liability_sub_types_7").Ranges.Single().FirstCell().GetString());
            Assert.True(excel.DefinedName("control_liability_sub_types_21").Ranges.Single().FirstCell().IsEmpty());
            var forbiddenExpenses = excel.DefinedName("control_expense_types").Ranges.Single().Cells().Select(c => c.GetString());
            Assert.DoesNotContain(forbiddenExpenses, text => text.Contains("Mortgage Repayments"));
            Assert.DoesNotContain(excel.DefinedName("control_income_types").Ranges.Single().Cells(), c => c.GetString().Contains("addback", StringComparison.OrdinalIgnoreCase) || c.GetString().Contains("Net Profit"));
        }
        using var document = SpreadsheetDocument.Open(path, false);
        Assert.Empty(new OpenXmlValidator().Validate(document));
        // The saved warning lets users override dropdown choices with numeric IDs.
        using var zip = ZipFile.OpenRead(path);
        XNamespace ns = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
        var validations = zip.Entries.Where(e => e.FullName.StartsWith("xl/worksheets/sheet") && e.FullName.EndsWith(".xml"))
            .SelectMany(e => { using var stream = e.Open(); return XDocument.Load(stream).Descendants(ns + "dataValidation").ToArray(); })
            .Where(v => v.Element(ns + "formula1")?.Value.Contains("control_") == true).ToArray();
        Assert.Equal(9, validations.Length);
        Assert.All(validations, v => { Assert.Equal("warning", (string)v.Attribute("errorStyle")); Assert.Equal("1", (string)v.Attribute("showErrorMessage")); });
    }

    [Fact]
    public void LabelledSelectionsRoundTripToNumericRequestIds()
    {
        var book = Example();
        book.Attributes.Liabilities[0].LiabilityId = 8401;
        book.Attributes.Incomes[0].IncomeVerificationId = 1;
        var path = Path.Combine(_directory, "roundtrip.xlsx"); book.Save(path);
        using (var excel = new XLWorkbook(path))
        {
            Cell(excel, "Liabilities", "liabilityTypeId").Value = "7 — Unsecured / Credit Card";
            Cell(excel, "Liabilities", "liabilitySubTypeId").Value = "4 — Credit Card / Visa";
            excel.Save();
        }
        var imported = SnapshotWorkbook.Load(path);
        Assert.Equal(7, imported.Attributes.Liabilities[0].LiabilityTypeId);
        Assert.Equal(4, imported.Attributes.Liabilities[0].LiabilitySubTypeId);
        Assert.Equal(8401, imported.Attributes.Liabilities[0].LiabilityId);
        Assert.Equal(1, imported.Attributes.Incomes[0].IncomeVerificationId);
        Assert.Empty(imported.Lookups); // Reference sheets are not read into the request.
    }

    [Theory]
    [InlineData("Assets", "assetTypeId", "99999")]
    [InlineData("Assets", "assetTypeId", "0")]
    [InlineData("Assets", "assetTypeId", "-1")]
    [InlineData("Incomes", "incomeVerificationId", "99999")]
    [InlineData("Expenses", "expenseTypeId", "31")]
    [InlineData("Incomes", "incomeTypeId", "100")]
    [InlineData("Liabilities", "liabilitySubTypeId", "4")]
    public async Task NumericControlIdsReachTheServiceAndItsValidationErrorIsReturned(string sheet, string column, string value)
    {
        var path = Path.Combine(_directory, "invalid.xlsx"); Example().Save(path);
        using (var excel = new XLWorkbook(path)) { Cell(excel, sheet, column).Value = value; excel.Save(); }

        var loaded = SnapshotWorkbook.Load(path);

        using var handler = new SnapshotTests.RecordingHandler(request =>
        {
            Assert.Equal(HttpMethod.Post, request.Method);
            using var payload = JsonDocument.Parse(request.Content!.ReadAsStringAsync().GetAwaiter().GetResult());
            var submitted = payload.RootElement.GetProperty("data").GetProperty("attributes").GetProperty(sheet.ToLowerInvariant())[0];
            Assert.Equal(int.Parse(value), submitted.GetProperty(column).GetInt32());

            return """{"errors":[{"status":"422","detail":"Invalid control ID or type/subtype combination."}]}""";
        }, HttpStatusCode.UnprocessableEntity);
        using var http = new HttpClient(handler);
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: http) { BaseUrl = "https://example.test" };
        var client = new MyCrmApiClient(adapter);

        var error = await Assert.ThrowsAsync<ErrorDocument>(() => client.Jsonapi.ContactGroups[5001].FinancialSnapshot.PostAsync(loaded.ToDocument()));
        Assert.Equal(422, error.ResponseStatusCode);
        Assert.Equal("Invalid control ID or type/subtype combination.", Assert.Single(error.Errors!).Detail);
        Assert.Single(handler.Requests);
    }

    [Fact]
    public void DropdownDescriptionsAreDisplayHints()
    {
        var path = Path.Combine(_directory, "invalid-label.xlsx");
        Example().Save(path);
        using (var excel = new XLWorkbook(path))
        {
            Cell(excel, "Liabilities", "liabilityTypeId").Value = "7 — Wrong description";
            excel.Save();
        }

        Assert.Equal(7, SnapshotWorkbook.Load(path).Attributes.Liabilities[0].LiabilityTypeId);
    }

    [Fact]
    public void NumericControlIdMissingFromWorkbookDoesNotRequireRefreshingLookups()
    {
        var path = Path.Combine(_directory, "new-control.xlsx");
        Example().Save(path);
        using (var excel = new XLWorkbook(path))
        {
            Cell(excel, "Incomes", "incomeVerificationId").Value = 99;
            excel.Save();
        }

        var loaded = SnapshotWorkbook.Load(path);
        Assert.Equal(99, loaded.Attributes.Incomes[0].IncomeVerificationId);
        Assert.DoesNotContain(loaded.Lookups, item => item.Resource == "income-verification-types" && item.Id == 99);
    }

    [Fact]
    public void EmbeddedLookupMembershipDoesNotDecideWhetherAnIdCanBeSubmitted()
    {
        var book = Example();
        book.Lookups.Add(new("asset-types", 9999, "Invented type"));
        book.Attributes.Assets[0].AssetTypeId = 9999;
        var path = Path.Combine(_directory, "tampered.xlsx"); book.Save(path);
        var loaded = SnapshotWorkbook.Load(path);
        Assert.Equal(9999, loaded.Attributes.Assets[0].AssetTypeId);
    }

    [Fact]
    public void CurrentFormatIsVersionOneAndReferenceSheetsAreNotRequiredOnImport()
    {
        var path = Path.Combine(_directory, "current.xlsx");
        Example().Save(path);

        using (var excel = new XLWorkbook(path))
        {
            Assert.Equal("1", excel.Worksheet("Snapshot").Cell("B2").GetString());
            excel.Worksheet("Lookups").Delete();
            excel.Worksheet("Contact Lookups").Delete();
            excel.Save();
        }

        var loaded = SnapshotWorkbook.Load(path);
        Assert.Equal(1, loaded.Attributes.Assets[0].AssetTypeId);
        Assert.Equal(1001, loaded.Attributes.Assets[0].Ownership[0].Id);
    }

    [Theory]
    [InlineData("2")]
    [InlineData("0")]
    [InlineData("")]
    public void RejectsUnsupportedOrMissingWorkbookVersion(string version)
    {
        var path = Path.Combine(_directory, "unsupported.xlsx");
        Example().Save(path);

        using (var excel = new XLWorkbook(path))
        {
            excel.Worksheet("Snapshot").Cell("B2").Value = version;
            excel.Save();
        }

        Assert.Contains("unsupported or missing formatVersion", Assert.Throws<FormatException>(() => SnapshotWorkbook.Load(path)).Message);
    }

    [Fact]
    public async Task ControlReadsFollowPagesAndRetainCategoryAndSubtypeRelationships()
    {
        using var handler = new SnapshotTests.RecordingHandler(request =>
        {
            var resource = request.RequestUri!.AbsolutePath.Split('/').Last();
            var isPageTwo = Uri.UnescapeDataString(request.RequestUri.Query).Contains("page[number]=2");
            var controls = Controls().Where(c => c.Resource == resource).ToArray();
            object Resource(SnapshotLookup c) => new
            {
                type = c.Resource, id = c.Id.ToString(),
                attributes = c.Resource.EndsWith("categories") ? (object)new { description = c.Name } : new { name = c.Name },
                relationships = c.Resource == "liability-types" ? (object)new
                {
                    liabilityCategory = new { data = new { type = c.ParentResource, id = c.ParentId.ToString() } },
                    liabilitySubTypes = new { data = Controls().Where(s => s.Resource == "liability-sub-types" && s.ParentId == c.Id).Select(s => new { type = s.Resource, id = s.Id.ToString() }) }
                } : c.ParentId.HasValue ? new Dictionary<string, object> { [c.Resource == "income-types" ? "incomeCategory" : "expenseCategory"] = new { data = new { type = c.ParentResource, id = c.ParentId.ToString() } } } : new { }
            };
            if (resource == "liability-categories") controls = controls.Where(c => c.Id == (isPageTwo ? 2 : 1)).ToArray();
            return JsonSerializer.Serialize(new
            {
                data = controls.Select(Resource),
                included = resource == "liability-types" ? Controls().Where(c => c.Resource == "liability-sub-types").Select(Resource) : [],
                links = new { next = resource == "liability-categories" && !isPageTwo ? "?page[number]=2" : null }
            });
        });
        using var http = new HttpClient(handler);
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: http) { BaseUrl = "https://example.test" };
        var controls = await new SnapshotApi(new MyCrmApiClient(adapter), adapter.BaseUrl).LookupsAsync();
        Assert.Equal("Unsecured", controls.Single(c => c.Resource == "liability-categories" && c.Id == 2).Name);
        Assert.Equal(2, controls.Single(c => c.Resource == "liability-types" && c.Id == 7).ParentId);
        Assert.Equal(7, controls.Single(c => c.Resource == "liability-sub-types" && c.Id == 4).ParentId);
        Assert.Contains(handler.Requests, r => r.Contains("/liability-categories?page[number]=2"));
        Assert.Contains(handler.Requests, r => r.Contains("include=liabilityCategory,liabilitySubTypes"));
        Assert.Contains(controls, c => c.Resource == "income-verification-types");
        Assert.Contains(controls, c => c.Resource == "rental-verification-types");
    }

    private static IXLCell Cell(XLWorkbook book, string sheet, string header) => book.Worksheet(sheet).Cell(2,
        book.Worksheet(sheet).Row(1).CellsUsed().Single(c => c.GetString() == header).Address.ColumnNumber);
}
