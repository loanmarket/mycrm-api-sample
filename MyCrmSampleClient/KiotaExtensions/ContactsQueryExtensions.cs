using System;
using System.Threading;
using System.Threading.Tasks;
using MyCrmSampleClient.Kiota.Models;
using RootContactsRequestBuilder = MyCrmSampleClient.Kiota.Jsonapi.Contacts.ContactsRequestBuilder;

namespace MyCrmSampleClient.KiotaExtensions;

public static class ContactsQueryExtensions
{
    public static JsonApiSearchBuilder<ContactsDocument> Page(this RootContactsRequestBuilder requestBuilder, int number = 1, int size = 10) =>
        CreateBuilder(requestBuilder).Page(number, size);

    public static JsonApiSearchBuilder<ContactsDocument> Sort(this RootContactsRequestBuilder requestBuilder, string field, SortOrder order = SortOrder.Ascending) =>
        CreateBuilder(requestBuilder).Sort(field, order);
    
    public static JsonApiSearchBuilder<ContactsDocument> Include(this RootContactsRequestBuilder requestBuilder, params string[] includes) =>
        CreateBuilder(requestBuilder).Include(includes);

    public static JsonApiSearchBuilder<ContactsDocument> Filter(this RootContactsRequestBuilder requestBuilder, params string[] filters) =>
        CreateBuilder(requestBuilder).Filter(filters);

    public static JsonApiSearchBuilder<ContactsDocument> Fields(this RootContactsRequestBuilder requestBuilder, string resourceType, params string[] fields) =>
        CreateBuilder(requestBuilder).Fields(resourceType, fields);

    private static JsonApiSearchBuilder<ContactsDocument> CreateBuilder(RootContactsRequestBuilder requestBuilder)
    {
        if (requestBuilder == null) throw new ArgumentNullException(nameof(requestBuilder));

        return new JsonApiSearchBuilder<ContactsDocument>(
            () => requestBuilder.ToGetRequestInformation(),
            (rawUrl, cancellationToken) => ExecuteGet(requestBuilder, rawUrl, cancellationToken));
    }

    private static Task<ContactsDocument> ExecuteGet(
        RootContactsRequestBuilder requestBuilder,
        string rawUrl,
        CancellationToken cancellationToken) =>
        requestBuilder.WithUrl(rawUrl).GetAsync(cancellationToken: cancellationToken);
}
