using System;
using System.Linq;
using ClosedXML.Excel;
using MyCrmSampleClient.Kiota.Models;

namespace MyCrmSampleClient.FinancialSnapshots;

/// <summary>Offers links to workbook records while preserving explicit references to existing API records.</summary>
public static class SnapshotLinkData
{
    private sealed record Link(string Header, string Sheet, string Target, bool AllowOtherIds = false);

    private static readonly Link[] Links =
    [
        new("address.lid", "Addresses", "lid"),
        new("linkedAsset.lid", "Assets", "lid"),
        new("linkedAsset.id", "Assets", "assetId", AllowOtherIds: true),
        new("employment.lid", "Employments", "lid"),
        new("employment.id", "Employments", "employmentId", AllowOtherIds: true)
    ];

    public static string TableName(string sheet) => "snapshot_" + sheet.ToLowerInvariant();

    private static string RangeName(Link link) => "link_" + link.Header.Replace('.', '_');

    public static bool IsContactColumn(string header) => header is "contactId" or "owner1ContactId" or "owner2ContactId";

    public static string ContactName(Contact contact)
    {
        var attributes = contact.Attributes;
        var name = string.Join(" ", new[] { attributes?.FirstName, attributes?.MiddleName, attributes?.LastName }
            .Where(part => !string.IsNullOrWhiteSpace(part)).Select(part => part.Trim()));

        return name.Length == 0 ? attributes?.PreferredName?.Trim() ?? "" : name;
    }

    public static string DisplayContact(Contact contact)
    {
        var name = ContactName(contact);

        return name.Length == 0 ? contact.Id : contact.Id + SnapshotControlData.Separator + name;
    }

    public static int ParseContactSelection(string text)
    {
        return SnapshotControlData.ParseSelection("contactId", text);
    }

    public static void AddDropdowns(XLWorkbook excel, SnapshotWorkbook snapshot)
    {
        // Table references grow with inserted rows and keep working when columns move.
        foreach (var link in Links)
            excel.DefinedNames.Add(RangeName(link), $"{TableName(link.Sheet)}[{link.Target}]");

        var lists = excel.TryGetWorksheet("Dropdown lists", out var existing) ? existing : excel.AddWorksheet("Dropdown lists");
        var contactColumn = (lists.LastColumnUsed()?.ColumnNumber() ?? 0) + 1;
        lists.Cell(1, contactColumn).Value = "link_contactId";

        var contactRow = 2;
        foreach (var contact in snapshot.Contacts.Where(c => c.Attributes?.Role == ContactAttributes_role.Adult)
            .DistinctBy(c => c.Id).OrderBy(c => SnapshotWorkbook.PositiveId(c.Id)))
            lists.Cell(contactRow++, contactColumn).Value = DisplayContact(contact);

        excel.DefinedNames.Add("link_contactId", lists.Range(2, contactColumn, Math.Max(2, contactRow - 1), contactColumn));
        lists.Visibility = XLWorksheetVisibility.VeryHidden;

        foreach (var sheet in SnapshotWorkbook.DataSheets)
        {
            var ws = excel.Worksheet(sheet);
            var columns = SnapshotWorkbook.Columns(sheet);

            for (var c = 0; c < columns.Count; c++)
            {
                var header = columns[c].Header;
                var link = Links.SingleOrDefault(item => item.Header == header);
                if (link == null && !IsContactColumn(header)) continue;

                var allowOtherIds = link?.AllowOtherIds == true;
                var validation = ws.Range(2, c + 1, snapshot.LastInputRow(sheet), c + 1).CreateDataValidation();
                validation.List(link == null ? "link_contactId" : RangeName(link), true);
                validation.IgnoreBlanks = true;
                validation.ShowErrorMessage = true;
                validation.ErrorStyle = allowOtherIds ? XLErrorStyle.Warning : XLErrorStyle.Stop;
                validation.ErrorTitle = allowOtherIds ? "Confirm existing record ID" : "Choose a listed reference";
                validation.ErrorMessage = allowOtherIds
                    ? "This ID is not in the workbook. Continue only if it identifies an existing record in this contact group."
                    : "Choose an available reference. Add local IDs to the corresponding sheet before linking to them.";

                validation.ShowInputMessage = true;
                validation.InputTitle = "Record link";
                validation.InputMessage = link == null
                    ? "Choose an Adult contact. Each owner must be distinct; a single owner needs only one column. Names are listed on Contact Lookups."
                    : $"Choose from {link.Sheet}.{link.Target}. Supply either .id or .lid, not both.";

                SnapshotWorkbook.StyleDropdownColumn(ws, c + 1, link == null ? 44 : 32);
            }
        }
    }
}
