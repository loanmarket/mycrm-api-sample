using System;
using System.Threading;
using System.Threading.Tasks;
using MyCrmSampleClient.Kiota.Jsonapi.ContactGroups;
using MyCrmSampleClient.Kiota.Models;

namespace MyCrmSampleClient.KiotaExtensions;

public static class ContactGroupsQueryExtensions
{
    public static JsonApiSearchBuilder<ContactGroupsDocument> Page(this ContactGroupsRequestBuilder requestBuilder, int number = 1, int size = 10) =>
        CreateBuilder(requestBuilder).Page(number, size);

    public static JsonApiSearchBuilder<ContactGroupsDocument> Sort(this ContactGroupsRequestBuilder requestBuilder, string field, SortOrder order = SortOrder.Ascending) =>
        CreateBuilder(requestBuilder).Sort(field, order);
    
    public static JsonApiSearchBuilder<ContactGroupsDocument> Include(this ContactGroupsRequestBuilder requestBuilder, params string[] includes) =>
        CreateBuilder(requestBuilder).Include(includes);

    public static JsonApiSearchBuilder<ContactGroupsDocument> Filter(this ContactGroupsRequestBuilder requestBuilder, params string[] filters) =>
        CreateBuilder(requestBuilder).Filter(filters);

    public static JsonApiSearchBuilder<ContactGroupsDocument> Fields(this ContactGroupsRequestBuilder requestBuilder, string resourceType, params string[] fields) =>
        CreateBuilder(requestBuilder).Fields(resourceType, fields);

    private static JsonApiSearchBuilder<ContactGroupsDocument> CreateBuilder(ContactGroupsRequestBuilder requestBuilder)
    {
        if (requestBuilder == null) throw new ArgumentNullException(nameof(requestBuilder));

        return new JsonApiSearchBuilder<ContactGroupsDocument>(
            () => requestBuilder.ToGetRequestInformation(),
            (rawUrl, cancellationToken) => ExecuteGet(requestBuilder, rawUrl, cancellationToken));
    }

    private static Task<ContactGroupsDocument> ExecuteGet(
        ContactGroupsRequestBuilder requestBuilder,
        string rawUrl,
        CancellationToken cancellationToken) =>
        requestBuilder.WithUrl(rawUrl).GetAsync(cancellationToken: cancellationToken);
}
