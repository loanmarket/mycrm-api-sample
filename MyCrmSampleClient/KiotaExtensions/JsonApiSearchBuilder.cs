using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Kiota.Abstractions;

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
        var requestInfo = _createRequestInformation();
        JsonApiQueryBuilderHelpers.ApplyCommonQueryParameters(requestInfo.QueryParameters, _filters, _sort, _include, _pageNumber, _pageSize, _fields);

        if (!requestInfo.PathParameters.ContainsKey("baseurl"))
        {
            var baseUrl = JsonApiFluentContext.BaseUrl;
            if (!string.IsNullOrWhiteSpace(baseUrl))
            {
                requestInfo.PathParameters["baseurl"] = baseUrl;
            }
        }

        var rawUrl = requestInfo.URI.ToString();
        return await _executeGet(rawUrl, cancellationToken).ConfigureAwait(false);
    }
}

internal static class JsonApiQueryBuilderHelpers
{
    internal static void ApplyCommonQueryParameters(
        IDictionary<string, object> queryParameters,
        List<string> filters,
        List<string> sort,
        List<string> include,
        int? pageNumber,
        int? pageSize,
        Dictionary<string, string> fields)
    {
        if (filters.Count > 0)
        {
            queryParameters["filter"] = filters.ToArray();
        }

        if (sort.Count > 0)
        {
            queryParameters["sort"] = sort.ToArray();
        }

        if (include.Count > 0)
        {
            queryParameters["include"] = include.ToArray();
        }

        if (pageNumber.HasValue)
        {
            queryParameters["page[number]"] = pageNumber.Value.ToString(CultureInfo.InvariantCulture);
        }

        if (pageSize.HasValue)
        {
            queryParameters["page[size]"] = pageSize.Value.ToString(CultureInfo.InvariantCulture);
        }

        foreach (var pair in fields)
        {
            queryParameters[$"fields[{pair.Key}]"] = pair.Value;
        }
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
