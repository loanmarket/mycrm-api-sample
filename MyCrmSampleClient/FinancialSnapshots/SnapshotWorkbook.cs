using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using ClosedXML.Excel;
using Microsoft.Kiota.Abstractions;
using MyCrmSampleClient.Kiota.Models;

namespace MyCrmSampleClient.FinancialSnapshots;

/// <summary>The same versioned, flat Excel contract is used for export, examples and import.</summary>
public sealed class SnapshotWorkbook
{
    public const string FormatVersion = "1";

    public int ContactGroupId { get; set; }
    public string Source { get; set; } = "Manual";
    public FinancialSnapshotAttributes Attributes { get; } = new()
    {
        Assets = [], Expenses = [], Incomes = [], Liabilities = [], Employments = [], Addresses = []
    };

    public List<string> Notes { get; } = [];
    public List<Contact> Contacts { get; } = [];
    public List<SnapshotLookup> Lookups { get; } = [];
    public List<string[]> Excluded { get; } = [];

    public static readonly string[] Collections = ["Assets", "Expenses", "Incomes", "Liabilities", "Employments"];
    public static readonly string[] DataSheets = [.. Collections, "Addresses"];

    public SnapshotWorkbook()
    {
        foreach (var collection in Collections) SetMode(collection, "Merge");
    }

    public IList Rows(string sheet) => (IList)typeof(FinancialSnapshotAttributes).GetProperty(sheet)!.GetValue(Attributes)!;

    internal int LastInputRow(string sheet) => Math.Max(1000, Rows(sheet).Count + 1);

    public string Mode(string sheet) => typeof(FinancialSnapshotAttributes).GetProperty(sheet + "Mode")!.GetValue(Attributes)?.ToString() ?? "Merge";

    public void SetMode(string sheet, string mode)
    {
        if (mode is not ("Merge" or "Replace")) throw new FormatException($"{sheet} mode must be Merge or Replace.");

        var property = typeof(FinancialSnapshotAttributes).GetProperty(sheet + "Mode")!;
        property.SetValue(Attributes, Enum.Parse(Nullable.GetUnderlyingType(property.PropertyType)!, mode));
    }

    public FinancialSnapshotDocument ToDocument() => new()
    {
        Data = new FinancialSnapshot { Type = "financial-snapshots", Attributes = Attributes }
    };

    // Derive columns from the generated input DTOs, never from read-resource attributes.
    // Ownership has two contact columns; references use address.lid, linkedAsset.id/lid, etc.
    public sealed record Column(string Header, PropertyInfo Property, PropertyInfo Child = null, int? OwnerIndex = null)
    {
        public Type Type => OwnerIndex.HasValue ? typeof(int) : Nullable.GetUnderlyingType((Child ?? Property).PropertyType) ?? (Child ?? Property).PropertyType;

        public object Get(object row)
        {
            if (OwnerIndex.HasValue)
            {
                var owners = (List<FinancialSnapshotContactReference>)Property.GetValue(row);
                if (owners?.Count > 2) throw new FormatException("A workbook row supports at most two owners; export stopped to avoid dropping ownership.");

                return owners?.ElementAtOrDefault(OwnerIndex.Value)?.Id;
            }

            return Child == null ? Property.GetValue(row) : Property.GetValue(row) is { } parent ? Child.GetValue(parent) : null;
        }

        public void Set(object row, object value)
        {
            if (OwnerIndex.HasValue)
            {
                var owners = (List<FinancialSnapshotContactReference>)Property.GetValue(row) ?? [];
                while (owners.Count <= OwnerIndex.Value) owners.Add(null);

                owners[OwnerIndex.Value] = new FinancialSnapshotContactReference { Id = (int)value };
                Property.SetValue(row, owners);
            }
            else if (Child == null) Property.SetValue(row, value);
            else
            {
                var parent = Property.GetValue(row) ?? Activator.CreateInstance(Property.PropertyType)!;
                Property.SetValue(row, parent);
                Child.SetValue(parent, value);
            }
        }
    }

    public static Type RowType(string sheet) => typeof(FinancialSnapshotAttributes).GetProperty(sheet)!.PropertyType.GetGenericArguments()[0];

    private static string Camel(string name) => char.ToLowerInvariant(name[0]) + name[1..];

