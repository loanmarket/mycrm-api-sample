using System;
using System.Collections.Generic;
using System.Linq;
using MyCrmSampleClient.Kiota.Models;

namespace MyCrmSampleClient.FinancialSnapshots;

public static partial class SnapshotExamples
{
    private static SnapshotWorkbook CreateSized(int groupId, IEnumerable<Contact> contacts,
        IReadOnlyCollection<SnapshotLookup> lookups, string country, int seed, int itemCount)
    {
        var people = contacts.ToArray();
        var random = new Random(seed);
        var result = new SnapshotWorkbook
        {
            ContactGroupId = groupId, Country = country,
            Source = $"Fabricated example (seed {seed}, {itemCount} financial items)"
        };
        SnapshotApi.AddContacts(result, people);
        result.Lookups.AddRange(lookups);

        var added = 0;
        var batch = 0;
        while (added < itemCount)
        {
            // Reuse the established country/type rules, then select rows with their dependencies.
            // Each pool contains standalone savings, so even one remaining slot can be filled.
            var pool = new Generator(groupId, people, lookups, country, random.Next()).Create();
            MakeDistinct(pool, ++batch);
            var candidates = SnapshotWorkbook.Collections
                .SelectMany(sheet => pool.Rows(sheet).Cast<object>().Select(row => (Sheet: sheet, Row: row)))
                .OrderBy(_ => random.Next()).ToArray();

            foreach (var candidate in candidates)
            {
                if (result.Rows(candidate.Sheet).Contains(candidate.Row)) continue;

                // Ordinary expenses consolidate by type on import. Keep them distinct so
                // the generated item count does not depend on server-side aggregation.
                if (candidate.Row is FinancialSnapshotExpense expense
                    && result.Attributes.Expenses.Any(existing => existing.ExpenseTypeId == expense.ExpenseTypeId)) continue;

                var rows = Dependencies(pool, candidate.Sheet, candidate.Row)
                    .Where(item => !result.Rows(item.Sheet).Contains(item.Row)).Distinct().ToArray();
                var financialCount = rows.Count(item => item.Sheet != "Addresses");
                if (added + financialCount > itemCount) continue;

                foreach (var (sheet, row) in rows) result.Rows(sheet).Add(row);
                added += financialCount;
                if (added == itemCount) break;
            }
        }

        foreach (var jobs in result.Attributes.Employments.GroupBy(job => job.ContactId))
            foreach (var job in jobs.Skip(1)) job.EmploymentStatus = FinancialSnapshotEmployment_employmentStatus.SecondaryEmployment;

        result.Notes.Add($"Generated with Bogus using seed {seed} and {itemCount} financial items. Reuse the count and seed with the same contacts, controls and generator version to repeat the example.");
        result.Notes.Add("The count includes assets, liabilities, incomes, expenses and employments. Supporting addresses and lookup rows are additional. Small examples may not include every collection.");
        result.Notes.Add("All balances, employers, accounts and addresses are fabricated. Real contact IDs and lookup IDs are used. Review before importing into a test group.");

        return result;
    }

    private static IEnumerable<(string Sheet, object Row)> Dependencies(SnapshotWorkbook pool, string sheet, object row)
    {
        foreach (var column in SnapshotWorkbook.Columns(sheet).Where(column => column.Child?.Name == "Lid"))
        {
            if (column.Get(row) is not string lid) continue;

            var targetSheet = column.Property.Name switch
            {
                "Address" => "Addresses", "Employment" => "Employments", "LinkedAsset" => "Assets",
                _ => throw new InvalidOperationException("Unknown snapshot relationship.")
            };
            var target = pool.Rows(targetSheet).Cast<object>().Single(item => (string)item.GetType().GetProperty("Lid")!.GetValue(item) == lid);

            foreach (var dependency in Dependencies(pool, targetSheet, target)) yield return dependency;
        }

        yield return (sheet, row);
    }

    private static void MakeDistinct(SnapshotWorkbook pool, int batch)
    {
        foreach (var sheet in SnapshotWorkbook.DataSheets)
        {
            foreach (var row in pool.Rows(sheet))
            {
                foreach (var column in SnapshotWorkbook.Columns(sheet).Where(column => column.Header == "lid" || column.Child?.Name == "Lid"))
                    if (column.Get(row) is string lid) column.Set(row, $"example-{batch}-{lid}");

                var description = row.GetType().GetProperty("Description");
                if (sheet != "Expenses" && description?.GetValue(row) is string text)
                    description.SetValue(row, $"{text} sample {batch}");
            }
        }

        foreach (var job in pool.Attributes.Employments) job.EmployerName += $" Sample {batch}";
        foreach (var address in pool.Attributes.Addresses)
        {
            address.StreetAddress = $"Unit {batch}, {address.StreetAddress}";
            address.FormattedAddress = $"{address.StreetAddress}, {address.Suburb}, {address.State} {address.PostCode}, {address.Country}";
        }
    }
}
