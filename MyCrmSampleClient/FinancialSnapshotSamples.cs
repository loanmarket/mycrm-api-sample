using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Http.HttpClientLibrary.Middleware.Options;
using MyCrmSampleClient.Console;
using MyCrmSampleClient.FinancialSnapshots;
using MyCrmSampleClient.Kiota.Models;
using MyCrmSampleClient.KiotaExtensions;
using Serilog;
using Spectre.Console;

namespace MyCrmSampleClient;

public partial class Samples
{
    [Sample("financial-snapshots", "Import, export, generate or compare a financial snapshot spreadsheet", IncludeInRunAll = false)]
    public async Task RunFinancialSnapshotsSample()
    {
        while (true)
        {
            var action = MenuPrompt.Show(new SelectionPrompt<string>().Title("Financial snapshots [grey](Esc: back)[/]")
                .AddChoices("Import spreadsheet", "Export current financials", "Generate fabricated spreadsheet", "Compare spreadsheet with current financials", "Back"));
            if (action == "Back") return;

            try
            {
                var api = new SnapshotApi(_console.Client, JsonApiFluentContext.BaseUrl);
                if (action == "Import spreadsheet") await ImportFinancialSnapshot();
                else if (action == "Export current financials") await ExportFinancialSnapshot(api);
                else if (action == "Compare spreadsheet with current financials") await CompareFinancialSnapshot(api);
                else await GenerateFinancialSnapshot(api);
            }
            catch (MenuBackException)
            {
                // Leave the unfinished action and return to the financial snapshots menu.
                continue;
            }
            catch (ErrorDocument error)
            {
                Log.Warning("Financial snapshot request returned HTTP {StatusCode}", error.ResponseStatusCode);

                foreach (var detail in error.Errors ?? [])
                {
                    var message = detail.Detail ?? detail.Title ?? "The API returned an error without a description.";
                    var code = string.IsNullOrWhiteSpace(detail.Code) ? "" : $"{detail.Code}: ";
                    var pointer = string.IsNullOrWhiteSpace(detail.Source?.Pointer) ? "" : $" ({detail.Source.Pointer})";

                    Log.Warning("{CodePrefix}{Detail}{PointerSuffix}", code, message, pointer);
                }

                AnsiConsole.WriteLine(FinancialRequestAdvice(error.ResponseStatusCode));
            }
            catch (ApiException error)
            {
                // Authentication challenges may have an empty body rather than JSON:API errors.
                // Kiota then throws ApiException, but still preserves the HTTP status.
                AnsiConsole.WriteLine($"Request failed (HTTP {error.ResponseStatusCode}). {FinancialRequestAdvice(error.ResponseStatusCode)}");
            }
            catch (Exception error) when (error is FormatException or IOException or InvalidOperationException or ArgumentException)
            {
                AnsiConsole.WriteLine(error.Message);
            }
            catch (Exception error) when (error is HttpRequestException or OperationCanceledException)
            {
                AnsiConsole.WriteLine($"Request failed: {error.Message}. If submitting a snapshot, read back the current profile before retrying; the write may have succeeded.");
            }
        }
    }

    internal static string FinancialRequestAdvice(int statusCode) => statusCode switch
    {
        401 => "Authentication was rejected. This request did not apply a financial snapshot. Check the configured credentials and API environment.",
        403 => "Access was denied. Check the configured scopes, permitted adviser UserId and group access.",
        423 => "Beta access must be enabled for the resolved adviser before retrying.",
        409 => "Another update conflicted with this snapshot. Read the current group profile before retrying.",
        429 => "The API rate limit is still being exceeded. Wait before trying again. Imports retry 429 responses up to eight times with progressive backoff.",
        0 or >= 500 => "If submitting a snapshot, the write outcome may be uncertain. Export/read the current profile before deciding whether to retry.",
        _ => "Correct the reported fields or references before submitting again."
    };

    private int PromptFinancialGroup(int? preferredGroupId = null)
    {
        var prompt = new TextPrompt<int>("Contact group ID:").Validate(id => id > 0 ? ValidationResult.Success() : ValidationResult.Error("Use a positive ID."));
        var defaultGroupId = preferredGroupId is > 0 ? preferredGroupId : _console.State.LastContactGroupId;
        if (defaultGroupId is > 0) prompt.DefaultValue(defaultGroupId.Value);

        return MenuPrompt.Show(prompt);
    }

    private bool FinancialScopes(params string[] scopes) => scopes.All(scope => _console.RequireScope(scope, "financial-snapshots"));

