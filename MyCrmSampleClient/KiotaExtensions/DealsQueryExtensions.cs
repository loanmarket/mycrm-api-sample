using System;
using System.Threading;
using System.Threading.Tasks;
using MyCrmSampleClient.Kiota.Models;
using RootDealsRequestBuilder = MyCrmSampleClient.Kiota.Jsonapi.Deals.DealsRequestBuilder;

namespace MyCrmSampleClient.KiotaExtensions;

public static class DealsQueryExtensions
{
    public static JsonApiSearchBuilder<DealsDocument> Page(this RootDealsRequestBuilder requestBuilder, int number = 1, int size = 10) =>
        CreateBuilder(requestBuilder).Page(number, size);

    public static JsonApiSearchBuilder<DealsDocument> Sort(this RootDealsRequestBuilder requestBuilder, string field, SortOrder order = SortOrder.Ascending) =>
        CreateBuilder(requestBuilder).Sort(field, order);

    public static JsonApiSearchBuilder<DealsDocument> Include(this RootDealsRequestBuilder requestBuilder, params string[] includes) =>
        CreateBuilder(requestBuilder).Include(includes);

    public static JsonApiSearchBuilder<DealsDocument> Filter(this RootDealsRequestBuilder requestBuilder, params string[] filters) =>
        CreateBuilder(requestBuilder).Filter(filters);

    public static JsonApiSearchBuilder<DealsDocument> Fields(this RootDealsRequestBuilder requestBuilder, string resourceType, params string[] fields) =>
        CreateBuilder(requestBuilder).Fields(resourceType, fields);

    private static JsonApiSearchBuilder<DealsDocument> CreateBuilder(RootDealsRequestBuilder requestBuilder)
    {
        if (requestBuilder == null) throw new ArgumentNullException(nameof(requestBuilder));

        return new JsonApiSearchBuilder<DealsDocument>(
            () => requestBuilder.ToGetRequestInformation(),
            (rawUrl, cancellationToken) => ExecuteGet(requestBuilder, rawUrl, cancellationToken));
    }

    private static Task<DealsDocument> ExecuteGet(
        RootDealsRequestBuilder requestBuilder,
        string rawUrl,
        CancellationToken cancellationToken) =>
        requestBuilder.WithUrl(rawUrl).GetAsync(cancellationToken: cancellationToken);
}