    public static IReadOnlyList<Column> Columns(string sheet) => RowType(sheet).GetProperties()
        .Where(p => p.Name != "AdditionalData")
        .SelectMany(p => p.Name == "Ownership"
            ? new[] { new Column("owner1ContactId", p, OwnerIndex: 0), new Column("owner2ContactId", p, OwnerIndex: 1) }
            : p.Name is "Address" or "LinkedAsset" or "Employment"
            ? p.PropertyType.GetProperties().Where(c => c.Name != "AdditionalData").Select(c => new Column(Camel(p.Name) + "." + Camel(c.Name), p, c))
            : [new Column(Camel(p.Name), p)])
        .OrderBy(c => ColumnGroup(sheet, c.Header))
        .ThenBy(c => c.Header, StringComparer.Ordinal)
        .ToArray();

    private static int ColumnGroup(string sheet, string header)
    {
        var record = Camel(RowType(sheet).Name["FinancialSnapshot".Length..]);

        if (header == "lid") return 0;
        if (header == record + "Id") return 1;
        if (header == record + "TypeId") return 2;
        if (header == record + "SubTypeId") return 3;
        if (header.EndsWith("Id", StringComparison.Ordinal) || header.EndsWith(".id", StringComparison.Ordinal) ||
            header.EndsWith(".lid", StringComparison.Ordinal)) return 4;

        return 5;
    }

    public void Save(string path, bool overwrite = false)
    {
        using var book = new XLWorkbook();

        WriteTable(book, "Snapshot", ["setting", "value"],
            new[] { new[] { "formatVersion", FormatVersion }, new[] { "contactGroupId", ContactGroupId.ToString(CultureInfo.InvariantCulture) }, new[] { "source", Source } }
                .Concat(Collections.Select(s => new[] { Camel(s) + "Mode", Mode(s) })));
        book.Worksheet("Snapshot").Range("B5:B9").CreateDataValidation().List("\"Merge,Replace\"", true);

        foreach (var sheet in DataSheets)
        {
            var ws = book.AddWorksheet(sheet);
            var columns = Columns(sheet);

            for (var c = 0; c < columns.Count; c++)
            {
                var column = columns[c];
                ws.Cell(1, c + 1).Value = column.Header;

                // Column defaults preserve input formats without storing thousands of blank cells.
                var format = ws.Column(c + 1).Style.NumberFormat;
                if (column.Type == typeof(string) || column.Property.Name == "Ownership") format.Format = "@";
                if (column.Type == typeof(double)) format.Format = "#,##0.00";
                if (column.Type == typeof(Date)) format.Format = "yyyy-mm-dd";

                if (column.Type == typeof(bool))
                    ws.Range(2, c + 1, LastInputRow(sheet), c + 1).CreateDataValidation().List("\"TRUE,FALSE\"", true);

                var r = 2;
                foreach (var row in Rows(sheet))
                {
                    var value = column.Get(row);
                    if (value is int contactId && SnapshotLinkData.IsContactColumn(column.Header))
                    {
                        var contact = Contacts.FirstOrDefault(item => PositiveId(item.Id) == contactId);
                        if (contact != null) value = SnapshotLinkData.DisplayContact(contact);
                    }
                    else if (value is int id && SnapshotControlData.Columns.TryGetValue(column.Header, out var resource))
                    {
                        var control = Lookups.FirstOrDefault(item => item.Resource == resource && item.Id == id);
                        if (control != null) value = SnapshotControlData.Display(control, Lookups);
                    }

                    WriteCell(ws.Cell(r++, c + 1), value);
                }
            }

            Style(ws, columns.Count, Rows(sheet).Count + 1);
        }

        WriteContacts(book);
        WriteTable(book, "Lookups", ["resource", "id", "name", "parentResource", "parentId"], Lookups.Select(c => new[] { c.Resource, c.Id.ToString(CultureInfo.InvariantCulture), c.Name, c.ParentResource ?? "", c.ParentId?.ToString(CultureInfo.InvariantCulture) ?? "" }));
        SnapshotControlData.AddDropdowns(book, this);
        SnapshotLinkData.AddDropdowns(book, this);
        SnapshotEnumData.AddDropdowns(book, this);

        WriteTable(book, "Excluded records", ["resource", "id", "type", "value", "frequency", "description"], Excluded);
        WriteTable(book, "Read me", ["topic", "guidance"], new[]
        {
            new[] { "Format", "Keep every data sheet and column header. Blank cells mean omitted optional values; matched records can clear those values. No formulas in imported cells." },
            new[] { "Modes", "Set each collection to Merge or Replace on Snapshot. Replace with an empty sheet removes all eligible records in that collection for the whole group." },
            new[] { "IDs", "assetId/expenseId/incomeId/liabilityId/employmentId select existing records. Leave blank for automatic matching. Keep account numbers, BSBs and postcodes as text." },
            new[] { "Owners", "Choose up to two distinct adults in owner1ContactId and owner2ContactId. One column may be blank for a single owner. Dropdowns show contact ID and name; import uses the IDs and divides shares equally." },
            new[] { "Links", "Choose address.lid, linkedAsset.lid and employment.lid from the corresponding sheet. Add rows within the Excel tables to extend the lists. Use exactly one of .id or .lid; existing ID links also allow typed IDs outside the workbook. Local IDs are case-sensitive." },
            new[] { "Amounts", "Use local currency amounts and explicit frequencies. interestRate is in percentage points: 6.25 means 6.25%. loanTerm is years; NZ term fields are months." },
            new[] { "Dates", "Use Excel dates or YYYY-MM-DD. Enums use the dropdown spelling. Contact Lookups and Lookups are reference sheets, never submitted." },
            new[] { "Contact Lookups", "Reference-only contactId, Name, Gender and isPrimary values. Changing this sheet does not update contacts. Ownership and employment dropdowns offer Adult contacts; the snapshot service validates submitted owners and employment contacts." },
            new[] { "Control data", "Type, subtype, property and verification ID columns have dropdowns showing ID — category / name. Numeric IDs can also be entered, including values outside the dropdown. The snapshot service validates IDs and type/subtype combinations when submitted." },
            new[] { "Repayments", "Enter repayment and repaymentFrequency on Liabilities. Do not also submit managed repayment expenses." },
            new[] { "Export", "Read endpoints are not a transactional snapshot. Review notes and current data before importing. Unsupported records may need removal from the input sheets; server validation remains authoritative." },
            new[] { "Examples", "Fabricated values are examples only. Real contact and lookup IDs are selected from your accessible group and reference data." }
        }.Concat(Notes.Select(n => new[] { "Export note", n })));

        var fullPath = Path.GetFullPath(path);
        var directory = Path.GetDirectoryName(fullPath)!;
        Directory.CreateDirectory(directory);

        // Finish writing before replacing an existing file; overwriting requires explicit opt-in.
        var temporaryPath = Path.Combine(directory, $".snapshot-{Guid.NewGuid():N}.tmp");
        try
        {
            using (var output = new FileStream(temporaryPath, FileMode.CreateNew, FileAccess.Write))
                book.SaveAs(output);

            File.Move(temporaryPath, fullPath, overwrite);
        }
        finally
        {
            File.Delete(temporaryPath);
        }
    }

