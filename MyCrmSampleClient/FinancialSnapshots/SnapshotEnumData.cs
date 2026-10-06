using System;
using System.Collections.Generic;
using ClosedXML.Excel;

namespace MyCrmSampleClient.FinancialSnapshots;

/// <summary>Builds enum dropdowns from the generated snapshot input models.</summary>
public static class SnapshotEnumData
{
    public static void AddDropdowns(XLWorkbook excel, SnapshotWorkbook snapshot)
    {
        var lists = excel.TryGetWorksheet("Dropdown lists", out var existing) ? existing : excel.AddWorksheet("Dropdown lists");
        var listColumn = lists.LastColumnUsed()?.ColumnNumber() ?? 0;
        var addedTypes = new HashSet<Type>();

        foreach (var sheet in SnapshotWorkbook.DataSheets)
        {
            var ws = excel.Worksheet(sheet);
            var columns = SnapshotWorkbook.Columns(sheet);
            var lastRow = snapshot.LastInputRow(sheet);

            for (var c = 0; c < columns.Count; c++)
            {
                var type = columns[c].Type;
                if (!type.IsEnum) continue;

                var name = "enum_" + type.Name;
                if (addedTypes.Add(type))
                {
                    listColumn++;
                    lists.Cell(1, listColumn).Value = name;

                    var values = Enum.GetNames(type);
                    for (var r = 0; r < values.Length; r++)
                        lists.Cell(r + 2, listColumn).Value = values[r];

                    // Worksheet ranges also support enums whose combined labels exceed
                    // Excel's 255-character limit for inline validation lists.
                    excel.DefinedNames.Add(name, lists.Range(2, listColumn, values.Length + 1, listColumn));
                }

                var validation = ws.Range(2, c + 1, lastRow, c + 1).CreateDataValidation();
                validation.List(name, true);
                validation.IgnoreBlanks = true;
                validation.ShowErrorMessage = true;
                validation.ErrorStyle = XLErrorStyle.Stop;
                validation.ErrorTitle = "Choose a listed value";
                validation.ErrorMessage = "Select a value from this column's dropdown.";

                validation.ShowInputMessage = true;
                validation.InputTitle = "Allowed values";
                validation.InputMessage = "Choose the exact enum value from the list. Optional fields may be left blank.";

                SnapshotWorkbook.StyleDropdownColumn(ws, c + 1, 32);
            }
        }

        lists.Visibility = XLWorksheetVisibility.VeryHidden;
    }
}
