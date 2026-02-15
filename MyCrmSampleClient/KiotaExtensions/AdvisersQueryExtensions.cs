using System;
using System.Threading;
using System.Threading.Tasks;
using MyCrmSampleClient.Kiota.Models;
using RootAdvisersRequestBuilder = MyCrmSampleClient.Kiota.Jsonapi.Advisers.AdvisersRequestBuilder;

namespace MyCrmSampleClient.KiotaExtensions;

public static class AdvisersQueryExtensions
{
    public static JsonApiSearchBuilder<AdvisersDocument> Page(this RootAdvisersRequestBuilder requestBuilder, int number = 1, int size = 10) =>
        CreateBuilder(requestBuilder).Page(number, size);

    public static JsonApiSearchBuilder<AdvisersDocument> Sort(this RootAdvisersRequestBuilder requestBuilder, string field, SortOrder order = SortOrder.Ascending) =>
        CreateBuilder(requestBuilder).Sort(field, order);

    public static JsonApiSearchBuilder<AdvisersDocument> Include(this RootAdvisersRequestBuilder requestBuilder, params string[] includes) =>
        CreateBuilder(requestBuilder).Include(includes);

    public static JsonApiSearchBuilder<AdvisersDocument> Filter(this RootAdvisersRequestBuilder requestBuilder, params string[] filters) =>
        CreateBuilder(requestBuilder).Filter(filters);

    public static JsonApiSearchBuilder<AdvisersDocument> Fields(this RootAdvisersRequestBuilder requestBuilder, string resourceType, params string[] fields) =>
        CreateBuilder(requestBuilder).Fields(resourceType, fields);

    private static JsonApiSearchBuilder<AdvisersDocument> CreateBuilder(RootAdvisersRequestBuilder requestBuilder)
    {
        if (requestBuilder == null) throw new ArgumentNullException(nameof(requestBuilder));

        return new JsonApiSearchBuilder<AdvisersDocument>(
            () => requestBuilder.ToGetRequestInformation(),
            (rawUrl, cancellationToken) => ExecuteGet(requestBuilder, rawUrl, cancellationToken));
    }

    private static Task<AdvisersDocument> ExecuteGet(
        RootAdvisersRequestBuilder requestBuilder,
        string rawUrl,
        CancellationToken cancellationToken) =>
        requestBuilder.WithUrl(rawUrl).GetAsync(cancellationToken: cancellationToken);
}

