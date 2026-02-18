using System;
using System.Threading;
using System.Threading.Tasks;
using MyCrmSampleClient.Kiota.Models;
using ContactsItemRequestBuilder = MyCrmSampleClient.Kiota.Jsonapi.Contacts.Item.ContactsItemRequestBuilder;

namespace MyCrmSampleClient.KiotaExtensions;

public static class ContactItemQueryExtensions
{
    public static JsonApiSearchBuilder<ContactDocument> Include(this ContactsItemRequestBuilder requestBuilder, params string[] includes) =>
        CreateBuilder(requestBuilder).Include(includes);

    private static JsonApiSearchBuilder<ContactDocument> CreateBuilder(ContactsItemRequestBuilder requestBuilder)
    {
        if (requestBuilder == null) throw new ArgumentNullException(nameof(requestBuilder));

        return new JsonApiSearchBuilder<ContactDocument>(
            () => requestBuilder.ToGetRequestInformation(),
            (rawUrl, cancellationToken) => ExecuteGet(requestBuilder, rawUrl, cancellationToken));
    }

    private static Task<ContactDocument> ExecuteGet(
        ContactsItemRequestBuilder requestBuilder,
        string rawUrl,
        CancellationToken cancellationToken) =>
        requestBuilder.WithUrl(rawUrl).GetAsync(cancellationToken: cancellationToken);
}