    private static void WriteCell(IXLCell cell, object value)
    {
        switch (value)
        {
            case null: break;
            case double number: cell.Value = number; break;
            case int id: cell.Value = id; break;
            case bool boolean: cell.Value = boolean; break;
            case Date date: cell.Value = DateTime.ParseExact(date.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture); break;
            default: cell.Value = value.ToString(); break;
        }
    }

    private void WriteContacts(XLWorkbook book)
    {
        var ws = book.AddWorksheet("Contact Lookups");
        var headers = new[] { "contactId", "Name", "Gender", "isPrimary" };
        var adults = Contacts.Where(contact => contact.Attributes?.Role == ContactAttributes_role.Adult).ToArray();

        for (var c = 0; c < headers.Length; c++)
            ws.Cell(1, c + 1).Value = headers[c];

        for (var r = 0; r < adults.Length; r++)
        {
            var contact = adults[r];
            ws.Cell(r + 2, 1).Value = PositiveId(contact.Id);
            WriteCell(ws.Cell(r + 2, 2), SnapshotLinkData.ContactName(contact));
            WriteCell(ws.Cell(r + 2, 3), contact.Attributes?.Gender);
            WriteCell(ws.Cell(r + 2, 4), contact.Attributes?.IsPrimary);
        }

        Style(ws, headers.Length, adults.Length + 1);
        ws.Column(2).Width = 36;
    }

