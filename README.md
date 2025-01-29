# MyCRM Sample

To use these samples you will need credentials and details supplied by Loan Market Group.

## Gaining access

Customers and partners wishing to access the api will need to request credentials for each interacting application.

The following information will be required to support your request:

- The operation(s) you wish to perform (e.g. read contacts)
- The context of the access. For example, it may be for a broker organisation or based on a subscription agreement
- If you intend to use real-time updates from MyCRM via webhooks

## Reviewing the API

You can review our [API specification online](https://api-docs.mycrm.io/) or the full OpenAPI document is available in [MyCrmSampleClient/swagger.json](MyCrmSampleClient/swagger.json).

The API conforms to the [json:api](https://jsonapi.org/) standard.

## Using the Postman Collection

The included [Postman Collection](MyCRM%20API%20Sample.postman_collection.json) can be imported into [Postman](https://www.postman.com/) to review the behaviour of the MyCRM API.

To begin, go to the `Variables` tab and populate the following values:

- Auth_Url
- ClientID
- ClientSecret
- AdviserContactId
- PublicAPI_Url
- Scopes

This will allow you to execute the various sample operations. The OAuth 2.0 authorization flow is handled by the `Pre-request Script`.

## Using the Sample Client

Go to the [MyCrmSampleClient](./MyCrmSampleClient) folder and edit the [appsettings.json](./MyCrmSampleClient/appsettings.json) file to populate the following values:

```json
{
    "Auth": {
        "Url": "",
        "ClientID": "",
        "ClientSecret": "",
        "Scopes": ""
    },
    "MyCRM": {
        "Url": "",
        "AdviserContactId": 0
    }
}
```

Then run `dotnet run`

## Data access

The key differences between API and MyCRM user access are:

- Credentials for an application are not issued for a single user. At a minimum, it is issued for an organisation
- Records shared with an organisation/user but not owned by them will not be accessible via the API

The nominated `AdviserContactId` in the samples is sent as the `UserId` header with each request.

- During maintenance operations (`POST`, `PATCH`, `DELETE`) the `UserId` will be treated as the creator/last modified user of the record. For leads as an example, this will result in the lead being allocated to them.
- In most `read` operations the `UserId` header has no impact
- In some `search` operations it may limit responses to records owned by the nominated user

## Understanding Scopes

[OAuth scopes](https://datatracker.ietf.org/doc/html/rfc6749#section-3.3) are used to control the access you have for the MyCRM api. You will be informed of the scopes available to you when your credentials are issued.

Example scopes are:

- `api`: full access to all endpoints
- `api.read`: read access to all entities by their key (e.g. GET `/jsonapi/contacts/{id}`)
- `api.search`: access to query for any entity (e.g. GET `/jsonapi/contacts`)
- `api.leads`: access to the `/jsonapi/leads` endpoints to create and monitor leads
- `api.marketing`: access to the `/jsonapi/contact-marketing` endpoints to update marketing consent
- `api.contacts`: access to the `/jsonapi/contacts` endpoints to manage contacts
- `api.contacts.read`: access to read contact details via the GET `/jsonapi/contacts/{id}` endpoint

You will only be able to access an endpoint if you request an applicable scope, regardless of the scopes available to you.

## Advanced queries

Most `search` operations support the following features:

- **Sorting**: Specify the sort order of the responses. The default is `id`
- **Pagination**: Paginate through the responses. All queries are limited to the maximum page size even when not specified
- **Filtering**: Query based on criteria

Most `read` and `search` operations support the following features:

- **Includes**: Include related entities in the response
- **Field Selection**: Limit the fields included in the response to improve performance

### Sorting

The following sort rules may be applied to a `search` query, however not all fields support sorting.

| Operation  | Operator    | Example                     |
| ---------- | ----------- | --------------------------- |
| Ascending  | *none*    | `?sort=id`                |
| Descending | **-** | `?sort=-id`               |
| Multiple   | **,** | `?sort=lastName,-updated` |

The default sort order is ascending by `id`.

### Pagination

The following pagination options are available on `search` queries.

| Operation   | Function               | Example             |
| ----------- | ---------------------- | ------------------- |
| Page Size   | **page[size]**   | `?page[size]=50`  |
| Page Number | **page[number]** | `?page[number]=2` |

The maximum and default page size is `100` and the minimum and default page number is `1`.

### Filtering

The following filters may be applied to a `search` query, however not all fields support filtering.

| Operation                 | Function                 | Example                                                                               |
| ------------------------- | ------------------------ | ------------------------------------------------------------------------------------- |
| Equality                  | **equals**         | `?filter=equals(hasMarketingConsent,'true')`                                        |
| Less than                 | **lessThan**       | `?filter=lessThan(value,'25000')`                                                   |
| Less than or equal to     | **lessOrEqual**    | `?filter=lessOrEqual(created,'2020-12-31')`                                         |
| Greater than              | **greaterThan**    | `?filter=greaterThan(value,'25000')`                                                |
| Greater than or equal to  | **greaterOrEqual** | `?filter=greaterThan(updated,'2021-07-01')`                                         |
| Contains text             | **contains**       | `?filter=contains(email,'acme.com')`                                                |
| Starts with text          | **startsWith**     | `?filter=startsWith(firstName,'Rob')`                                               |
| Ends with text            | **endsWith**       | `?filter=endsWith(lastName,'Smythe')`                                               |
| Equals one value from set | **any**            | `?filter=any(firstName,'Kate','Joan','Robert')`                                     |
| A relationship exists     | **has**            | `?filter=has(contactAddress)`                                                       |
| Negation                  | **not**            | `?filter=not(equals(firstName,'Mary'))`                                             |
| One criterion must apply  | **or**             | `?filter=or(equals(firstName,'Kate'),equals(preferredName,'Kate'))`                 |
| All criteria must apply   | **and**            | `?filter=and(equals(hasMarketingConsent,'true'),greaterThan(updated,'2021-07-01'))` |

Regardless of data type the filter value should be enclosed in single quotes. Dates should be in `yyyy-MM-dd` format.

### Includes

When querying you may also include related resources in the response. Using the following query as an example:

`/jsonapi/contact-groups/7117726?include=contacts,contacts.contactAddress`

Both the contact and their addresses will be returned. The information is normalised so that the keys are embedded in the `relationships` section pointing to the records in the `included` section. The following response is shortened for brevity and to highlight the behaviour.

```json
{
  "data":{
    "type":"contact-groups",
    "id":"7117726",
    "attributes":{
      "created":"2022-11-11T03:42:03.8766667+00:00",
      "enquirySourceCategory": "Self Generated",
      "enquirySource": "Referral from existing client"
    },
    "relationships":{
      "contacts":{
        "links":{},
        "data":[
          { "type":"contacts", "id":"8503844" },
          { "type":"contacts", "id":"8503845" }      
        ]
      }
    },
    "links":{}
  },
  "included":[
    {
      "type":"contacts",
      "id":"8503844",
      "attributes":{
        "created":"2022-11-11T03:42:03.99+00:00",
        "firstName": "Evangelia",
        "lastName": "Rodriguez",
        "isPrimary": true
      },
      "contactAddress":{
        "links":{},
        "data":[
          { "type":"contact-address", "id":"3189832" },
          { "type":"contact-address", "id":"3189833" }
        ]
      }
    },
    {
      "type":"contacts",
      "id":"8503845",
      "attributes":{
        "created":"2022-11-11T03:42:03.99+00:00",
        "firstName": "Diaz",
        "lastName": "Rodriguez",
        "isPrimary": false
      },
      "contactAddress":{
        "links":{},
        "data":[]
      }
    },  
    {
      "type":"contact-address",
      "id":"3189832",
      "attributes":{
        "addressType":"PostalAddress",
        "formattedAddress": "PO Box 121, Athol Park SA 5012, Australia",
      }
    },
    {
      "type":"contact-address",
      "id":"3189833",
      "attributes":{
        "addressType":"PreviousAddress",
        "formattedAddress": "11/21 Frenchs Blvd, Athol Park SA 5012, Australia",
      }
    }
  ]
}
```

### Field Selection

The default behaviour is to include all available fields in the response, however reducing the fields to only those needed can improve the performance of your query. Filtering applies to both the `attributes` and `relationships` sections of the response.

For example, the following query:

`/jsonapi/contacts/8503844?fields[contacts]=firstName,lastName,email`

Produces this response:

```json
{
  "links":{
    "self":"/jsonapi/contacts/8503844?fields[contacts]=firstName,lastName,email"
  },
  "data":{
    "type":"contacts",
    "id":"8503844",
    "attributes":{
      "firstName": "Evangelia",
      "lastName": "Rodriguez",
      "email":"ev123442@hotmail.com"
    },
    "links":{
      "self":"/jsonapi/contacts/8503844"
    }
  }
}
```

In the same manner, fields can be restricted for included entities:

`/jsonapi/contacts/1894732?fields[contacts]=firstName,lastName,email,contactAddress&include=contactAddress&fields[contact-address]=addressType,formattedAddress`

Things to note:

- Including `contactAddress` in the fields for `contact` ensures the relationship details are included in the response. If not specified the addresses would appear in the `included` section but with no link back to the `contact`. For `read` requests this is not strictly necessary but is logically required for a `search`.
- When specifying the fields for the address it is the entity type `contact-address` not the relationship name `contactAddress` which is used. If addresses where linked to a contact by multiple relationships, the field selection would apply regardless of the relationship.

## Real-time Updates via Webhooks

[Webhooks](https://en.wikipedia.org/wiki/Webhook) are a common method of loose integration between web accessible systems. MyCRM supports webhooks for some scenarios, including organisation and advisor changes. Once configured, MyCRM will `POST` to your web endpoint sending a JSON body similar to:

```json
{
  "action": "updated",
  "resource": {
    "type": "organisations",
    "id": 1123,
    "url": "https://api.mycrm/jsonapi/organisations/1123"
  },
  "webhook": {
    "name": "Example Organisation Webhook"
  }
}
```

On successful receipt your system should respond with HTTP status `200 OK`. If the callback fails we will retry a number of times over a short period before finally failing the notification.

## Rate Limiting

Requests to the API are subject to rate limiting to ensure fair use. Once your requests hit the limit, the API will respond with a 429 Too Many Requests until you earn enough tokens to make another request. MyCRM applies a token bucket algorithm with a variable cost per request, where the cost is an estimation of the underlying capacity reserved to respond the request.

The following response headers relating to rate limiting will be returned for each request:

| Header                | Purpose                                                                                                                          |
| --------------------- | -------------------------------------------------------------------------------------------------------------------------------- |
| retry-after           | The minimum period to wait to make a follow-up request. Depending on the cost of the request it may be necessary to wait longer |
| x-ratelimit-cost      | The tokens required to perform the request                                                                                       |
| x-ratelimit-limit     | The total tokens earnt over a 5 minute period                                                                                  |
| x-ratelimit-remaining | The number of tokens currently left                                                                                              |

LMG reserves the right to update the rate limiting rules to protect the system. At this time, the following factors affect the rate limiting cost calculation.

#### Time of Day

Higher limits are available outside of Business Hours. Due to the breadth of operations, Business Hours is defined as 08:00 to 18:00 AEST (Brisbane, Australia) time.

**Recommended Action:** Perform operations out of hours where possible.

#### Type of Record

As a general guide, the more complex the record type the more expensive the request. For example, querying `deals` is more expensive than querying `referrers`.

**Recommended Action:** Review your requests relating to complex records, particularly any search endpoint.

#### Type of Request

The following table approximates the base cost by type of request

| Request Type        | Example Endpoint                                       | Cost    |
| ------------------- | ------------------------------------------------------ | ------- |
| Search              | GET `/jsonapi/deals`                                 | `$$$` |
| Fetch               | GET `/jsonapi/deals/{id}`                            | `$$`  |
| Fetch Related       | GET `/jsonapi/deals/{id}/participants`               | `$$`  |
| Fetch Relationships | GET `/jsonapi/deals/{id}/relationships/participants` | `$`   |
| Create              | POST `/jsonapi/leads`                                | `$$`  |
| Update              | PATCH `/jsonapi/contact-marketing/{id}`              | `$$`  |
| Delete              | DELETE `/jsonapi/deal-notes/{id}`                    | `$`   |

**Recommended Action:** Review your use of expensive endpoints, particularly any search. Be aware that the use of pagination, sort and includes may dramatically increase the cost of the request. Use `Fetch Relationships` instead of `Fetch Related` if all that is required is the `id` of the related entity.

#### Search Pagination

Use of pagination within a search such as `?page[number]={no}`, incurs an additional ****multiplicative**** cost if the page number is greater than `1`. The cost increases as the page number increases.

**Recommended Action:** Avoid pagination where possible. One solution is to use a query similar to `/jsonapi/deals?sort=id&filter=greaterThan(id, '{id}')` where `{id}` the maximum (or mimimum depending on sort order) id from the previous request to the same endpoint. This achieves a paginated result without incurring the additional cost.

#### Search Sorting

Use of any sort other than `?sort=id` or `?sort=-id` will incur an additional **multiplicative** cost.

**Recommended Action:** Avoid sorting by columns other than `id` when possible.

#### Includes

Use of includes such as `?include=contacts,contacts.contactAddress` will incur an additional ****multiplicative**** cost for each include. The deeper the include, the higher the cost.

**Recommended Action:** Limit the use of includes where possible, particularly with search endpoints. It may also be possible to call a different endpoint which is closer to the required data and so doesn't require as deep an include. For example, rather than calling the `contact` endpoint to get a linked asset adress, use the `asset` endpoint.

