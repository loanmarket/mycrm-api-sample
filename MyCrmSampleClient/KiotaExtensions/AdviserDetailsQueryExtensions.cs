using System;
using System.Threading;
using System.Threading.Tasks;
using MyCrmSampleClient.Kiota.Models;
using RootAdviserDetailsRequestBuilder = MyCrmSampleClient.Kiota.Jsonapi.AdviserDetails.AdviserDetailsRequestBuilder;

namespace MyCrmSampleClient.KiotaExtensions;

public static class AdviserDetailsQueryExtensions
{
    public static JsonApiSearchBuilder<AdviserDetailsDocument> Page(this RootAdviserDetailsRequestBuilder requestBuilder, int number = 1, int size = 10) =>
        CreateBuilder(requestBuilder).Page(number, size);

    public static JsonApiSearchBuilder<AdviserDetailsDocument> Sort(this RootAdviserDetailsRequestBuilder requestBuilder, string field, SortOrder order = SortOrder.Ascending) =>
        CreateBuilder(requestBuilder).Sort(field, order);

    public static JsonApiSearchBuilder<AdviserDetailsDocument> Include(this RootAdviserDetailsRequestBuilder requestBuilder, params string[] includes) =>
        CreateBuilder(requestBuilder).Include(includes);

    public static JsonApiSearchBuilder<AdviserDetailsDocument> Filter(this RootAdviserDetailsRequestBuilder requestBuilder, params string[] filters) =>
        CreateBuilder(requestBuilder).Filter(filters);

    public static JsonApiSearchBuilder<AdviserDetailsDocument> Fields(this RootAdviserDetailsRequestBuilder requestBuilder, string resourceType, params string[] fields) =>
        CreateBuilder(requestBuilder).Fields(resourceType, fields);

    private static JsonApiSearchBuilder<AdviserDetailsDocument> CreateBuilder(RootAdviserDetailsRequestBuilder requestBuilder)
    {
        if (requestBuilder == null) throw new ArgumentNullException(nameof(requestBuilder));

        return new JsonApiSearchBuilder<AdviserDetailsDocument>(
            () => requestBuilder.ToGetRequestInformation(),
            (rawUrl, cancellationToken) => ExecuteGet(requestBuilder, rawUrl, cancellationToken));
    }

    private static Task<AdviserDetailsDocument> ExecuteGet(
        RootAdviserDetailsRequestBuilder requestBuilder,
        string rawUrl,
        CancellationToken cancellationToken) =>
        requestBuilder.WithUrl(rawUrl).GetAsync(cancellationToken: cancellationToken);
}

