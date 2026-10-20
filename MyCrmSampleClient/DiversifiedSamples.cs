using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MyCrmSampleClient.Console;
using MyCrmSampleClient.Kiota.Models;
using MyCrmSampleClient.KiotaExtensions;
using Serilog;
using Spectre.Console;

namespace MyCrmSampleClient;

public partial class Samples
{

    [Sample("latest-asset-deals", "GET /jsonapi/asset-deal?sort=-createdDate&page[number]=1&page[size]=10")]
    public Task RunLatestAssetDealSample() => ReadAssetDealPage(false);

    [Sample("filtered-asset-deals", "GET /jsonapi/asset-deal with status Settled")]
    public Task RunFilteredAssetDealSample() => ReadAssetDealPage(true);

    [Sample("paged-asset-deals", "GET /jsonapi/asset-deal?sort=-createdDate&page[number]=2&page[size]=3")]
    public Task RunPagedAssetDealSample() => ReadAssetDealPage(false, 2, 3, "paged-asset-deals");

    private async Task ReadAssetDealPage(bool filtered, int pageNumber = 1, int pageSize = 10,
        string sampleName = null)
    {
        var sample = sampleName ?? (filtered ? "filtered-asset-deals" : "latest-asset-deals");
        if (!_console.RequireScope("api.diversifiedassetdeals.search", sample)) return;
        var endpoint = _console.Client.Jsonapi.AssetDeal;
        var query = new JsonApiSearchBuilder<AssetDealsDocument>(
            () => endpoint.ToGetRequestInformation(),
            (url, ct) => endpoint.WithUrl(url).GetAsync(cancellationToken: ct))
            .Page(pageNumber, pageSize).Sort("createdDate", SortOrder.Descending);
        if (filtered)
        {
            query.Filter("equals(status,'Settled')");
        }
        var response = await query.GetAsync();
        if (response?.Data == null || response.Data.Count == 0)
        {
            Log.Information("No asset-deal records returned on this page. Saved ID is unchanged.");
            return;
        }
        var table = new Table().AddColumn("Type").AddColumn("ID").AddColumn("Title")
            .AddColumn("Status").AddColumn("Amount").AddColumn("Created");
        foreach (var item in response.Data)
            table.AddRow(EscapeDiversified(item.Type), EscapeDiversified(item.Id),
                EscapeDiversified(item.Attributes?.Title), EscapeDiversified(item.Attributes?.Status),
                EscapeDiversified(item.Attributes?.Amount), EscapeDiversified(item.Attributes?.CreatedDate));
        AnsiConsole.Write(table);
        if (int.TryParse(response.Data[0].Id, out var id) && id > 0)
            _console.UpdateState(_console.State with { LastAssetDealId = id });
    }

    [Sample("get-asset-deal", "GET /jsonapi/asset-deal/{id}")]
    public Task RunGetAssetDealSample() => ReadAssetDealMember("get");

    [Sample("asset-deal-includes", "GET /jsonapi/asset-deal/{id}?include=adviser,applicants,applicantBusinesses")]
    public Task RunAssetDealIncludesSample() => ReadAssetDealMember("includes");

    [Sample("asset-deal-fields", "GET /jsonapi/asset-deal/{id} with sparse summary and related identity fields")]
    public Task RunAssetDealFieldsSample() => ReadAssetDealMember("fields");

    private async Task ReadAssetDealMember(string mode)
    {
        var sample = mode == "get" ? "get-asset-deal" : "asset-deal-" + mode;
        if (!_console.RequireScope("api.diversifiedassetdeals.read", sample)) return;
        var id = GetDiversifiedId(_console.State.LastAssetDealId, "asset-deal");
        var endpoint = _console.Client.Jsonapi.AssetDeal[id];
        var query = new JsonApiSearchBuilder<AssetDealDocument>(
            () => endpoint.ToGetRequestInformation(),
            (url, ct) => endpoint.WithUrl(url).GetAsync(cancellationToken: ct));
        if (mode != "get") query.Include("adviser", "applicants", "applicantBusinesses");
        if (mode == "fields")
            query.Fields("asset-deal", "title", "status", "amount", "createdDate",
                    "adviser", "applicants", "applicantBusinesses")
                .Fields("advisers", "id").Fields("contacts", "id").Fields("business", "id");
        var response = await query.GetAsync();
        var item = response?.Data;
        if (item == null)
        {
            Log.Warning("No asset-deal resource returned. Saved ID is unchanged.");
            return;
        }
        if (int.TryParse(item.Id, out var returnedId) && returnedId > 0)
            _console.UpdateState(_console.State with { LastAssetDealId = returnedId });
        WriteDiversifiedSummary(item.Type, item.Id,
            ("title", item.Attributes?.Title), ("status", item.Attributes?.Status),
            ("amount", item.Attributes?.Amount), ("createdDate", item.Attributes?.CreatedDate));
        if (mode != "fields")
            WriteDiversifiedSummary(item.Type, item.Id,
                ("statusLabel", item.Attributes?.StatusLabel), ("loanReference", item.Attributes?.LoanReference),
                ("lenderName", item.Attributes?.LenderName),
                ("submittedDate", item.Attributes?.SubmittedDate),
                ("decisionDate", item.Attributes?.DecisionDate),
                ("settlementDate", item.Attributes?.SettlementDate));
        if (mode != "get")
            WriteDiversifiedRelationships(item.Relationships?.Adviser?.Data,
                item.Relationships?.Applicants?.Data, item.Relationships?.ApplicantBusinesses?.Data,
                response.Included);
    }

