using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using Microsoft.Kiota.Abstractions;
using MyCrmSampleClient.Kiota.Models;

namespace MyCrmSampleClient.FinancialSnapshots;

public enum SnapshotComparisonStatus { Equal, Changed, SpreadsheetOnly, ApiOnly, Ambiguous }

public sealed record SnapshotFieldDifference(string Field, string Spreadsheet, string Api);

public sealed record SnapshotRowComparison(string Collection, string SpreadsheetRecord, string ApiRecord,
    SnapshotComparisonStatus Status, IReadOnlyList<SnapshotFieldDifference> Differences);

/// <summary>Compares readable snapshot fields without changing either input or predicting an import.</summary>
public sealed class SnapshotComparison
{
    private readonly SnapshotWorkbook _spreadsheet;
    private readonly SnapshotWorkbook _api;
    private readonly string _country;
    private readonly Dictionary<object, object> _matches = new();
    private readonly List<SnapshotRowComparison> _rows = [];

    public IReadOnlyList<SnapshotRowComparison> Rows => _rows;
    public bool HasDifferences => _rows.Any(row => row.Status != SnapshotComparisonStatus.Equal);

    private SnapshotComparison(SnapshotWorkbook spreadsheet, SnapshotWorkbook api, string country)
    {
        _spreadsheet = spreadsheet;
        _api = api;
        _country = country;
    }

    public static SnapshotComparison Compare(SnapshotWorkbook spreadsheet, SnapshotWorkbook api, string country = null)
    {
        var comparison = new SnapshotComparison(spreadsheet, api, country);

        // Resolve assets and employments before comparing their dependants' local-ID links.
        foreach (var sheet in new[] { "Assets", "Employments", "Liabilities", "Incomes", "Expenses" })
            comparison.MatchCollection(sheet);

        comparison.ReportUnusedAddresses();

        return comparison;
    }

    private bool Includes(string sheet, string header) => _spreadsheet.HasColumn(sheet, header)
        && !(_country == "AU" && header.StartsWith("nz", StringComparison.Ordinal))
        && !(_country == "NZ" && sheet == "Liabilities" && header == "loanTerm");

    private void MatchCollection(string sheet)
    {
        var left = _spreadsheet.Rows(sheet).Cast<object>().ToList();
        var right = _api.Rows(sheet).Cast<object>().ToList();
        var pairs = new List<(object Left, object Right)>();

        void Match(object source, object target)
        {
            _matches.Add(source, target);
            pairs.Add((source, target));
            left.Remove(source);
            right.Remove(target);
        }

        // Reserve explicit IDs first. Never fall back to another record when an ID is missing.
        foreach (var group in left.Where(row => Id(sheet, row).HasValue).GroupBy(row => Id(sheet, row)).ToArray())
        {
            var targets = right.Where(row => Id(sheet, row) == group.Key).ToArray();
            if (group.Count() == 1 && targets.Length == 1)
                Match(group.Single(), targets[0]);
            else if (group.Count() > 1 || targets.Length > 1)
            {
                foreach (var row in group) Unmatched(sheet, row, targets, SnapshotComparisonStatus.Ambiguous);
                foreach (var target in targets) right.Remove(target);
            }
            else Unmatched(sheet, group.Single(), [], SnapshotComparisonStatus.SpreadsheetOnly);

            foreach (var row in group) left.Remove(row);
        }

        var targetsByKey = right.Where(row => Identity(sheet, row, _api) != null)
            .ToLookup(row => Identity(sheet, row, _api));

        foreach (var group in left.GroupBy(row => Identity(sheet, row, _spreadsheet)))
        {
            var sources = group.ToList();
            var targets = group.Key == null ? [] : targetsByKey[group.Key].ToList();

            // Prefer a unique owner/link context when several records share identifying fields.
            // Resolve only one-to-one choices; never choose the first of several candidates.
            if (sources.Count > 1 || targets.Count > 1)
            {
                foreach (var source in sources.ToArray())
                {
                    var candidates = targets.Where(target => ContextMatches(sheet, source, target)).ToArray();
                    if (candidates.Length != 1 || sources.Count(row => ContextMatches(sheet, row, candidates[0])) != 1) continue;

                    Match(source, candidates[0]);
                    sources.Remove(source);
                    targets.Remove(candidates[0]);
                }
            }

            if (sources.Count == 1 && targets.Count == 1)
            {
                Match(sources[0], targets[0]);
                sources.Clear();
                targets.Clear();
            }

            foreach (var source in sources)
                Unmatched(sheet, source, targets, targets.Count == 0 ? SnapshotComparisonStatus.SpreadsheetOnly : SnapshotComparisonStatus.Ambiguous);

            // Ambiguous candidates belong to the unresolved group, not to an API-only claim.
            if (sources.Count > 0)
                foreach (var target in targets) right.Remove(target);
        }

        foreach (var (source, target) in pairs)
        {
            var differences = CompareFields(sheet, source, target);
            _rows.Add(new(sheet, Label(sheet, source, _spreadsheet), Label(sheet, target, _api),
                differences.Count == 0 ? SnapshotComparisonStatus.Equal : SnapshotComparisonStatus.Changed, differences));
        }

        foreach (var target in right)
            _rows.Add(new(sheet, "", Label(sheet, target, _api), SnapshotComparisonStatus.ApiOnly, []));
    }

