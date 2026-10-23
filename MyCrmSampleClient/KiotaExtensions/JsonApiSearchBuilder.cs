using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Kiota.Abstractions;
using MyCrmSampleClient.Kiota.Models;

namespace MyCrmSampleClient.KiotaExtensions;

public sealed class JsonApiSearchBuilder<TDocument>
{
    private readonly Func<RequestInformation> _createRequestInformation;
    private readonly Func<string, CancellationToken, Task<TDocument>> _executeGet;
    private readonly List<string> _filters = new();
    private readonly List<string> _sort = new();
    private readonly List<string> _include = new();
    private readonly Dictionary<string, string> _fields = new(StringComparer.OrdinalIgnoreCase);
    private int? _pageNumber;
    private int? _pageSize;

    internal JsonApiSearchBuilder(
        Func<RequestInformation> createRequestInformation,
        Func<string, CancellationToken, Task<TDocument>> executeGet)
    {
        _createRequestInformation = createRequestInformation ?? throw new ArgumentNullException(nameof(createRequestInformation));
        _executeGet = executeGet ?? throw new ArgumentNullException(nameof(executeGet));
    }

    public JsonApiSearchBuilder<TDocument> Page(int number, int size)
    {
        if (number < 1) throw new ArgumentOutOfRangeException(nameof(number));
        if (size < 1) throw new ArgumentOutOfRangeException(nameof(size));

        _pageNumber = number;
        _pageSize = size;
        return this;
    }

    public JsonApiSearchBuilder<TDocument> Sort(string field, SortOrder order = SortOrder.Ascending)
    {
        if (string.IsNullOrWhiteSpace(field)) throw new ArgumentException("Sort field cannot be empty.", nameof(field));

        _sort.Add(order == SortOrder.Descending ? $"-{field}" : field);
        return this;
    }

    public JsonApiSearchBuilder<TDocument> Include(params string[] includes)
    {
        JsonApiQueryBuilderHelpers.AddValues(_include, includes);
        return this;
    }

    public JsonApiSearchBuilder<TDocument> Filter(params string[] filters)
    {
        JsonApiQueryBuilderHelpers.AddValues(_filters, filters);
        return this;
    }

    public JsonApiSearchBuilder<TDocument> Fields(string resourceType, params string[] fields)
    {
        if (string.IsNullOrWhiteSpace(resourceType))
        {
            throw new ArgumentException("Resource type cannot be empty.", nameof(resourceType));
        }

        var sanitizedFields = JsonApiQueryBuilderHelpers.SanitizeValues(fields);
        if (sanitizedFields.Count > 0)
        {
            _fields[resourceType] = string.Join(",", sanitizedFields);
        }

        return this;
    }

    public async Task<TDocument> GetAsync(CancellationToken cancellationToken = default)
    {
        var requestInfo = CreateRequestInformation();

        return await _executeGet(CreateRequestUrl(requestInfo), cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Reads every page of a collection; GetAsync still reads a single page or item.</summary>
    public Task<List<TResource>> GetAllAsync<TResource>(Action<IncludedResource> onResource = null,
        CancellationToken cancellationToken = default) where TResource : IncludedResource
    {
        var requestInfo = CreateRequestInformation();
        var baseUrl = requestInfo.PathParameters.TryGetValue("baseurl", out var value) ? value?.ToString() : null;
        if (string.IsNullOrWhiteSpace(baseUrl))
            throw new InvalidOperationException("Configure the API base URL before reading all pages.");

        return JsonApiPagination.ReadPagesAsync<TResource>(baseUrl, CreateRequestUrl(requestInfo),
            async (url, token) => await _executeGet(url, token).ConfigureAwait(false), onResource, cancellationToken);
    }

    private RequestInformation CreateRequestInformation()
    {
        var requestInfo = _createRequestInformation();

        if (!requestInfo.PathParameters.ContainsKey("baseurl"))
        {
            var baseUrl = JsonApiFluentContext.BaseUrl;
            if (!string.IsNullOrWhiteSpace(baseUrl))
            {
                requestInfo.PathParameters["baseurl"] = baseUrl;
            }
        }

        return requestInfo;
    }

    private string CreateRequestUrl(RequestInformation requestInfo) =>
        JsonApiQueryBuilderHelpers.ApplyCommonQueryParameters(requestInfo.URI, _filters, _sort, _include, _pageNumber, _pageSize, _fields);
}

internal static class JsonApiQueryBuilderHelpers
{
    internal static string ApplyCommonQueryParameters(
        Uri uri,
        List<string> filters,
        List<string> sort,
        List<string> include,
        int? pageNumber,
        int? pageSize,
        Dictionary<string, string> fields)
    {
        // Generated templates may omit sort/page entirely, or expose fields/page as
        // maps. Build the raw query so bracketed JSON:API keys are never discarded.
        var parameters = new List<KeyValuePair<string, string>>();
        foreach (var filter in filters) parameters.Add(new("filter", filter));

        if (sort.Count > 0)
            parameters.Add(new("sort", string.Join(",", sort)));

        if (include.Count > 0)
            parameters.Add(new("include", string.Join(",", include)));

        if (pageNumber.HasValue)
            parameters.Add(new("page[number]", pageNumber.Value.ToString(CultureInfo.InvariantCulture)));

        if (pageSize.HasValue)
            parameters.Add(new("page[size]", pageSize.Value.ToString(CultureInfo.InvariantCulture)));

        foreach (var pair in fields)
            parameters.Add(new($"fields[{pair.Key}]", pair.Value));

        var replacedKeys = parameters.Select(parameter => parameter.Key).ToHashSet(StringComparer.Ordinal);
        var existing = uri.Query.TrimStart('?').Split('&', StringSplitOptions.RemoveEmptyEntries)
            .Where(parameter => !replacedKeys.Contains(Uri.UnescapeDataString(parameter.Split('=')[0])));
        var added = parameters.Select(parameter => Uri.EscapeDataString(parameter.Key) + "=" + Uri.EscapeDataString(parameter.Value));

        return new UriBuilder(uri) { Query = string.Join("&", existing.Concat(added)) }.Uri.AbsoluteUri;
    }

    internal static void AddValues(List<string> target, params string[] values)
    {
        target.AddRange(SanitizeValues(values));
    }

    internal static List<string> SanitizeValues(params string[] values)
    {
        if (values == null || values.Length == 0)
        {
            return new List<string>();
        }

        return values
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .Where(x => x.Length > 0)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }
}
