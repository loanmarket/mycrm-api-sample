using ClosedXML.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Validation;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Http.HttpClientLibrary;
using MyCrmSampleClient.FinancialSnapshots;
using MyCrmSampleClient.Kiota;
using MyCrmSampleClient.Kiota.Models;
using Xunit;
using Spreadsheet = DocumentFormat.OpenXml.Spreadsheet;

namespace MyCrmSampleClient.Tests;

public sealed class SnapshotLayoutTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), "mycrm-layout-" + Guid.NewGuid().ToString("N"));
    private static readonly Contact[] Contacts =
    [
        new() { Id = "1001", Attributes = new() { Role = ContactAttributes_role.Adult } },
        new() { Id = "1002", Attributes = new() { Role = ContactAttributes_role.Child } }
    ];

    public SnapshotLayoutTests() => Directory.CreateDirectory(_directory);

    public void Dispose() => Directory.Delete(_directory, true);

    private static SnapshotWorkbook Example() => SnapshotExamples.Create(5001, Contacts, SnapshotExampleData.MinimalControls(), "AU", seed: 12345);

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void WorkbookKeepsUnusedInputRowsOutOfTheFileWhilePreservingFormatsAndDropdowns(bool populated)
    {
        var snapshot = populated ? Example() : new SnapshotWorkbook { ContactGroupId = 5001 };
        var path = Path.Combine(_directory, "compact.xlsx");
        snapshot.Save(path);

        using (var document = SpreadsheetDocument.Open(path, false))
        {
            var workbook = document.WorkbookPart!;
            foreach (var sheet in workbook.Workbook.Sheets!.Elements<Spreadsheet.Sheet>()
                .Where(sheet => SnapshotWorkbook.DataSheets.Contains(sheet.Name!.Value)))
            {
                var part = (WorksheetPart)workbook.GetPartById(sheet.Id!);
                var lastRow = Math.Max(2, snapshot.Rows(sheet.Name!).Count + 1);
                var rows = part.Worksheet.GetFirstChild<Spreadsheet.SheetData>()!.Elements<Spreadsheet.Row>();

                // Inspect persisted rows: library round trips alone can hide unused-cell bloat.
                Assert.All(rows, row => Assert.InRange(row.RowIndex!.Value, 1u, (uint)lastRow));
                Assert.All(part.Worksheet.GetFirstChild<Spreadsheet.Columns>()!.Elements<Spreadsheet.Column>(),
                    column => Assert.True(column.Max!.Value <= SnapshotWorkbook.Columns(sheet.Name!).Count));
            }

            Assert.Empty(new OpenXmlValidator().Validate(document));
        }

        using var excel = new XLWorkbook(path);
        foreach (var sheet in SnapshotWorkbook.DataSheets)
        {
            var ws = excel.Worksheet(sheet);
            foreach (var (column, index) in SnapshotWorkbook.Columns(sheet).Select((column, index) => (column, index)))
            {
                var expectedFormat = column.Type == typeof(string) || column.Property.Name == "Ownership" ? "@"
                    : column.Type == typeof(double) ? "#,##0.00"
                    : column.Type == typeof(Microsoft.Kiota.Abstractions.Date) ? "yyyy-mm-dd" : null;

                if (expectedFormat != null)
                    Assert.Equal(expectedFormat, ws.Cell(1000, index + 1).Style.NumberFormat.Format);

                if (column.Type.IsEnum || SnapshotLinkData.IsContactColumn(column.Header) || column.Child != null)
                {
                    foreach (var row in new[] { 2, 1000 })
                    {
                        Assert.False(ws.Cell(row, index + 1).Style.Alignment.WrapText);
                        Assert.True(ws.Cell(row, index + 1).Style.Alignment.ShrinkToFit);
                    }

                    Assert.True(ws.Cell(1, index + 1).Style.Alignment.WrapText);
                    Assert.False(ws.Cell(1, index + 1).Style.Alignment.ShrinkToFit);
                }
            }

            Assert.All(ws.DataValidations.SelectMany(validation => validation.Ranges), range =>
            {
                Assert.Equal(2, range.RangeAddress.FirstAddress.RowNumber);
                Assert.Equal(1000, range.RangeAddress.LastAddress.RowNumber);
            });
        }
    }

    [Fact]
    public void DropdownsExtendToIncludeAllInitiallyExportedRecords()
    {
        var snapshot = new SnapshotWorkbook { ContactGroupId = 5001 };
        snapshot.Lookups.Add(new SnapshotLookup("asset-types", 1, "Real Estate"));
        while (snapshot.Attributes.Assets.Count < 1005)
            snapshot.Attributes.Assets.Add(new() { AssetTypeId = 1 });

        var path = Path.Combine(_directory, "large.xlsx");
        snapshot.Save(path);

        using var excel = new XLWorkbook(path);
        var ws = excel.Worksheet("Assets");
        Assert.NotEmpty(ws.DataValidations);
        Assert.All(ws.DataValidations.SelectMany(validation => validation.Ranges), range =>
            Assert.Equal(1006, range.RangeAddress.LastAddress.RowNumber));
        Assert.Equal(1005, SnapshotWorkbook.Load(path).Attributes.Assets.Count);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void EveryEnumHasMatchingDropdownChoicesThroughRow1000(bool populated)
    {
        var snapshot = populated ? Example() : new SnapshotWorkbook { ContactGroupId = 5001 };
        var path = Path.Combine(_directory, "enums.xlsx");
        snapshot.Save(path);

        using (var excel = new XLWorkbook(path))
        {
            foreach (var sheet in SnapshotWorkbook.DataSheets)
            {
                foreach (var column in SnapshotWorkbook.Columns(sheet).Where(c => c.Type.IsEnum))
                {
                    var cell = Cell(excel, sheet, column.Header);
                    var validation = cell.GetDataValidation();
                    Assert.Equal(XLAllowedValues.List, validation.AllowedValues);
                    Assert.True(validation.InCellDropdown);
                    Assert.True(validation.ShowErrorMessage);
                    Assert.True(validation.IgnoreBlanks);
                    Assert.Equal(XLErrorStyle.Stop, validation.ErrorStyle);
                    Assert.Contains(validation.Ranges, range => range.Contains(excel.Worksheet(sheet).Cell(1000, cell.Address.ColumnNumber)));
                    Assert.DoesNotContain(validation.Ranges, range => range.Contains(excel.Worksheet(sheet).Cell(1001, cell.Address.ColumnNumber)));

                    var choices = excel.DefinedName(validation.Value).Ranges.Single().Cells().Select(c => c.GetString());
                    Assert.Equal(Enum.GetNames(column.Type), choices);
                    Assert.Equal(XLWorksheetVisibility.VeryHidden, excel.DefinedName(validation.Value).Ranges.Single().Worksheet.Visibility);
                }
            }
        }

        using var document = SpreadsheetDocument.Open(path, false);
        Assert.Empty(new OpenXmlValidator().Validate(document));
    }

    [Fact]
    public void EveryEnumChoiceRoundTripsThroughTheWorkbook()
    {
        var snapshot = new SnapshotWorkbook { ContactGroupId = 5001 };
        foreach (var sheet in SnapshotWorkbook.DataSheets)
        {
            foreach (var column in SnapshotWorkbook.Columns(sheet).Where(c => c.Type.IsEnum))
            {
                foreach (var value in Enum.GetValues(column.Type))
                {
                    var row = Activator.CreateInstance(SnapshotWorkbook.RowType(sheet))!;
                    column.Set(row, value);
                    snapshot.Rows(sheet).Add(row);
                }
            }
        }

        var path = Path.Combine(_directory, "enum-values.xlsx");
        snapshot.Save(path);
        var imported = SnapshotWorkbook.Load(path);

        foreach (var sheet in SnapshotWorkbook.DataSheets)
        {
            Assert.Equal(snapshot.Rows(sheet).Count, imported.Rows(sheet).Count);
            foreach (var column in SnapshotWorkbook.Columns(sheet).Where(c => c.Type.IsEnum))
            {
                for (var r = 0; r < snapshot.Rows(sheet).Count; r++)
                    Assert.Equal(column.Get(snapshot.Rows(sheet)[r]), column.Get(imported.Rows(sheet)[r]));
            }
        }
    }

    [Fact]
    public void EverySheetPutsIdentityAndSortedReferencesBeforeSortedFields()
    {
        var prefixes = new Dictionary<string, string[]>
        {
            ["Assets"] = ["lid", "assetId", "assetTypeId", "assetSubTypeId", "address.lid", "owner1ContactId", "owner2ContactId", "propertyTypeId"],
            ["Expenses"] = ["lid", "expenseId", "expenseTypeId", "owner1ContactId", "owner2ContactId"],
            ["Incomes"] = ["lid", "incomeId", "incomeTypeId", "employment.id", "employment.lid", "incomeVerificationId", "linkedAsset.id", "linkedAsset.lid", "nzRentalVerificationTypeId", "owner1ContactId", "owner2ContactId"],
            ["Liabilities"] = ["lid", "liabilityId", "liabilityTypeId", "liabilitySubTypeId", "linkedAsset.id", "linkedAsset.lid", "owner1ContactId", "owner2ContactId"],
            ["Employments"] = ["lid", "employmentId", "address.lid", "contactId"],
            ["Addresses"] = ["lid"]
        };
        var path = Path.Combine(_directory, "layout.xlsx");
        Example().Save(path);

        using var excel = new XLWorkbook(path);
        foreach (var (sheet, prefix) in prefixes)
        {
            var headers = excel.Worksheet(sheet).Row(1).CellsUsed().Select(c => c.GetString()).ToArray();
            Assert.Equal(prefix, headers.Take(prefix.Length));

            var fields = headers.Skip(prefix.Length).ToArray();
            Assert.Equal(fields.OrderBy(h => h, StringComparer.Ordinal), fields);
        }
    }

    [Theory]
    [InlineData("Assets", "address.lid", "Addresses", "lid")]
    [InlineData("Employments", "address.lid", "Addresses", "lid")]
    [InlineData("Liabilities", "linkedAsset.lid", "Assets", "lid")]
    [InlineData("Incomes", "linkedAsset.lid", "Assets", "lid")]
    [InlineData("Incomes", "employment.lid", "Employments", "lid")]
    [InlineData("Liabilities", "linkedAsset.id", "Assets", "assetId")]
    [InlineData("Incomes", "linkedAsset.id", "Assets", "assetId")]
    [InlineData("Incomes", "employment.id", "Employments", "employmentId")]
    public void LinkDropdownsFollowEditedAndAddedTargetRows(string sheet, string header, string targetSheet, string targetHeader)
    {
        var path = Path.Combine(_directory, "links.xlsx");
        Example().Save(path);

        using (var excel = new XLWorkbook(path))
        {
            var validation = Cell(excel, sheet, header).GetDataValidation();
            Assert.Equal(XLAllowedValues.List, validation.AllowedValues);
            Assert.True(validation.InCellDropdown);
            Assert.True(validation.ShowErrorMessage);
            Assert.Equal(header.EndsWith(".id") ? XLErrorStyle.Warning : XLErrorStyle.Stop, validation.ErrorStyle);
            var columnNumber = Cell(excel, sheet, header).Address.ColumnNumber;
            Assert.Contains(validation.Ranges, range => range.Contains(excel.Worksheet(sheet).Cell(1000, columnNumber)));
            Assert.DoesNotContain(validation.Ranges, range => range.Contains(excel.Worksheet(sheet).Cell(1001, columnNumber)));

            var target = Cell(excel, targetSheet, targetHeader);
            target.Value = targetHeader == "lid" ? "edited-local-id" : "8101";

            // Keep an empty row before a new record to check that gaps do not hide choices.
            var table = excel.Worksheet(targetSheet).Tables.Single();
            var rowNumber = table.RangeAddress.LastAddress.RowNumber + 2;
            var newTarget = excel.Worksheet(targetSheet).Cell(rowNumber, target.Address.ColumnNumber);
            newTarget.Value = targetHeader == "lid" ? "new-local-id" : "8102";
            table.Resize(excel.Worksheet(targetSheet).Range(1, 1, rowNumber, table.ColumnCount()));

            var choices = excel.DefinedName(validation.Value).Ranges.Single().Cells().Select(c => c.GetString()).ToArray();
            Assert.Contains(target.GetString(), choices);
            Assert.Contains(newTarget.GetString(), choices);
            excel.Save();
        }

        using var saved = new XLWorkbook(path);
        var savedValidation = Cell(saved, sheet, header).GetDataValidation();
        Assert.Contains(saved.DefinedName(savedValidation.Value).Ranges.Single().Cells(), cell => cell.GetString() == (targetHeader == "lid" ? "new-local-id" : "8102"));

        using var document = SpreadsheetDocument.Open(path, false);
        Assert.Empty(new OpenXmlValidator().Validate(document));
    }

    [Fact]
    public void ContactDropdownOffersOnlyAdultContacts()
    {
        var path = Path.Combine(_directory, "contacts.xlsx");
        Example().Save(path);

        using var excel = new XLWorkbook(path);
        var validation = Cell(excel, "Employments", "contactId").GetDataValidation();
        Assert.Equal(XLErrorStyle.Stop, validation.ErrorStyle);
        Assert.Equal(["1001"], excel.DefinedName(validation.Value).Ranges.Single().Cells().Select(c => c.GetString()));
    }

    [Fact]
    public void EmptyTablesRemainEmptyOnImportAndRetainLinkDropdowns()
    {
        var snapshot = new SnapshotWorkbook { ContactGroupId = 5001 };
        var path = Path.Combine(_directory, "empty.xlsx");
        snapshot.Save(path);

        var imported = SnapshotWorkbook.Load(path);
        Assert.All(SnapshotWorkbook.DataSheets, sheet => Assert.Empty(imported.Rows(sheet)));

        using var excel = new XLWorkbook(path);
        var validation = Cell(excel, "Assets", "address.lid").GetDataValidation();
        Assert.True(excel.DefinedName(validation.Value).Ranges.Single().FirstCell().IsEmpty());
    }

    [Fact]
    public async Task ReorderedColumnsImportWithoutChangingTheRequest()
    {
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider());
        _ = new MyCrmApiClient(adapter);
        var snapshot = Example();
        var path = Path.Combine(_directory, "current.xlsx");
        snapshot.Save(path);

        var reorderedPath = Path.Combine(_directory, "reordered.xlsx");
        using (var current = new XLWorkbook(path))
        using (var reordered = new XLWorkbook())
        {
            current.Worksheet("Snapshot").CopyTo(reordered, "Snapshot");
            current.Worksheet("Lookups").CopyTo(reordered, "Lookups");

            foreach (var sheet in SnapshotWorkbook.DataSheets)
            {
                var ws = reordered.AddWorksheet(sheet);
                var columns = SnapshotWorkbook.Columns(sheet).Reverse().ToArray();

                for (var c = 0; c < columns.Length; c++)
                {
                    var source = Cell(current, sheet, columns[c].Header).Address.ColumnNumber;
                    for (var r = 1; r <= snapshot.Rows(sheet).Count + 1; r++)
                        ws.Cell(r, c + 1).Value = current.Worksheet(sheet).Cell(r, source).Value;
                }
            }

            reordered.SaveAs(reorderedPath);
        }

        var imported = SnapshotWorkbook.Load(reorderedPath);
        Assert.Equal(await KiotaJsonSerializer.SerializeAsStringAsync(snapshot.ToDocument()),
            await KiotaJsonSerializer.SerializeAsStringAsync(imported.ToDocument()));
    }

    [Fact]
    public void ImportPreservesMissingLocalLinksAndExistingIdsOutsideWorkbook()
    {
        var snapshot = Example();
        snapshot.Attributes.Liabilities[0].LinkedAsset = new() { Id = 9876 };
        snapshot.Attributes.Assets[0].Address.Lid = "missing-address";
        var path = Path.Combine(_directory, "existing-id.xlsx");
        snapshot.Save(path);

        var imported = SnapshotWorkbook.Load(path);
        Assert.Equal(9876, imported.Attributes.Liabilities[0].LinkedAsset.Id);
        Assert.Equal("missing-address", imported.Attributes.Assets[0].Address.Lid);
    }

    private static IXLCell Cell(XLWorkbook book, string sheet, string header) => book.Worksheet(sheet).Cell(2,
        book.Worksheet(sheet).Row(1).CellsUsed().Single(c => c.GetString() == header).Address.ColumnNumber);
}