    private void Unmatched(string sheet, object row, IEnumerable<object> candidates, SnapshotComparisonStatus status) =>
        _rows.Add(new(sheet, Label(sheet, row, _spreadsheet), string.Join("; ", candidates.Select(candidate => Label(sheet, candidate, _api))), status, []));

    private List<SnapshotFieldDifference> CompareFields(string sheet, object source, object target)
    {
        var differences = new List<SnapshotFieldDifference>();

        void Compare(string field, object expected, object actual)
        {
            if (!Equals(ComparisonValue(expected), ComparisonValue(actual)))
                differences.Add(new(field, Display(expected), Display(actual)));
        }

        foreach (var column in SnapshotWorkbook.Columns(sheet).Where(column => column.Header != "lid"
            && column.Header != IdHeader(sheet) && column.Child == null && !column.OwnerIndex.HasValue && Includes(sheet, column.Header)))
            Compare(column.Header, column.Get(source), column.Get(target));

        if (!OwnersMatch(sheet, source, target))
            differences.Add(new("ownership", Owners(source), Owners(target)));

        foreach (var (field, relatedSheet) in new[] { ("LinkedAsset", "Assets"), ("Employment", "Employments") })
        {
            if (!IncludesReference(sheet, source, field)) continue;

            var expected = Reference(source, field, relatedSheet, _spreadsheet);
            var actual = Reference(target, field, relatedSheet, _api);
            if (expected.Unresolved || actual.Unresolved || expected.Id != actual.Id)
                differences.Add(new(char.ToLowerInvariant(field[0]) + field[1..], expected.Display, actual.Display));
        }

        if (source.GetType().GetProperty("Address") != null && Includes(sheet, "address.lid"))
        {
            var expected = Address(source, _spreadsheet);
            var actual = Address(target, _api);
            if (expected.Unresolved || actual.Unresolved)
                differences.Add(new("address", expected.Display, actual.Display));
            else if ((expected.Row == null) != (actual.Row == null))
                differences.Add(new("address", expected.Display, actual.Display));
            else if (expected.Row != null)
                foreach (var column in SnapshotWorkbook.Columns("Addresses").Where(column => column.Header != "lid" && Includes("Addresses", column.Header)))
                    Compare("address." + column.Header, column.Get(expected.Row), column.Get(actual.Row));
        }

        return differences;
    }