    private static void WriteTable(XLWorkbook book, string name, string[] headers, IEnumerable<string[]> rows)
    {
        var ws = book.AddWorksheet(name);
        for (var c = 0; c < headers.Length; c++) ws.Cell(1, c + 1).Value = headers[c];

        var r = 2;
        foreach (var row in rows)
        {
            for (var c = 0; c < row.Length; c++)
            {
                ws.Cell(r, c + 1).Style.NumberFormat.Format = "@";
                ws.Cell(r, c + 1).Value = row[c];
            }

            r++;
        }

        Style(ws, headers.Length, r - 1);
        if (name == "Read me")
        {
            ws.Column(2).Width = 110;
            ws.Rows(2, r).Height = 48;
        }
    }

    internal static void StyleDropdownColumn(IXLWorksheet ws, int columnNumber, double width)
    {
        var column = ws.Column(columnNumber);
        column.Width = width;
        column.Style.Alignment.WrapText = false;
        column.Style.Alignment.ShrinkToFit = true;

        // Keep the complete selection on one line; headers can still wrap.
        var header = ws.Cell(1, columnNumber);
        header.Style.Alignment.WrapText = true;
        header.Style.Alignment.ShrinkToFit = false;
    }

    private static void Style(IXLWorksheet ws, int columns, int rows)
    {
        ws.Style.Font.FontName = "Calibri";
        ws.Style.Font.FontSize = 11;

        ws.Range(1, 1, 1, columns).Style.Fill.BackgroundColor = XLColor.FromHtml("#17365D");
        ws.Range(1, 1, 1, columns).Style.Font.FontColor = XLColor.White;
        ws.Range(1, 1, 1, columns).Style.Font.Bold = true;

        ws.Range(1, 1, Math.Max(1, rows), columns).Style.Alignment.WrapText = true;
        ws.Range(1, 1, Math.Max(1, rows), columns).Style.Alignment.Vertical = XLAlignmentVerticalValues.Top;
        ws.Columns(1, columns).Width = 24;
        ws.Row(1).Height = 34;

        if (DataSheets.Contains(ws.Name))
        {
            var table = ws.Range(1, 1, Math.Max(2, rows), columns).CreateTable(SnapshotLinkData.TableName(ws.Name));
            table.Theme = XLTableTheme.None;
        }
        else if (rows > 1)
        {
            ws.Range(1, 1, rows, columns).SetAutoFilter();
        }

        ws.SheetView.FreezeRows(1);
        ws.SheetView.FreezeColumns(1);
    }