    [Sample("latest-commercial-deals", "GET /jsonapi/commercial-deal?sort=-createdDate&page[number]=1&page[size]=10")]
    public Task RunLatestCommercialDealSample() => ReadCommercialDealPage(false);

    [Sample("filtered-commercial-deals", "GET /jsonapi/commercial-deal with status NewApplication")]
    public Task RunFilteredCommercialDealSample() => ReadCommercialDealPage(true);

    [Sample("paged-commercial-deals", "GET /jsonapi/commercial-deal?sort=-createdDate&page[number]=2&page[size]=3")]
    public Task RunPagedCommercialDealSample() => ReadCommercialDealPage(false, 2, 3, "paged-commercial-deals");

    private async Task ReadCommercialDealPage(bool filtered, int pageNumber = 1, int pageSize = 10,
        string sampleName = null)
    {
        var sample = sampleName ?? (filtered ? "filtered-commercial-deals" : "latest-commercial-deals");
        if (!_console.RequireScope("api.diversifiedcommercialdeals.search", sample)) return;
        var endpoint = _console.Client.Jsonapi.CommercialDeal;
        var query = new JsonApiSearchBuilder<CommercialDealsDocument>(
            () => endpoint.ToGetRequestInformation(),
            (url, ct) => endpoint.WithUrl(url).GetAsync(cancellationToken: ct))
            .Page(pageNumber, pageSize).Sort("createdDate", SortOrder.Descending);
        if (filtered)
        {
            query.Filter("equals(status,'NewApplication')");
        }
        var response = await query.GetAsync();
        if (response?.Data == null || response.Data.Count == 0)
        {
            Log.Information("No commercial-deal records returned on this page. Saved ID is unchanged.");
            return;
        }
        var table = new Table().AddColumn("Type").AddColumn("ID").AddColumn("Title")
            .AddColumn("Status").AddColumn("Amount").AddColumn("Created");
        foreach (var item in response.Data)
            table.AddRow(EscapeDiversified(item.Type), EscapeDiversified(item.Id),
                EscapeDiversified(item.Attributes?.Title), EscapeDiversified(item.Attributes?.Status),
                EscapeDiversified(item.Attributes?.Amount), EscapeDiversified(item.Attributes?.CreatedDate));
        AnsiConsole.Write(table);
        if (int.TryParse(response.Data[0].Id, out var id) && id > 0)
            _console.UpdateState(_console.State with { LastCommercialDealId = id });
    }

    [Sample("get-commercial-deal", "GET /jsonapi/commercial-deal/{id}")]
    public Task RunGetCommercialDealSample() => ReadCommercialDealMember("get");

    [Sample("commercial-deal-includes", "GET /jsonapi/commercial-deal/{id}?include=adviser,applicants,applicantBusinesses")]
    public Task RunCommercialDealIncludesSample() => ReadCommercialDealMember("includes");

    [Sample("commercial-deal-fields", "GET /jsonapi/commercial-deal/{id} with sparse summary and related identity fields")]
    public Task RunCommercialDealFieldsSample() => ReadCommercialDealMember("fields");