    private string Identity(string sheet, object row, SnapshotWorkbook book)
    {
        object Get(string name) => Includes(sheet, char.ToLowerInvariant(name[0]) + name[1..])
            ? row.GetType().GetProperty(name)?.GetValue(row) : null;
        string Key(params object[] parts) => JsonSerializer.Serialize(parts.Select(part => Normalise(Display(part))));

        if (sheet == "Employments")
        {
            if (Get("ContactId") == null) return null;
            var type = Get("EmploymentType")?.ToString();
            if (type is "Unemployed" or "Retired" or "HomeDuties" or "Student")
                return Key(Get("ContactId"), type, Get("DateStarted"));

            return Includes(sheet, "employerName") && string.IsNullOrWhiteSpace(Get("EmployerName") as string) ? null
                : Key(Get("ContactId"), Get("EmployerName"), Get("DateStarted"));
        }

        var typeId = Get(SnapshotWorkbook.RowType(sheet).Name["FinancialSnapshot".Length..] + "TypeId");
        if (typeId == null) return null;

        if (sheet == "Assets")
        {
            if ((int)typeId == 1)
            {
                if (!Includes(sheet, "address.lid")) return Key(typeId);

                var address = Address(row, book);
                if (address.Unresolved || address.Row == null) return null;
                var a = address.Row;
                var street = Includes("Addresses", "streetAddress") ? a.StreetAddress : null;
                var suburb = Includes("Addresses", "suburb") ? a.Suburb : null;
                var postCode = Includes("Addresses", "postCode") ? a.PostCode : null;
                var structured = new[] { ("streetAddress", street), ("suburb", suburb), ("postCode", postCode) }
                    .Where(part => Includes("Addresses", part.Item1)).ToArray();

                if (structured.Length > 0 && structured.All(part => !string.IsNullOrWhiteSpace(part.Item2)))
                    return Key(typeId, street, suburb, postCode);

                return !Includes("Addresses", "formattedAddress") || string.IsNullOrWhiteSpace(a.FormattedAddress)
                    ? null : Key(typeId, a.FormattedAddress);
            }

            if ((int)typeId is 5 or 7 or 15 or 20 or 21 or 24 or 28 or 31)
                return Key(typeId, Get("Institution"), Account(Get("Bsb")), Account(Get("AccountNumber")));
            if ((int)typeId == 18)
                return Key(typeId, Get("AssetSubTypeId"), Get("VehicleYear"), Get("VehicleMake"), Get("Description"));
        }

        if (sheet == "Liabilities")
            return Key(typeId, Get("CreditorName"), Account(Get("AccountNumber")), Get("LiabilitySubTypeId"));
        if (sheet == "Expenses") return Key(typeId);

        return Key(typeId, Get("Description"));
    }

    private bool ContextMatches(string sheet, object source, object target)
    {
        if (!OwnersMatch(sheet, source, target)) return false;

        foreach (var (field, relatedSheet) in new[] { ("LinkedAsset", "Assets"), ("Employment", "Employments") })
            if (IncludesReference(sheet, source, field)
                && Reference(source, field, relatedSheet, _spreadsheet).Display != Reference(target, field, relatedSheet, _api).Display)
                return false;

        if (sheet == "Expenses" && Includes(sheet, "description"))
            return Normalise(((FinancialSnapshotExpense)source).Description) == Normalise(((FinancialSnapshotExpense)target).Description);
        if (sheet == "Employments" && Includes(sheet, "employmentStatus"))
            return ((FinancialSnapshotEmployment)source).EmploymentStatus == ((FinancialSnapshotEmployment)target).EmploymentStatus;

        return true;
    }

    private bool IncludesReference(string sheet, object source, string property)
    {
        var field = source.GetType().GetProperty(property);
        if (field == null) return false;

        var header = char.ToLowerInvariant(property[0]) + property[1..];
        var hasId = Includes(sheet, header + ".id");
        var hasLid = Includes(sheet, header + ".lid");

        // id and lid are alternative ways to express the same link. A missing alternative
        // leaves a blank reference unspecified, but a supplied reference can still be resolved.
        return (hasId && hasLid) || ((hasId || hasLid) && field.GetValue(source) != null);
    }

    private bool OwnersMatch(string sheet, object source, object target)
    {
        if (source.GetType().GetProperty("Ownership") == null) return true;

        var missing = new[] { "owner1ContactId", "owner2ContactId" }.Count(header => !Includes(sheet, header));
        if (missing == 2) return true;
        if (missing == 0) return Owners(source) == Owners(target);

        // Owner order has no meaning. Each omitted owner column permits one unspecified owner.
        var remaining = OwnerIds(target).ToList();
        foreach (var owner in OwnerIds(source))
            if (!remaining.Remove(owner)) return false;

        return remaining.Count <= missing;
    }

