using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyCrmSampleClient.Kiota.Models;

namespace MyCrmSampleClient.KiotaExtensions;

/// <summary>Reads a JSON:API collection by following the server's links.next.</summary>
public static class JsonApiPagination
{
    /// <param name="baseUrl">The configured API base URL, including any path prefix.</param>
    /// <param name="url">The initial collection URL, absolute or relative to baseUrl.</param>
    /// <param name="fetch">Calls the generated request builder's WithUrl(url).GetAsync().</param>
    /// <param name="onResource">Optional collector for primary and included resources from every page.</param>
    public static async Task<List<T>> ReadPagesAsync<T>(string baseUrl, string url,
        Func<string, CancellationToken, Task<object>> fetch, Action<IncludedResource> onResource = null,
        CancellationToken cancellationToken = default) where T : IncludedResource
    {
        ArgumentNullException.ThrowIfNull(fetch);
        ArgumentException.ThrowIfNullOrWhiteSpace(baseUrl);
        ArgumentException.ThrowIfNullOrWhiteSpace(url);

        var origin = new Uri(baseUrl.TrimEnd('/') + "/", UriKind.Absolute);
        var records = new Dictionary<(string Type, string Id), T>();
        var visited = new HashSet<string>(StringComparer.Ordinal);

        while (url != null)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var uri = new Uri(origin, url);
            if (uri.Scheme != origin.Scheme || uri.Authority != origin.Authority || uri.UserInfo.Length > 0 ||
                !uri.AbsolutePath.StartsWith(origin.AbsolutePath, StringComparison.Ordinal))
                throw new InvalidOperationException("The API returned a pagination link outside the configured API host or base path.");

            // Fragments are not sent over HTTP and must not disguise a repeated request.
            uri = new UriBuilder(uri) { Fragment = "" }.Uri;
            if (!visited.Add(uri.AbsoluteUri)) throw new InvalidOperationException("The API returned a repeated pagination link.");

            var doc = await fetch(uri.AbsoluteUri, cancellationToken).ConfigureAwait(false)
                ?? throw new InvalidOperationException("The API returned an empty document.");

            // Kiota's generated collection documents do not implement a common interface.
            var docType = doc.GetType();
            if (docType.GetProperty("Data")?.GetValue(doc) is not IEnumerable<T> data)
                throw new InvalidOperationException($"The API response has no data array of {typeof(T).Name} resources.");

            foreach (var item in data)
            {
                RequireIdentity(item);
                records[(item.Type, item.Id)] = item;
                onResource?.Invoke(item);
            }

            foreach (var item in (IEnumerable<IncludedResource>)docType.GetProperty("Included")?.GetValue(doc) ?? [])
            {
                RequireIdentity(item);
                onResource?.Invoke(item);
            }

            // A short or empty page can still have a next link. Resolve query-only and
            // relative links against the page that supplied them, not the API root.
            var next = ((TopLevelLinks)docType.GetProperty("Links")?.GetValue(doc))?.Next;
            url = string.IsNullOrWhiteSpace(next) ? null : new Uri(uri, next).AbsoluteUri;
        }

        return records.Values.ToList();
    }

    private static void RequireIdentity(IncludedResource resource)
    {
        if (resource == null || string.IsNullOrEmpty(resource.Type) || string.IsNullOrEmpty(resource.Id))
            throw new InvalidOperationException("A read resource has no type or ID.");
    }
}
