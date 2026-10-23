using ClosedXML.Excel;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Http.HttpClientLibrary;
using MyCrmSampleClient.FinancialSnapshots;
using MyCrmSampleClient.Kiota;
using MyCrmSampleClient.Kiota.Models;
using Xunit;

namespace MyCrmSampleClient.Tests;

public sealed class SnapshotOwnershipTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), "mycrm-owners-" + Guid.NewGuid().ToString("N"));
    private static readonly Contact[] Contacts =
    [
        new() { Id = "1001", Attributes = new() { FirstName = "Alex", LastName = "Taylor", Role = ContactAttributes_role.Adult, IsPrimary = true } },
        new() { Id = "1002", Attributes = new() { FirstName = "Alex", LastName = "Taylor", Role = ContactAttributes_role.Adult, IsPrimary = false } },
        new() { Id = "1003", Attributes = new() { FirstName = "Sam", Role = ContactAttributes_role.Child } }
    ];

    public SnapshotOwnershipTests() => Directory.CreateDirectory(_directory);

    public void Dispose() => Directory.Delete(_directory, true);

    private static SnapshotWorkbook Example() => SnapshotExamples.Create(5001, Contacts, SnapshotExampleData.MinimalControls(), "AU", seed: 12345);

    [Fact]
    public void BothOwnerColumnsOfferDistinctAdultIdsWithReadableNamesOnEveryFinancialSheet()
    {
        var path = Path.Combine(_directory, "owners.xlsx");
        Example().Save(path);

        using var excel = new XLWorkbook(path);
        Assert.Equal("1", excel.Worksheet("Snapshot").Cell("B2").GetString());

        foreach (var sheet in new[] { "Assets", "Expenses", "Incomes", "Liabilities" })
        {
            Assert.DoesNotContain(excel.Worksheet(sheet).Row(1).CellsUsed(), cell => cell.GetString() == "ownerContactIds");

            foreach (var header in new[] { "owner1ContactId", "owner2ContactId" })
            {
                var cell = Cell(excel, sheet, header);
                var validation = cell.GetDataValidation();
                Assert.Equal(XLAllowedValues.List, validation.AllowedValues);
                Assert.True(validation.InCellDropdown);
                Assert.True(validation.IgnoreBlanks);
                Assert.True(validation.ShowErrorMessage);
                Assert.Equal(XLErrorStyle.Stop, validation.ErrorStyle);
                Assert.False(cell.Style.Alignment.WrapText);
                Assert.True(cell.Style.Alignment.ShrinkToFit);
                Assert.True(cell.WorksheetColumn().Width >= 44);

                var choices = excel.DefinedName(validation.Value).Ranges.Single().Cells().Select(choice => choice.GetString());
                Assert.Equal(["1001 — Alex Taylor", "1002 — Alex Taylor"], choices);
                if (!cell.IsEmpty()) Assert.Contains(cell.GetString(), choices);
            }
        }

        Assert.Equal("1001 — Alex Taylor", Cell(excel, "Assets", "owner1ContactId").GetString());
        Assert.Equal("1002 — Alex Taylor", Cell(excel, "Assets", "owner2ContactId").GetString());
        Assert.Equal("1001 — Alex Taylor", Cell(excel, "Employments", "contactId").GetString());
        Assert.False(Cell(excel, "Employments", "contactId").Style.Alignment.WrapText);
        Assert.True(Cell(excel, "Employments", "contactId").Style.Alignment.ShrinkToFit);
    }

    [Theory]
    [InlineData("Assets", "owner1ContactId", "owner2ContactId")]
    [InlineData("Assets", "owner2ContactId", "owner1ContactId")]
    [InlineData("Expenses", "owner2ContactId", "owner1ContactId")]
    [InlineData("Incomes", "owner2ContactId", "owner1ContactId")]
    [InlineData("Liabilities", "owner2ContactId", "owner1ContactId")]
    public void SingleOwnerCanUseEitherColumnWithoutCreatingAnEmptyOwner(string sheet, string selected, string blank)
    {
        var path = Path.Combine(_directory, "single-owner.xlsx");
        Example().Save(path);

        using (var excel = new XLWorkbook(path))
        {
            Cell(excel, sheet, selected).Value = "1001 — Alex Taylor";
            Cell(excel, sheet, blank).Clear(XLClearOptions.Contents);
            excel.Save();
        }

        var imported = SnapshotWorkbook.Load(path);

        var row = imported.Rows(sheet)[0]!;
        var owners = (List<FinancialSnapshotContactReference>)row.GetType().GetProperty("Ownership")!.GetValue(row)!;
        Assert.Equal(1001, Assert.Single(owners).Id);
    }

    [Theory]
    [InlineData("1001", "1001", new int[] { 1001, 1001 })]
    [InlineData("1003 — Sam", "", new int[] { 1003 })]
    [InlineData("9999 — Unknown", "", new int[] { 9999 })]
    [InlineData("0", "-1", new int[] { 0, -1 })]
    [InlineData("", "", new int[] { })]
    public void ImportPreservesOwnerSelectionsForServerValidation(string first, string second, int[] expected)
    {
        var path = Path.Combine(_directory, "invalid-owner.xlsx");
        Example().Save(path);

        using (var excel = new XLWorkbook(path))
        {
            Cell(excel, "Assets", "owner1ContactId").Value = first;
            Cell(excel, "Assets", "owner2ContactId").Value = second;
            excel.Save();
        }

        var imported = SnapshotWorkbook.Load(path);
        Assert.Equal(expected, imported.Attributes.Assets[0].Ownership?.Select(owner => owner.Id!.Value) ?? []);
    }

    [Fact]
    public async Task ReorderedOwnerColumnsPreserveBothIdsAndTheSubmittedRequest()
    {
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider());
        _ = new MyCrmApiClient(adapter);
        var source = Example();
        var path = Path.Combine(_directory, "reordered-owners.xlsx");
        source.Save(path);

        using (var excel = new XLWorkbook(path))
        {
            var ws = excel.Worksheet("Assets");
            var first = Cell(excel, "Assets", "owner1ContactId").Address.ColumnNumber;
            var second = Cell(excel, "Assets", "owner2ContactId").Address.ColumnNumber;

            // Swap headers through a temporary name so table field names stay unique.
            ws.Cell(1, first).Value = "temporaryOwner";
            ws.Cell(1, second).Value = "owner1ContactId";
            ws.Cell(1, first).Value = "owner2ContactId";
            for (var r = 2; r <= source.Attributes.Assets.Count + 1; r++)
            {
                var value = ws.Cell(r, first).Value;
                ws.Cell(r, first).Value = ws.Cell(r, second).Value;
                ws.Cell(r, second).Value = value;
            }

            excel.Save();
        }

        var imported = SnapshotWorkbook.Load(path);
        Assert.Equal(await KiotaJsonSerializer.SerializeAsStringAsync(source.ToDocument()),
            await KiotaJsonSerializer.SerializeAsStringAsync(imported.ToDocument()));
    }

    [Fact]
    public void MoreThanTwoOwnersCannotBeSilentlyTruncatedOnExport()
    {
        var snapshot = Example();
        snapshot.Attributes.Assets[0].Ownership.Add(new() { Id = 1004 });
        var path = Path.Combine(_directory, "existing.xlsx");
        File.WriteAllText(path, "Existing file must be kept");

        Assert.Contains("at most two owners", Assert.Throws<FormatException>(() => snapshot.Save(path, overwrite: true)).Message);
        Assert.Equal("Existing file must be kept", File.ReadAllText(path));
    }

    private static IXLCell Cell(XLWorkbook book, string sheet, string header) =>
        book.Worksheet(sheet).Cell(2, book.Worksheet(sheet).Row(1).CellsUsed().Single(cell => cell.GetString() == header).Address.ColumnNumber);
}