    private async Task ImportFinancialSnapshot()
    {
        if (!FinancialScopes("api.contactgroups.financialsnapshot") || !_console.RequireAdviserContactId("financial-snapshots")) return;

        var path = MenuPrompt.Ask<string>("Spreadsheet path (.xlsx):").Trim().Trim('"');
        var book = SnapshotWorkbook.Load(path);
        AnsiConsole.WriteLine($"Workbook nominates contact group {book.ContactGroupId} ({book.Source}).");
        foreach (var note in book.Notes) AnsiConsole.WriteLine(note);

        var groupId = PromptFinancialGroup(book.ContactGroupId);

        var mode = MenuPrompt.Show(new SelectionPrompt<string>().Title("Collection modes").AddChoices("Use workbook modes", "Merge all collections", "Replace all collections"));
        if (mode != "Use workbook modes")
        {
            foreach (var collection in SnapshotWorkbook.Collections)
                book.SetMode(collection, mode.StartsWith("Merge", StringComparison.Ordinal) ? "Merge" : "Replace");
        }

        var table = _console.CreateTable("Collection", "Rows", "Mode");
        foreach (var collection in SnapshotWorkbook.Collections) table.AddRow(collection, book.Rows(collection).Count.ToString(), book.Mode(collection));
        _console.WriteTable(table);

        AnsiConsole.WriteLine("Matched rows replace optional fields and ownership. Owners are split equally. Standard expenses may consolidate even in Merge mode.");
        if (SnapshotWorkbook.Collections.Any(c => book.Mode(c) == "Replace"))
            AnsiConsole.WriteLine("Replace deactivates omitted records across the whole group, including empty collections. Removing assets can also remove linked income, liabilities and deal securities.");

        var document = book.ToDocument();
        if (MenuPrompt.Confirm("Display the complete request before submitting?", false))
            AnsiConsole.WriteLine(await KiotaJsonSerializer.SerializeAsStringAsync(document));

        if (!MenuPrompt.Confirm($"Apply this financial snapshot to contact group {groupId}?", false)) return;

        var backoff = new RateLimitBackoff(retry => AnsiConsole.WriteLine(
            $"HTTP 429: {retry.Request}. Waiting {retry.Delay.TotalSeconds:F1}s before retry {retry.Retry}/{retry.MaximumRetries}."));
        var url = new Uri(new Uri(JsonApiFluentContext.BaseUrl.TrimEnd('/') + "/"), $"jsonapi/contact-groups/{groupId}/financial-snapshot").AbsoluteUri;

        // Disable Kiota's general retries: only a confirmed 429 may replay this POST.
        // A timeout or server failure can follow a committed write.
        var response = await backoff.ExecuteAsync(() => _console.Client.Jsonapi.ContactGroups[groupId].FinancialSnapshot.PostAsync(document,
            request => request.Options.Add(new RetryHandlerOption { MaxRetry = 0 })), "POST " + url);

        var operations = response?.Data?.Attributes?.Operations ?? throw new InvalidOperationException("No operation result was returned. Read back the profile before retrying.");
        var results = _console.CreateTable("Operation", "Resource", "ID", "Local IDs");

        void Add(string action, IEnumerable<FinancialSnapshotOperation> rows)
        {
            foreach (var row in rows ?? []) results.AddRow(_console.Cell(action), _console.Cell(row.Type), _console.Cell(row.Id), _console.Cell(string.Join(", ", row.Meta?.Lids ?? [])));
        }
        Add("Created", operations.Created);
        Add("Updated", operations.Updated);
        Add("Deactivated", operations.Deleted);

        _console.WriteTable(results);
        AnsiConsole.WriteLine("Export the current financials to retain the saved IDs for your next deliberate update.");
    }

    private async Task ExportFinancialSnapshot(SnapshotApi api)
    {
        if (!FinancialScopes("api.contact-groups.read", "api.contacts.read", "api.assets.search", "api.incomes.search", "api.expenses.search", "api.liabilities.search")) return;
        if (!FinancialScopes(SnapshotControlData.RequiredScopes)) return;

        var groupId = PromptFinancialGroup();
        var book = await api.ExportAsync(groupId);
        book.Lookups.AddRange(await api.LookupsAsync());

        foreach (var note in book.Notes) AnsiConsole.WriteLine(note);
        SaveFinancialWorkbook(book, $"financials-{groupId}.xlsx");
    }

    private async Task GenerateFinancialSnapshot(SnapshotApi api)
    {
        if (!FinancialScopes("api.contact-groups.read") || !FinancialScopes(SnapshotControlData.RequiredScopes)) return;

        var groupId = PromptFinancialGroup();
        var contacts = await api.ContactsAsync(groupId);
        var country = await api.CountryAsync(groupId);
        var lookups = await api.LookupsAsync();

        var seed = MenuPrompt.Show(new TextPrompt<int>("Random seed (reuse to repeat an example):").DefaultValue(Random.Shared.Next(1, int.MaxValue)));
        var book = SnapshotExamples.Create(groupId, contacts, lookups, country, seed);
        var defaultCount = SnapshotWorkbook.Collections.Sum(sheet => book.Rows(sheet).Count);
        var itemCount = MenuPrompt.Show(new TextPrompt<int>("Number of financial items (excludes addresses and lookups):").DefaultValue(defaultCount)
            .Validate(count => count is >= 1 and <= SnapshotExamples.MaximumItemCount
                ? ValidationResult.Success() : ValidationResult.Error($"Enter 1–{SnapshotExamples.MaximumItemCount} items.")));
        if (itemCount != defaultCount) book = SnapshotExamples.Create(groupId, contacts, lookups, country, seed, itemCount);

        AnsiConsole.WriteLine($"Generated {SnapshotWorkbook.Collections.Sum(sheet => book.Rows(sheet).Count)} financial items and {book.Attributes.Addresses.Count} supporting addresses.");
        SaveFinancialWorkbook(book, $"financials-example-{groupId}.xlsx");
    }

