using System.Collections.Generic;
using System.Threading.Tasks;
using MyCrmSampleClient.Console;
using MyCrmSampleClient.Kiota.Models;
using MyCrmSampleClient.KiotaExtensions;
using Serilog;
using Spectre.Console;

namespace MyCrmSampleClient;

public partial class Samples
{
    [Sample("patch-deal-name", "PATCH /jsonapi/deals/{id}")]
    public async Task RunPatchDealNameSample()
    {
        if (!_console.RequireAdviserContactId("patch-deal-name")) return;
        if (!_console.RequireScope("api.deals.update", "patch-deal-name")) return;
        if (!_console.State.TryGetDealId("patch-deal-name", out var dealId)) return;

        var name = AnsiConsole.Ask<string>($"New name for deal {dealId}:");
        await _console.Client.Jsonapi.Deals[dealId].PatchAsync(new DealPatchDocument
        {
            Data = new DealPatch
            {
                Type = "deals",
                Id = dealId.ToString(),
                Attributes = new DealAttributes { Name = name }
            }
        });

        Log.Information("Updated name for deal {DealId}. Omitted attributes were left unchanged", dealId);
    }

    [Sample("patch-deal-rationale", "Read the deal's adviser country, then PATCH /jsonapi/deals/{id}")]
    public async Task RunPatchDealRationaleSample()
    {
        if (!_console.RequireAdviserContactId("patch-deal-rationale")) return;
        if (!_console.RequireScope("api.deals.read", "patch-deal-rationale")) return;
        if (!_console.RequireScope("api.deals.update", "patch-deal-rationale")) return;
        if (!_console.State.TryGetDealId("patch-deal-rationale", out var dealId)) return;

        AnsiConsole.WriteLine("The deal must be in pre-submission to edit its BID notes.");
        var adviser = await _console.Client.Jsonapi.Deals[dealId].Adviser.GetAsync();
        var countryCode = adviser?.Data?.Attributes?.CountryCode;
        var attributes = new DealAttributes();

        switch (countryCode)
        {
            case "AU":
                attributes.AuRationale = new AuDealRationale
                {
                    Summary = AnsiConsole.Ask<string>($"AU BID summary for deal {dealId}:")
                };
                break;
            case "NZ":
                attributes.NzRationale = new NzDealRationale
                {
                    Summary = AnsiConsole.Ask<string>($"NZ BID summary for deal {dealId}:"),
                    LenderPreferences = AnsiConsole.Ask<string>("Lender preferences:"),
                    LoanStructure = AnsiConsole.Ask<string>("Loan structure rationale:"),
                    ProductFeatures = AnsiConsole.Ask<string>("Product features rationale:")
                };
                break;
            default:
                Log.Warning("No AU or NZ countryCode returned for deal {DealId}'s adviser", dealId);
                return;
        }

        await _console.Client.Jsonapi.Deals[dealId].PatchAsync(new DealPatchDocument
        {
            Data = new DealPatch
            {
                Type = "deals",
                Id = dealId.ToString(),
                Attributes = attributes
            }
        });

        Log.Information(
            "Updated BID notes for deal {DealId}. These fields are write-only and are not returned in reads", dealId);
    }

    [Sample("custom-statuses", "GET /jsonapi/custom-statuses?sort=sortOrder")]
    public async Task RunCustomStatusesSample()
    {
        if (!_console.RequireScope("api.custom-statuses.search", "custom-statuses")) return;

        var response =
            await _console.Client.Jsonapi.CustomStatuses.GetAsync(request => request.QueryParameters.Sort =
                ["sortOrder"]);

        WriteCustomStatuses(response?.Data);
    }

    [Sample("deal-custom-status", "GET /jsonapi/deals/{id}/customStatus")]
    public async Task RunDealCustomStatusSample()
    {
        if (!_console.RequireScope("api.deals.read", "deal-custom-status")) return;
        if (!_console.State.TryGetDealId("deal-custom-status", out var dealId)) return;

        var response = await _console.Client.Jsonapi.Deals[dealId].CustomStatus.GetAsync();
        WriteCustomStatuses(response?.Data is { } status ? [status] : []);
    }

    [Sample("organisation-custom-statuses", "GET /jsonapi/organisations/{id}/customStatuses?sort=sortOrder")]
    public async Task RunOrganisationCustomStatusesSample()
    {
        if (!_console.RequireScope("api.organisations.read", "organisation-custom-statuses")) return;

        var organisationId = AnsiConsole.Ask<int>("Organisation ID:");
        var response = await _console.Client.Jsonapi.Organisations[organisationId].CustomStatuses
            .WithUrl(
                $"{JsonApiFluentContext.BaseUrl}/jsonapi/organisations/{organisationId}/customStatuses?sort=sortOrder")
            .GetAsync();

        WriteCustomStatuses(response?.Data);
    }

    [Sample("change-deal-status", "PUT /jsonapi/deals/{id}/status")]
    public async Task RunChangeDealStatusSample()
    {
        if (!_console.RequireAdviserContactId("change-deal-status")) return;
        if (!_console.RequireScope("api.deals.update", "change-deal-status")) return;
        if (!_console.State.TryGetDealId("change-deal-status", out var dealId)) return;

        AnsiConsole.WriteLine(
            "Use a custom status ID from the deal's organisation. The API checks whether the movement is allowed.");
        var customStatusId = AnsiConsole.Ask<int>($"Target custom status ID for deal {dealId}:");

        try
        {
            // This endpoint uses plain JSON; Kiota sets application/json from the specification.
            var response = await _console.Client.Jsonapi.Deals[dealId].Status.PutAsync(new DealStatusChangeRequest
            {
                CustomStatusId = customStatusId
            });

            var table = _console.CreateTable("Deal Id", "Custom Status Id", "Status Name");
            table.AddRow(
                _console.Cell(response?.Id?.ToString()),
                _console.Cell(response?.CustomStatusId?.ToString()),
                _console.Cell(response?.StatusName));
            _console.WriteTable(table);
        }
        catch (ErrorDocument error)
        {
            Log.Warning("Status change returned HTTP {StatusCode}", error.ResponseStatusCode);
            foreach (var detail in error.Errors ?? [])
            {
                Log.Warning("{Code}: {Detail}", detail.Code, detail.Detail);
            }
        }
    }

    private void WriteCustomStatuses(IEnumerable<CustomStatus> statuses)
    {
        var table = _console.CreateTable("Custom Status Id", "Name", "Sort Order");
        foreach (var status in statuses ?? [])
        {
            table.AddRow(
                _console.Cell(status.Id),
                _console.Cell(status.Attributes?.Name),
                _console.Cell(status.Attributes?.SortOrder?.ToString()));
        }

        _console.WriteTable(table);
    }
}