    private (int? Id, string Display, bool Unresolved) Reference(object row, string property, string sheet, SnapshotWorkbook book)
    {
        var reference = row.GetType().GetProperty(property)?.GetValue(row);
        if (reference == null) return (null, "(empty)", false);

        var id = (int?)reference.GetType().GetProperty("Id")?.GetValue(reference);
        var lid = (string)reference.GetType().GetProperty("Lid")?.GetValue(reference);
        if (id.HasValue && lid == null) return (id, "id=" + id.Value.ToString(CultureInfo.InvariantCulture), false);

        if (!id.HasValue && lid != null)
        {
            var targets = book.Rows(sheet).Cast<object>().Where(target => Lid(target) == lid).ToArray();
            if (targets.Length == 1)
            {
                var target = targets[0];
                if (book == _spreadsheet && _matches.TryGetValue(target, out var match)) target = match;
                if (Id(sheet, target) is int resolvedId)
                    return (resolvedId, "id=" + resolvedId.ToString(CultureInfo.InvariantCulture), false);
            }
        }

        return (null, $"Unresolved reference (id={id}, lid={lid})", true);
    }

    private static (FinancialSnapshotAddress Row, string Display, bool Unresolved) Address(object row, SnapshotWorkbook book)
    {
        var reference = row.GetType().GetProperty("Address")?.GetValue(row) as FinancialSnapshotAddressReference;
        if (reference == null) return (null, "(empty)", false);

        var addresses = book.Attributes.Addresses.Where(address => reference.Lid != null && address.Lid == reference.Lid).ToArray();

        return addresses.Length == 1 ? (addresses[0], "lid=" + reference.Lid, false)
            : (null, "Unresolved address.lid=" + reference.Lid, true);
    }

    private void ReportUnusedAddresses()
    {
        // An omitted parent-link column may have referenced these addresses; do not call them unused.
        if (!Includes("Assets", "address.lid") || !Includes("Employments", "address.lid")) return;

        foreach (var book in new[] { _spreadsheet, _api })
        {
            var referenced = book.Attributes.Assets.Select(asset => asset.Address?.Lid)
                .Concat(book.Attributes.Employments.Select(job => job.Address?.Lid)).Where(lid => lid != null).ToHashSet();

            foreach (var address in book.Attributes.Addresses.Where(address => address.Lid == null || !referenced.Contains(address.Lid)))
                _rows.Add(new("Addresses", book == _spreadsheet ? Label("Addresses", address, book) : "",
                    book == _api ? Label("Addresses", address, book) : "", SnapshotComparisonStatus.Ambiguous,
                    [new("address", "Unreferenced address; no asset or employment identifies its API counterpart.", "")]));
        }
    }

    private static IEnumerable<int?> OwnerIds(object row) =>
        ((IEnumerable<FinancialSnapshotContactReference>)row.GetType().GetProperty("Ownership")?.GetValue(row) ?? [])
        .Select(owner => owner?.Id);

    private static string Owners(object row) => string.Join(", ",
        OwnerIds(row).OrderBy(id => id).Select(id => id?.ToString(CultureInfo.InvariantCulture) ?? "(empty)"));

    private static string IdHeader(string sheet)
    {
        var name = SnapshotWorkbook.RowType(sheet).Name["FinancialSnapshot".Length..];

        return char.ToLowerInvariant(name[0]) + name[1..] + "Id";
    }

    private static int? Id(string sheet, object row) => (int?)SnapshotWorkbook.Columns(sheet)
        .FirstOrDefault(column => column.Header == IdHeader(sheet))?.Get(row);

    private static string Lid(object row) => row.GetType().GetProperty("Lid")?.GetValue(row) as string;

    private static string Label(string sheet, object row, SnapshotWorkbook book)
    {
        var parts = new List<string>();
        if (Id(sheet, row) is int id) parts.Add(IdHeader(sheet) + "=" + id.ToString(CultureInfo.InvariantCulture));
        if (Lid(row) is { Length: > 0 } lid) parts.Add("lid=" + lid);
        parts.Add("item " + (book.Rows(sheet).IndexOf(row) + 1));

        return string.Join(", ", parts);
    }

    private static object EmptyToNull(object value) => value is string { Length: 0 } ? null : value;

    // Kiota Date can retain a time component internally, but the API and workbook use calendar dates.
    private static object ComparisonValue(object value) => value is Date date ? (DateOnly)date : EmptyToNull(value);

    private static string Display(object value) => EmptyToNull(value) switch
    {
        null => "(empty)",
        IFormattable number => number.ToString(null, CultureInfo.InvariantCulture),
        _ => value.ToString()
    };

    private static string Normalise(string text) => text == null ? "" : string.Join(" ", text.Split((char[])null, StringSplitOptions.RemoveEmptyEntries)).ToUpperInvariant();

    private static string Account(object value) => (value as string)?.Replace(" ", "").Replace("-", "");
}
