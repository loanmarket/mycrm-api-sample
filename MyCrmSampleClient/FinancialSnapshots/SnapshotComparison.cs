using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
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
    private readonly Dictionary<object, object> _matches = new();
    private readonly List<SnapshotRowComparison> _rows = [];

    public IReadOnlyList<SnapshotRowComparison> Rows => _rows;
    public bool HasDifferences => _rows.Any(row => row.Status != SnapshotComparisonStatus.Equal);

    private SnapshotComparison(SnapshotWorkbook spreadsheet, SnapshotWorkbook api)
    {
        _spreadsheet = spreadsheet;
        _api = api;
    }

    public static SnapshotComparison Compare(SnapshotWorkbook spreadsheet, SnapshotWorkbook api)
    {
        var comparison = new SnapshotComparison(spreadsheet, api);

        // Resolve assets and employments before comparing their dependants' local-ID links.
        foreach (var sheet in new[] { "Assets", "Employments", "Liabilities", "Incomes", "Expenses" })
            comparison.MatchCollection(sheet);

        comparison.ReportUnusedAddresses();

        return comparison;
    }

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
                foreach (var context in sources.GroupBy(row => Context(sheet, row, _spreadsheet)).ToArray())
                {
                    var candidates = targets.Where(row => Context(sheet, row, _api) == context.Key).ToArray();
                    if (context.Count() != 1 || candidates.Length != 1) continue;

                    var source = context.Single();
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
            if (!Equals(EmptyToNull(expected), EmptyToNull(actual)))
                differences.Add(new(field, Display(expected), Display(actual)));
        }

        foreach (var column in SnapshotWorkbook.Columns(sheet).Where(column => column.Header != "lid"
            && column.Header != IdHeader(sheet) && column.Child == null && !column.OwnerIndex.HasValue))
            Compare(column.Header, column.Get(source), column.Get(target));

        if (source.GetType().GetProperty("Ownership") != null)
            Compare("ownership", Owners(source), Owners(target));

        foreach (var (field, relatedSheet) in new[] { ("LinkedAsset", "Assets"), ("Employment", "Employments") })
        {
            if (source.GetType().GetProperty(field) == null) continue;

            var expected = Reference(source, field, relatedSheet, _spreadsheet);
            var actual = Reference(target, field, relatedSheet, _api);
            if (expected.Unresolved || actual.Unresolved || expected.Id != actual.Id)
                differences.Add(new(char.ToLowerInvariant(field[0]) + field[1..], expected.Display, actual.Display));
        }

        if (source.GetType().GetProperty("Address") != null)
        {
            var expected = Address(source, _spreadsheet);
            var actual = Address(target, _api);
            if (expected.Unresolved || actual.Unresolved)
                differences.Add(new("address", expected.Display, actual.Display));
            else if ((expected.Row == null) != (actual.Row == null))
                differences.Add(new("address", expected.Display, actual.Display));
            else if (expected.Row != null)
                foreach (var column in SnapshotWorkbook.Columns("Addresses").Where(column => column.Header != "lid"))
                    Compare("address." + column.Header, column.Get(expected.Row), column.Get(actual.Row));
        }

        return differences;
    }

    private string Identity(string sheet, object row, SnapshotWorkbook book)
    {
        object Get(string name) => row.GetType().GetProperty(name)?.GetValue(row);
        string Key(params object[] parts) => JsonSerializer.Serialize(parts.Select(part => Normalise(Display(part))));

        if (sheet == "Employments")
        {
            if (Get("ContactId") == null) return null;
            var type = Get("EmploymentType")?.ToString();
            if (type is "Unemployed" or "Retired" or "HomeDuties" or "Student")
                return Key(Get("ContactId"), type, Get("DateStarted"));

            return string.IsNullOrWhiteSpace(Get("EmployerName") as string) ? null
                : Key(Get("ContactId"), Get("EmployerName"), Get("DateStarted"));
        }

        var typeId = Get(SnapshotWorkbook.RowType(sheet).Name["FinancialSnapshot".Length..] + "TypeId");
        if (typeId == null) return null;

        if (sheet == "Assets")
        {
            if ((int)typeId == 1)
            {
                var address = Address(row, book);
                if (address.Unresolved || address.Row == null) return null;
                var a = address.Row;

                return !string.IsNullOrWhiteSpace(a.StreetAddress) && !string.IsNullOrWhiteSpace(a.Suburb) && !string.IsNullOrWhiteSpace(a.PostCode)
                    ? Key(typeId, a.StreetAddress, a.Suburb, a.PostCode)
                    : string.IsNullOrWhiteSpace(a.FormattedAddress) ? null : Key(typeId, a.FormattedAddress);
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

    private string Context(string sheet, object row, SnapshotWorkbook book) => JsonSerializer.Serialize(new[]
    {
        Owners(row), Reference(row, "LinkedAsset", "Assets", book).Display,
        Reference(row, "Employment", "Employments", book).Display,
        sheet == "Expenses" ? Normalise(row.GetType().GetProperty("Description")?.GetValue(row) as string) : null,
        sheet == "Employments" ? row.GetType().GetProperty("EmploymentStatus")?.GetValue(row)?.ToString() : null
    });

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

    private static string Owners(object row) => string.Join(", ",
        ((IEnumerable<FinancialSnapshotContactReference>)row.GetType().GetProperty("Ownership")?.GetValue(row) ?? [])
        .Select(owner => owner?.Id).OrderBy(id => id).Select(id => id?.ToString(CultureInfo.InvariantCulture) ?? "(empty)"));

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

    private static string Display(object value) => EmptyToNull(value) switch
    {
        null => "(empty)",
        IFormattable number => number.ToString(null, CultureInfo.InvariantCulture),
        _ => value.ToString()
    };

    private static string Normalise(string text) => text == null ? "" : string.Join(" ", text.Split((char[])null, StringSplitOptions.RemoveEmptyEntries)).ToUpperInvariant();

    private static string Account(object value) => (value as string)?.Replace(" ", "").Replace("-", "");
}
