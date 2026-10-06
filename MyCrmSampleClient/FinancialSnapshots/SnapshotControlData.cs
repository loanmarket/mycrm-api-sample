using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ClosedXML.Excel;

namespace MyCrmSampleClient.FinancialSnapshots;

public sealed record SnapshotLookup(string Resource, int Id, string Name, string ParentResource = null, int? ParentId = null);

/// <summary>Maps editable snapshot ID columns to the API's reference data.</summary>
public static class SnapshotControlData
{
    public const string Separator = " — ";

    // These repayment expense types are represented through liabilities on export.
    internal static readonly HashSet<int> RepaymentExpenseTypes = [1, 8, 31, 33, 34, 35, 42];

    public static readonly IReadOnlyDictionary<string, string> Columns = new Dictionary<string, string>
    {
        ["assetTypeId"] = "asset-types",
        ["assetSubTypeId"] = "asset-sub-types",
        ["expenseTypeId"] = "expense-types",
        ["incomeTypeId"] = "income-types",
        ["liabilityTypeId"] = "liability-types",
        ["liabilitySubTypeId"] = "liability-sub-types",
        ["propertyTypeId"] = "property-types",
        ["incomeVerificationId"] = "income-verification-types",
        ["nzRentalVerificationTypeId"] = "rental-verification-types"
    };

    public static readonly string[] Resources = ["asset-categories", "liability-categories", "income-categories", "expense-categories", .. Columns.Values];
    public static readonly string[] RequiredScopes = Resources.Where(r => r != "liability-sub-types").Select(r => "api." + r + ".search").ToArray();

    public static string Display(SnapshotLookup item, IReadOnlyCollection<SnapshotLookup> controls)
    {
        var parent = controls.FirstOrDefault(c => c.Resource == item.ParentResource && c.Id == item.ParentId);

        return item.Id.ToString(CultureInfo.InvariantCulture) + Separator + (parent == null ? "" : parent.Name + " / ") + item.Name;
    }

    public static bool IsSelectable(SnapshotLookup item, IReadOnlyCollection<SnapshotLookup> controls)
    {
        if (item.Resource == "expense-types" && RepaymentExpenseTypes.Contains(item.Id)) return false;
        if (item.Resource != "income-types") return true;

        var category = controls.FirstOrDefault(c => c.Resource == item.ParentResource && c.Id == item.ParentId)?.Name ?? "";
        var name = new string(item.Name.Where(char.IsLetterOrDigit).ToArray()).ToUpperInvariant();

        return !category.Contains("Addback", StringComparison.OrdinalIgnoreCase) &&
            name is not ("ADDBACKS" or "NETPROFITBEFORETAX" or "DEPRECIATION" or "NONCASHBENEFITS" or "NONRECURRINGEXPENSES");
    }

    public static int ParseSelection(string header, string text)
    {
        // Labels are display hints; the numeric prefix is the submitted value.
        var separator = text.IndexOf(Separator, StringComparison.Ordinal);
        var idText = separator < 0 ? text : text[..separator];
        if (int.TryParse(idText, NumberStyles.Integer, CultureInfo.InvariantCulture, out var id)) return id;

        throw new FormatException($"Select {header} from the control-data dropdown, or enter an integer ID for the snapshot service to validate.");
    }

    public static string RangeName(string resource) => "control_" + resource.Replace('-', '_');

    public static void AddDropdowns(XLWorkbook excel, SnapshotWorkbook snapshot)
    {
        if (snapshot.Lookups.Count == 0) return;

        var lists = excel.AddWorksheet("Dropdown lists");
        var listColumn = 0;

        void AddList(string name, IEnumerable<SnapshotLookup> items)
        {
            listColumn++;
            lists.Cell(1, listColumn).Value = name;

            var row = 2;
            foreach (var item in items.OrderBy(c => c.Name, StringComparer.OrdinalIgnoreCase).ThenBy(c => c.Id))
                lists.Cell(row++, listColumn).Value = Display(item, snapshot.Lookups);

            // An empty list references one blank cell, so unsupported combinations offer no choices.
            excel.DefinedNames.Add(name, lists.Range(2, listColumn, Math.Max(2, row - 1), listColumn));
        }

        foreach (var resource in Columns.Values)
            AddList(RangeName(resource), snapshot.Lookups.Where(c => c.Resource == resource && IsSelectable(c, snapshot.Lookups)));

        AddList(RangeName("liability-sub-types") + "_", []);
        foreach (var type in snapshot.Lookups.Where(c => c.Resource == "liability-types"))
            AddList(RangeName("liability-sub-types") + "_" + type.Id, snapshot.Lookups.Where(c => c.Resource == "liability-sub-types" && c.ParentResource == "liability-types" && c.ParentId == type.Id));

        foreach (var sheet in SnapshotWorkbook.DataSheets)
        {
            var ws = excel.Worksheet(sheet);
            var columns = SnapshotWorkbook.Columns(sheet);

            for (var c = 0; c < columns.Count; c++)
            {
                if (!Columns.TryGetValue(columns[c].Header, out var resource)) continue;

                var formula = RangeName(resource);
                if (columns[c].Header == "liabilitySubTypeId")
                {
                    var parentIndex = columns.Select((column, index) => (column, index)).Single(x => x.column.Header == "liabilityTypeId").index + 1;
                    var parent = "$" + XLHelper.GetColumnLetterFromNumber(parentIndex) + "2";
                    formula = $"INDIRECT(\"{formula}_\"&IFERROR(LEFT({parent},FIND(\"{Separator}\",{parent})-1),{parent}))";
                }

                var validation = ws.Range(2, c + 1, snapshot.LastInputRow(sheet), c + 1).CreateDataValidation();
                validation.List(formula, true);
                validation.IgnoreBlanks = true;
                validation.ShowErrorMessage = true;
                validation.ErrorStyle = XLErrorStyle.Warning;
                validation.ErrorTitle = "Confirm control ID";
                validation.ErrorMessage = "Choose a listed value, or continue to enter a numeric ID. The snapshot service validates IDs and type/subtype combinations.";

                validation.ShowInputMessage = true;
                validation.InputTitle = "Control data";
                validation.InputMessage = "Choose an ID and description, or enter a numeric ID. The snapshot service validates the submitted value.";

                SnapshotWorkbook.StyleDropdownColumn(ws, c + 1, 44);
            }
        }

        lists.Visibility = XLWorksheetVisibility.VeryHidden;
    }
}
