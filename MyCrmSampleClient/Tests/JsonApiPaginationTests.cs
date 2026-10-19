using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using MyCrmSampleClient.Kiota;
using MyCrmSampleClient.Kiota.Models;
using MyCrmSampleClient.KiotaExtensions;
using Xunit;

namespace MyCrmSampleClient.Tests;

[Collection("Menu console")]
public sealed class JsonApiPaginationTests : IDisposable
{
    private readonly string _originalBaseUrl = JsonApiFluentContext.BaseUrl;

    public JsonApiPaginationTests() => JsonApiFluentContext.BaseUrl = "https://example.test/v2";

    public void Dispose() => JsonApiFluentContext.BaseUrl = _originalBaseUrl;

    private sealed record Page<T>(IEnumerable<T> Data, TopLevelLinks Links = null, IEnumerable<IncludedResource> Included = null);

    [Fact]
    public async Task FollowsShortAndEmptyPagesDeduplicatesAndCollectsIncludedResourcesInOrder()
    {
        var first = new Contact { Type = "contacts", Id = "1", Attributes = new() { FirstName = "Earlier" } };
        var latest = new Contact { Type = "contacts", Id = "1", Attributes = new() { FirstName = "Latest" } };
        var included = new ContactAddress { Type = "contact-addresses", Id = "1" };
        var pages = new Queue<object>([
            new Page<Contact>([first], new() { Next = "?page[number]=2" }, [included]),
            new Page<Contact>([], new() { Next = "./contacts?cursor=last" }),
            new Page<Contact>([latest, new() { Type = "contacts", Id = "2" }])
        ]);
        var requests = new List<string>();
        var resources = new Dictionary<(string, string), IncludedResource>();

        var result = await JsonApiPagination.ReadPagesAsync<Contact>("https://example.test/v2", "jsonapi/contacts?page[size]=100",
            (url, _) => { requests.Add(url); return Task.FromResult(pages.Dequeue()); },
            resource => resources[(resource.Type, resource.Id)] = resource);

        Assert.Equal(["1", "2"], result.Select(contact => contact.Id));
        Assert.Same(latest, result[0]);
        Assert.Same(latest, resources[("contacts", "1")]);
        Assert.Same(included, resources[("contact-addresses", "1")]);
        Assert.Equal(3, resources.Count);
        Assert.Equal([
            "https://example.test/v2/jsonapi/contacts?page[size]=100",
            "https://example.test/v2/jsonapi/contacts?page[number]=2",
            "https://example.test/v2/jsonapi/contacts?cursor=last"
        ], requests);
    }

    [Theory]
    [InlineData("https://untrusted.test/v2/jsonapi/contacts")]
    [InlineData("//untrusted.test/v2/jsonapi/contacts")]
    [InlineData("http://example.test/v2/jsonapi/contacts")]
    [InlineData("https://example.test:444/v2/jsonapi/contacts")]
    [InlineData("https://example.test/v20/jsonapi/contacts")]
    [InlineData("/outside/jsonapi/contacts")]
    [InlineData("https://user:password@example.test/v2/jsonapi/contacts")]
    public async Task RejectsUnsafeNextLinksBeforeFetchingThem(string next)
    {
        var requests = new List<string>();

        await Assert.ThrowsAsync<InvalidOperationException>(() => JsonApiPagination.ReadPagesAsync<Contact>(
            "https://example.test/v2", "jsonapi/contacts", (url, _) =>
            {
                requests.Add(url);
                return Task.FromResult<object>(new Page<Contact>([], new() { Next = next }));
            }));

        Assert.Single(requests);
    }

    [Fact]
    public async Task RejectsAnUnsafeInitialUrlWithoutSendingARequest()
    {
        await Assert.ThrowsAsync<InvalidOperationException>(() => JsonApiPagination.ReadPagesAsync<Contact>(
            "https://example.test", "https://untrusted.test/jsonapi/contacts",
            (_, _) => throw new Exception("The unsafe URL must not be fetched.")));
    }