    private async Task ReadCommercialDealMember(string mode)
    {
        var sample = mode == "get" ? "get-commercial-deal" : "commercial-deal-" + mode;
        if (!_console.RequireScope("api.diversifiedcommercialdeals.read", sample)) return;
        var id = GetDiversifiedId(_console.State.LastCommercialDealId, "commercial-deal");
        var endpoint = _console.Client.Jsonapi.CommercialDeal[id];
        var query = new JsonApiSearchBuilder<CommercialDealDocument>(
            () => endpoint.ToGetRequestInformation(),
            (url, ct) => endpoint.WithUrl(url).GetAsync(cancellationToken: ct));
        if (mode != "get") query.Include("adviser", "applicants", "applicantBusinesses");
        if (mode == "fields")
            query.Fields("commercial-deal", "title", "status", "amount", "createdDate",
                    "adviser", "applicants", "applicantBusinesses")
                .Fields("advisers", "id").Fields("contacts", "id").Fields("business", "id");
        var response = await query.GetAsync();
        var item = response?.Data;
        if (item == null)
        {
            Log.Warning("No commercial-deal resource returned. Saved ID is unchanged.");
            return;
        }
        if (int.TryParse(item.Id, out var returnedId) && returnedId > 0)
            _console.UpdateState(_console.State with { LastCommercialDealId = returnedId });
        WriteDiversifiedSummary(item.Type, item.Id,
            ("title", item.Attributes?.Title), ("status", item.Attributes?.Status),
            ("amount", item.Attributes?.Amount), ("createdDate", item.Attributes?.CreatedDate));
        if (mode != "fields")
            WriteDiversifiedSummary(item.Type, item.Id,
                ("statusLabel", item.Attributes?.StatusLabel), ("loanReference", item.Attributes?.LoanReference),
                ("lenderName", item.Attributes?.LenderName),
                ("submittedDate", item.Attributes?.SubmittedDate),
                ("decisionDate", item.Attributes?.DecisionDate),
                ("settlementDate", item.Attributes?.SettlementDate));
        if (mode != "get")
            WriteDiversifiedRelationships(item.Relationships?.Adviser?.Data,
                item.Relationships?.Applicants?.Data, item.Relationships?.ApplicantBusinesses?.Data,
                response.Included);
    }

    [Sample("latest-asset-referrals", "GET /jsonapi/asset-referral?sort=-createdDate&page[number]=1&page[size]=10")]
    public Task RunLatestAssetReferralSample() => ReadAssetReferralPage(false);

    [Sample("filtered-asset-referrals", "GET /jsonapi/asset-referral created within the last 365 days (UTC)")]
    public Task RunFilteredAssetReferralSample() => ReadAssetReferralPage(true);

    private async Task ReadAssetReferralPage(bool filtered)
    {
        var sample = filtered ? "filtered-asset-referrals" : "latest-asset-referrals";
        if (!_console.RequireScope("api.diversifiedassetreferrals.search", sample)) return;
        var endpoint = _console.Client.Jsonapi.AssetReferral;
        var query = new JsonApiSearchBuilder<AssetReferralsDocument>(
            () => endpoint.ToGetRequestInformation(),
            (url, ct) => endpoint.WithUrl(url).GetAsync(cancellationToken: ct))
            .Page(1, 10).Sort("createdDate", SortOrder.Descending);
        if (filtered)
        {
            var from = DateTime.UtcNow.AddDays(-365).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            // Include recent records even when their source status is missing.
            query.Filter($"greaterOrEqual(createdDate,'{from}')");
        }
        var response = await query.GetAsync();
        if (response?.Data == null || response.Data.Count == 0)
        {
            Log.Information("No asset-referral records returned on this page. Saved ID is unchanged.");
            return;
        }
        var table = new Table().AddColumn("Type").AddColumn("ID").AddColumn("Title")
            .AddColumn("Status").AddColumn("Amount").AddColumn("Created");
        foreach (var item in response.Data)
            table.AddRow(EscapeDiversified(item.Type), EscapeDiversified(item.Id),
                EscapeDiversified(item.Attributes?.Title), EscapeDiversified(item.Attributes?.Status),
                EscapeDiversified(item.Attributes?.Amount), EscapeDiversified(item.Attributes?.CreatedDate));
        AnsiConsole.Write(table);
        if (int.TryParse(response.Data[0].Id, out var id) && id > 0)
            _console.UpdateState(_console.State with { LastAssetReferralId = id });
    }

    [Sample("get-asset-referral", "GET /jsonapi/asset-referral/{id}")]
    public Task RunGetAssetReferralSample() => ReadAssetReferralMember("get");

    [Sample("asset-referral-includes", "GET /jsonapi/asset-referral/{id}?include=adviser,applicants,applicantBusinesses")]
    public Task RunAssetReferralIncludesSample() => ReadAssetReferralMember("includes");