    private async Task CompareFinancialSnapshot(SnapshotApi api)
    {
        if (!FinancialScopes("api.contact-groups.read", "api.contacts.read", "api.assets.search", "api.incomes.search", "api.expenses.search", "api.liabilities.search")) return;

        var path = MenuPrompt.Ask<string>("Spreadsheet path (.xlsx):").Trim().Trim('"');
        var spreadsheet = SnapshotWorkbook.Load(path, allowMissingColumns: true);
        AnsiConsole.WriteLine($"Workbook nominates contact group {spreadsheet.ContactGroupId} ({spreadsheet.Source}).");

        var groupId = PromptFinancialGroup(spreadsheet.ContactGroupId);
        AnsiConsole.WriteLine($"Comparing {Path.GetFullPath(path)} with current API data for contact group {groupId}.");
        if (groupId != spreadsheet.ContactGroupId)
            AnsiConsole.WriteLine($"The selected group differs from the workbook's group {spreadsheet.ContactGroupId}.");

        var current = await api.ExportAsync(groupId);
        var country = current.Country;
        var comparison = SnapshotComparison.Compare(spreadsheet, current, country);

        AnsiConsole.WriteLine("Comparison is read-only. Collection modes and reference sheets are not compared; this is not an import preview.");
        AnsiConsole.WriteLine($"Only columns present in the spreadsheet are compared. The group's adviser country is {country}.");
        if (country == "AU") AnsiConsole.WriteLine("NZ-only fields (nz columns) are excluded for Australian groups.");
        if (country == "NZ") AnsiConsole.WriteLine("Liability loanTerm is excluded for New Zealand groups; nzDocumentedLoanTermMonths is compared instead.");
        AnsiConsole.WriteLine("Records match by explicit ID or unique identifying fields. Addresses are compared through their asset or employment. Owner order and local-ID spelling are ignored.");
        AnsiConsole.WriteLine("Only spreadsheet/API means no counterpart was matched. Review means the match or address could not be resolved; use explicit record IDs to disambiguate.");
        AnsiConsole.WriteLine("API reads are not a transactional snapshot. Managed repayment expenses and unsupported incomes are excluded as on export.");

        var summary = _console.CreateTable("Collection", "Equal", "Changed", "Only spreadsheet", "Only API", "Review");
        foreach (var sheet in SnapshotWorkbook.DataSheets)
        {
            var rows = comparison.Rows.Where(row => row.Collection == sheet).ToArray();
            if (sheet == "Addresses" && rows.Length == 0) continue;

            string Count(SnapshotComparisonStatus status) => rows.Count(row => row.Status == status).ToString();
            summary.AddRow(sheet, Count(SnapshotComparisonStatus.Equal), Count(SnapshotComparisonStatus.Changed),
                Count(SnapshotComparisonStatus.SpreadsheetOnly), Count(SnapshotComparisonStatus.ApiOnly), Count(SnapshotComparisonStatus.Ambiguous));
        }

        _console.WriteTable(summary);

        foreach (var row in comparison.Rows.Where(row => row.Status != SnapshotComparisonStatus.Equal))
        {
            AnsiConsole.WriteLine($"{row.Collection}: {row.Status}");
            if (row.SpreadsheetRecord.Length > 0) AnsiConsole.WriteLine("  Spreadsheet: " + row.SpreadsheetRecord);
            if (row.ApiRecord.Length > 0) AnsiConsole.WriteLine("  API: " + row.ApiRecord);
            if (row.Differences.Count == 0) continue;

            var details = _console.CreateTable("Field", "Spreadsheet", "API");
            foreach (var difference in row.Differences)
                details.AddRow(_console.Cell(difference.Field), _console.Cell(difference.Spreadsheet), _console.Cell(difference.Api));

            _console.WriteTable(details);
        }

        if (!comparison.HasDifferences) AnsiConsole.WriteLine("No differences in the compared snapshot fields.");
        foreach (var note in current.Notes) AnsiConsole.WriteLine(note);
    }

    private static void SaveFinancialWorkbook(SnapshotWorkbook book, string defaultName)
    {
        var path = MenuPrompt.Show(new TextPrompt<string>("Output spreadsheet path:").DefaultValue(defaultName)).Trim().Trim('"');
        if (!string.Equals(Path.GetExtension(path), ".xlsx", StringComparison.OrdinalIgnoreCase)) throw new FormatException("Use an .xlsx filename.");

        path = Path.GetFullPath(path);
        var overwrite = File.Exists(path);
        if (overwrite && !MenuPrompt.Confirm($"{Markup.Escape(path)} already exists. Overwrite it?", true))
        {
            AnsiConsole.WriteLine("Save cancelled. Existing file kept.");
            return;
        }

        book.Save(path, overwrite);
        AnsiConsole.WriteLine($"Saved {path}");
    }
}