    public static SnapshotWorkbook Load(string path)
    {
        using var book = new XLWorkbook(path);
        var result = new SnapshotWorkbook();
        var metadata = RequireSheet(book, "Snapshot");
        var settings = new Dictionary<string, string>(StringComparer.Ordinal);

        foreach (var row in metadata.RowsUsed().Skip(1))
        {
            var key = Text(row.Cell(1));
            if (key.Length == 0 && Text(row.Cell(2)).Length == 0) continue;
            if (!settings.TryAdd(key, Text(row.Cell(2)))) throw new FormatException($"Snapshot: duplicate setting '{key}'.");
        }

        var version = settings.GetValueOrDefault("formatVersion");
        if (version != FormatVersion) throw new FormatException($"Snapshot: unsupported or missing formatVersion. Generate a current version {FormatVersion} workbook.");
        if (!int.TryParse(settings.GetValueOrDefault("contactGroupId"), out var groupId)) throw new FormatException("Snapshot: contactGroupId must be an integer.");

        result.ContactGroupId = groupId;
        result.Source = settings.GetValueOrDefault("source") ?? "Manual";
        foreach (var sheet in Collections) result.SetMode(sheet, settings.GetValueOrDefault(Camel(sheet) + "Mode") ?? "Merge");

        var allowedSettings = new[] { "formatVersion", "contactGroupId", "source" }.Concat(Collections.Select(s => Camel(s) + "Mode"));
        if (settings.Keys.Except(allowedSettings).Any()) throw new FormatException("Snapshot: unrecognised setting (check mode spelling).");

        // Reference sheets support editing, but are not part of the import request.
        foreach (var sheet in DataSheets)
        {
            var ws = RequireSheet(book, sheet);
            var columns = Columns(sheet).ToDictionary(c => c.Header, StringComparer.Ordinal);
            var headers = ws.Row(1).CellsUsed().Select(c => (Name: Text(c), Number: c.Address.ColumnNumber)).ToList();
            if (headers.Select(h => h.Name).Distinct().Count() != headers.Count ||
                headers.Any(h => !columns.ContainsKey(h.Name)) || columns.Keys.Except(headers.Select(h => h.Name)).Any())
                throw new FormatException($"{sheet}: headers must match the generated template exactly (columns may be reordered).");

            foreach (var row in ws.RowsUsed().Skip(1))
            {
                if (row.CellsUsed().All(c => !c.HasFormula && string.IsNullOrEmpty(c.GetString()))) continue;
                if (row.CellsUsed().Any(c => !headers.Any(h => h.Number == c.Address.ColumnNumber))) throw new FormatException($"{sheet} row {row.RowNumber()}: data without a column header.");

                var item = Activator.CreateInstance(RowType(sheet))!;
                foreach (var header in headers)
                {
                    var cell = row.Cell(header.Number);
                    try
                    {
                        var value = Parse(cell, columns[header.Name]);
                        if (value != null) columns[header.Name].Set(item, value);
                    }
                    catch (Exception ex) when (ex is FormatException or ArgumentException or OverflowException)
                    {
                        throw new FormatException($"{sheet}!{cell.Address}: {ex.Message}", ex);
                    }
                }

                // Preserve owner-column order even when headers have moved, then drop blank slots.
                if (item.GetType().GetProperty("Ownership")?.GetValue(item) is List<FinancialSnapshotContactReference> owners)
                    owners.RemoveAll(owner => owner == null);

                result.Rows(sheet).Add(item);
            }
        }

        if (book.TryGetWorksheet("Read me", out var readme))
            foreach (var row in readme.RowsUsed().Skip(1))
                if (Text(row.Cell(1)) == "Export note") result.Notes.Add(Text(row.Cell(2)));

        return result;
    }

    private static IXLWorksheet RequireSheet(XLWorkbook book, string name) => book.TryGetWorksheet(name, out var ws) ? ws : throw new FormatException($"Missing required worksheet '{name}'. An empty collection still needs its sheet and headers.");

    private static string Text(IXLCell cell) => CellText(cell).Trim();

    private static string CellText(IXLCell cell)
    {
        if (cell.HasFormula) throw new FormatException("Formulas are not supported; paste values instead.");
        if (cell.DataType == XLDataType.Error) throw new FormatException("Excel error values are not supported.");

        return cell.GetString();
    }

    private static object Parse(IXLCell cell, Column column)
    {
        var text = CellText(cell);
        if (text.Length == 0) return null;

        if (SnapshotControlData.Columns.ContainsKey(column.Header)) return SnapshotControlData.ParseSelection(column.Header, text);
        if (SnapshotLinkData.IsContactColumn(column.Header)) return SnapshotLinkData.ParseContactSelection(text);

        if (column.Type == typeof(string)) return text;

        text = text.Trim();

        if (column.Type == typeof(int))
        {
            if (!int.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
                throw new FormatException("Expected an integer.");

            return value;
        }

        if (column.Type == typeof(double))
        {
            var value = cell.DataType == XLDataType.Number ? cell.GetDouble() : double.Parse(text, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture);
            if (!double.IsFinite(value)) throw new FormatException("Expected a finite number that can be sent as JSON.");

            return value;
        }

        if (column.Type == typeof(bool)) return bool.Parse(text);
        if (column.Type == typeof(Date)) return new Date(cell.DataType == XLDataType.DateTime ? cell.GetDateTime() : DateTime.ParseExact(text, "yyyy-MM-dd", CultureInfo.InvariantCulture));
        if (column.Type.IsEnum && Enum.GetNames(column.Type).Contains(text)) return Enum.Parse(column.Type, text);

        throw new FormatException($"Invalid {column.Header}; expected {string.Join(", ", column.Type.IsEnum ? Enum.GetNames(column.Type) : [column.Type.Name])}.");
    }

    public static int PositiveId(string text) => int.TryParse(text, NumberStyles.None, CultureInfo.InvariantCulture, out var id) && id > 0 ? id : throw new FormatException($"'{text}' is not a positive contact/resource ID.");
}