    [Sample("asset-referral-fields", "GET /jsonapi/asset-referral/{id} with sparse summary and related identity fields")]
    public Task RunAssetReferralFieldsSample() => ReadAssetReferralMember("fields");

    private async Task ReadAssetReferralMember(string mode)
    {
        var sample = mode == "get" ? "get-asset-referral" : "asset-referral-" + mode;
        if (!_console.RequireScope("api.diversifiedassetreferrals.read", sample)) return;
        var id = GetDiversifiedId(_console.State.LastAssetReferralId, "asset-referral");
        var endpoint = _console.Client.Jsonapi.AssetReferral[id];
        var query = new JsonApiSearchBuilder<AssetReferralDocument>(
            () => endpoint.ToGetRequestInformation(),
            (url, ct) => endpoint.WithUrl(url).GetAsync(cancellationToken: ct));
        if (mode != "get") query.Include("adviser", "applicants", "applicantBusinesses");
        if (mode == "fields")
            query.Fields("asset-referral", "title", "status", "amount", "createdDate",
                    "adviser", "applicants", "applicantBusinesses")
                .Fields("advisers", "id").Fields("contacts", "id").Fields("business", "id");
        var response = await query.GetAsync();
        var item = response?.Data;
        if (item == null)
        {
            Log.Warning("No asset-referral resource returned. Saved ID is unchanged.");
            return;
        }
        if (int.TryParse(item.Id, out var returnedId) && returnedId > 0)
            _console.UpdateState(_console.State with { LastAssetReferralId = returnedId });
        WriteDiversifiedSummary(item.Type, item.Id,
            ("title", item.Attributes?.Title), ("status", item.Attributes?.Status),
            ("amount", item.Attributes?.Amount), ("createdDate", item.Attributes?.CreatedDate));
        if (mode != "fields")
            WriteDiversifiedSummary(item.Type, item.Id,
                ("statusLabel", item.Attributes?.StatusLabel), ("loanReference", item.Attributes?.LoanReference),
                ("lenderName", item.Attributes?.LenderName),
                ("referralSentDate", item.Attributes?.ReferralSentDate),
                ("approvalDate", item.Attributes?.ApprovalDate),
                ("settlementDate", item.Attributes?.SettlementDate));
        if (mode != "get")
            WriteDiversifiedRelationships(item.Relationships?.Adviser?.Data,
                item.Relationships?.Applicants?.Data, item.Relationships?.ApplicantBusinesses?.Data,
                response.Included);
    }

    private static int GetDiversifiedId(int? savedId, string resource)
    {
        if (savedId is > 0) return savedId.Value;
        return AnsiConsole.Prompt(new TextPrompt<int>($"Enter an accessible {resource} Engagement V1 ID:")
            .Validate(id => id > 0 ? ValidationResult.Success() : ValidationResult.Error("Use a positive ID.")));
    }

    private static string EscapeDiversified(object value) =>
        Markup.Escape(Convert.ToString(value, CultureInfo.InvariantCulture) ?? "(not returned)");

    private static void WriteDiversifiedSummary(string type, string id, params (string Name, object Value)[] fields)
    {
        var table = new Table().Title(EscapeDiversified(type + "/" + id)).AddColumn("Field").AddColumn("Value");
        foreach (var field in fields) table.AddRow(field.Name, EscapeDiversified(field.Value));
        AnsiConsole.Write(table);
    }

    private static void WriteDiversifiedRelationships(ResourceIdentifier adviser,
        IEnumerable<ResourceIdentifier> applicants, IEnumerable<ResourceIdentifier> businesses,
        IEnumerable<IncludedResource> included)
    {
        var identities = (included ?? Enumerable.Empty<IncludedResource>())
            .Select(item => (item.Type, item.Id)).ToHashSet();
        var table = new Table().AddColumn("Relationship").AddColumn("Type").AddColumn("ID")
            .AddColumn("Included");
        void Add(string name, IEnumerable<ResourceIdentifier> links)
        {
            var items = links?.ToArray() ?? Array.Empty<ResourceIdentifier>();
            if (items.Length == 0) table.AddRow(name, "-", "-", "No related records returned");
            foreach (var link in items)
                table.AddRow(name, EscapeDiversified(link.Type), EscapeDiversified(link.Id),
                    identities.Contains((link.Type, link.Id)) ? "Yes" : "No");
        }
        // Match by BOTH type and ID: contacts/11 and business/11 are different parties.
        Add("adviser", adviser == null ? null : new[] { adviser });
        Add("applicants", applicants);
        Add("applicantBusinesses", businesses);
        AnsiConsole.Write(table);
    }
}