    [Theory]
    [InlineData("?page[number]=1")]
    [InlineData("?page[number]=1#different-fragment")]
    public async Task RejectsRepeatedPagesIncludingFragmentVariations(string next)
    {
        var requests = 0;

        var error = await Assert.ThrowsAsync<InvalidOperationException>(() => JsonApiPagination.ReadPagesAsync<Contact>(
            "https://example.test", "jsonapi/contacts?page[number]=1", (_, _) =>
            {
                requests++;
                return Task.FromResult<object>(new Page<Contact>([], new() { Next = next }));
            }));

        Assert.Contains("repeated", error.Message);
        Assert.Equal(1, requests);
    }

    [Theory]
    [InlineData("null-document")]
    [InlineData("null-data")]
    [InlineData("single-resource")]
    [InlineData("wrong-resource-type")]
    [InlineData("missing-identity")]
    [InlineData("null-resource")]
    [InlineData("missing-included-identity")]
    public async Task MalformedCollectionsCannotLookLikeSuccessfulEmptyResults(string kind)
    {
        object document = kind switch
        {
            "null-document" => null,
            "null-data" => new Page<Contact>(null),
            "single-resource" => new ContactDocument { Data = new() { Type = "contacts", Id = "1" } },
            "wrong-resource-type" => new Page<Asset>([]),
            "missing-identity" => new Page<Contact>([new() { Id = "1" }]),
            "null-resource" => new Page<Contact>([null]),
            _ => new Page<Contact>([], Included: [new ContactAddress { Type = "contact-addresses" }])
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => JsonApiPagination.ReadPagesAsync<Contact>(
            "https://example.test", "jsonapi/contacts", (_, _) => Task.FromResult(document)));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task CancellationStopsPagingAndIsPassedToTheRequest(bool beforeFirstPage)
    {
        using var cancellation = new CancellationTokenSource();
        var requests = 0;
        if (beforeFirstPage) cancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => JsonApiPagination.ReadPagesAsync<Contact>(
            "https://example.test", "jsonapi/contacts", (_, token) =>
            {
                Assert.Equal(cancellation.Token, token);
                requests++;
                cancellation.Cancel();
                return Task.FromResult<object>(new Page<Contact>([], new() { Next = "?page[number]=2" }));
            }, cancellationToken: cancellation.Token));

        Assert.Equal(beforeFirstPage ? 0 : 1, requests);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task FluentQueriesKeepTheirParametersAndReadAllPagesOnlyWhenRequested(bool allPages)
    {
        using var handler = new SnapshotTests.RecordingHandler(request =>
        {
            Assert.Equal(HttpMethod.Get, request.Method);
            Assert.Equal("/v2/jsonapi/contacts", request.RequestUri!.AbsolutePath);
            if (request.RequestUri.Query.Contains("cursor"))
                return """{"data":[{"type":"contacts","id":"2"}]}""";

            var query = Uri.UnescapeDataString(request.RequestUri.Query);
            Assert.Contains("page[number]=1", query);
            Assert.Contains("page[size]=100", query);
            Assert.Contains("sort=-id", query);
            Assert.Contains("filter=equals(firstName,'Alex')", query);
            Assert.Contains("fields[contacts]=email", query);
            Assert.Contains("include=contactAddress", query);

            return """{"data":[{"type":"contacts","id":"1"}],"links":{"next":"?cursor=next"}}""";
        });
        using var http = new HttpClient(handler);
        using var adapter = new HttpClientRequestAdapter(new AnonymousAuthenticationProvider(), httpClient: http) { BaseUrl = "https://example.test/v2" };
        var query = new MyCrmApiClient(adapter).Jsonapi.Contacts.Page(1, 100).Sort("id", SortOrder.Descending)
            .Filter("equals(firstName,'Alex')").Fields("contacts", "email").Include("contactAddress");

        var contacts = allPages ? await query.GetAllAsync<Contact>() : (await query.GetAsync()).Data;

        Assert.Equal(allPages ? 2 : 1, contacts.Count);
        Assert.Equal(contacts.Count, handler.Requests.Count);
    }
}
