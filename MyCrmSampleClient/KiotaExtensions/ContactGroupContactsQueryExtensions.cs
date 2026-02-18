using System;
using System.Threading;
using System.Threading.Tasks;
using MyCrmSampleClient.Kiota.Models;
using GroupContactsRequestBuilder = MyCrmSampleClient.Kiota.Jsonapi.ContactGroups.Item.Contacts.ContactsRequestBuilder;

namespace MyCrmSampleClient.KiotaExtensions;

public static class ContactGroupContactsQueryExtensions
{
    public static JsonApiSearchBuilder<ContactsDocument> Page(this GroupContactsRequestBuilder requestBuilder, int number = 1, int size = 10) =>
        CreateBuilder(requestBuilder).Page(number, size);

    public static JsonApiSearchBuilder<ContactsDocument> Sort(this GroupContactsRequestBuilder requestBuilder, string field, SortOrder order = SortOrder.Ascending) =>
        CreateBuilder(requestBuilder).Sort(field, order);
    
    public static JsonApiSearchBuilder<ContactsDocument> Fields(this GroupContactsRequestBuilder requestBuilder, string resourceType, params string[] fields) =>
        CreateBuilder(requestBuilder).Fields(resourceType, fields);

    private static JsonApiSearchBuilder<ContactsDocument> CreateBuilder(GroupContactsRequestBuilder requestBuilder)
    {
        if (requestBuilder == null) throw new ArgumentNullException(nameof(requestBuilder));

        return new JsonApiSearchBuilder<ContactsDocument>(
            () => requestBuilder.ToGetRequestInformation(),
            (rawUrl, cancellationToken) => ExecuteGet(requestBuilder, rawUrl, cancellationToken));
    }

    private static Task<ContactsDocument> ExecuteGet(
        GroupContactsRequestBuilder requestBuilder,
        string rawUrl,
        CancellationToken cancellationToken) =>
        requestBuilder.WithUrl(rawUrl).GetAsync(cancellationToken: cancellationToken);
}

