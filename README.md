# Understanding the MyCRM API

To use the MyCRM API you will need credentials and details supplied by LMG.

## Contents

- [Gaining access](#gaining-access)
- [Using the Postman Collection](#using-the-postman-collection)
- [Using the Sample Client](#using-the-sample-client)
- [Understanding Scopes](#understanding-scopes)
- [Authentication](#authentication)
- [Data access](#data-access)
- [Required request headers](#required-request-headers)
- [Reviewing the API](#reviewing-the-api)
- [JSON:API overview](#jsonapi-overview)
- [Advanced queries](#advanced-queries)
  - [Sorting](#sorting)
  - [Pagination](#pagination)
  - [Filtering](#filtering)
  - [Includes](#includes)
  - [Field Selection](#field-selection)
- [Creating and updating resources](#creating-and-updating-resources)
- [Real-time Updates via Webhooks](#real-time-updates-via-webhooks)
- [Rate Limiting](#rate-limiting)

## Gaining access

Customers and partners wishing to access the API will need to request credentials for each interacting application.

The following information will be required to support your request:

- The operation(s) you wish to perform (e.g. read contacts)
- The context of the access. For example, it may be for a broker organisation or based on a subscription agreement
- If you intend to use real-time updates from MyCRM via webhooks

Please send this information to Ask LMG. Depending on the nature of the request, we may need to follow up with additional questions before issuing credentials.

If you simply wish to create leads within MyCRM you may self-serve credentials from within MyCRM under Profile Management - API Access Management. If you cannot see that page, you may need to ask the head adviser or owner of the business to assist you.

## Using the Postman Collection

The included [Postman Collection](MyCRM%20API%20Sample.postman_collection.json) can be imported into [Postman](https://www.postman.com/) to explore the MyCRM API without writing any code.

To begin, go to the `Variables` tab and populate the following values:

- `Auth_Url`
- `ClientID`
- `ClientSecret`
- `AdviserContactId`
- `PublicAPI_Url`
- `Scopes`

The OAuth 2.0 token exchange is handled automatically by the `Pre-request Script` on each request.

## Using the Sample Client

The [MyCrmSampleClient](./MyCrmSampleClient) is an interactive .NET console application that demonstrates how to authenticate and call the MyCRM API from C# code. See the [Sample Client README](./MyCrmSampleClient/README.md) for a full description of each sample.

To run it, edit [appsettings.json](./MyCrmSampleClient/appsettings.json) to populate your credentials and then run `dotnet run`:

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

## Understanding Scopes

[OAuth scopes](https://datatracker.ietf.org/doc/html/rfc6749#section-3.3) control which endpoints and operations your credentials can access. You will be informed of the scopes available to you when your credentials are issued, but you must also include the specific scopes you need when requesting an access token.

Example scopes are:

- `api`: full access to all endpoints
- `api.read`: read access to all entities by their key (e.g. GET `/jsonapi/contacts/{id}`)
- `api.search`: access to query for any entity (e.g. GET `/jsonapi/contacts`)
- `api.leads`: access to the `/jsonapi/leads` endpoints to create and monitor leads
- `api.marketing`: access to the `/jsonapi/contact-marketing` endpoints to update marketing consent
- `api.contacts`: access to the `/jsonapi/contacts` endpoints to manage contacts
- `api.contacts.read`: access to read contact details via the GET `/jsonapi/contacts/{id}` endpoint

You will only be able to access an endpoint if you request an applicable scope when authenticating, regardless of the scopes available to you.

## Authentication

The MyCRM API uses the [OAuth 2.0 client credentials](https://datatracker.ietf.org/doc/html/rfc6749#section-4.4) flow. This is the standard OAuth flow for server-to-server integrations where there is no user login — your application authenticates directly using its own credentials.

**Step 1 — Request a token**

Send a `POST` request to your `Auth_Url` to exchange your credentials for an access token:

```http
POST {Auth_Url}
Content-Type: application/x-www-form-urlencoded

grant_type=client_credentials&client_id={ClientID}&client_secret={ClientSecret}&scope={Scopes}
```

The response will contain a JSON body including an `access_token` and the number of seconds until it expires (`expires_in`).

**Step 2 — Use the token**

Include the token in every API request as a Bearer token in the `Authorization` header:

```http
Authorization: Bearer {access_token}
```

Tokens expire after the period indicated by `expires_in`.

**NOTE:** You should cache and reuse the token until it nears expiry, then request a new one. The Postman collection and the sample client handle this automatically.

## Data access

API credentials are not issued for a single user — at a minimum they are issued for an organisation. Also, records shared with an organisation or user but not owned by them will not be accessible via the API. Be aware that the MyCRM application does allow a user to access records shared with them and this may lead to differences in what is available via the API.

Each request should identify which adviser it is acting on behalf of via the `UserId` header. The value is the `AdviserContactId` supplied with your credentials, displayed in the API Settings page within MyCRM or from the `adviser-details` endpoint.

- During write operations (`POST`, `PATCH`, `DELETE`) the `UserId` is recorded as the creator or last modified user of the record. For example, a new lead will be allocated to that adviser. As such, this header is required and a failure to include it will result in a `403 Forbidden` error.
- In most `read` operations the `UserId` header has no impact.
- In some `search` operations including the `UserId` header may limit responses to only records owned by the nominated user. If the `UserId` header is necessary for a `search` endpoint, an error will be returned indicating that the  `UserId` header is missing.

## Required request headers

| Header          | Value                      | Purpose                                                          |
|-----------------|----------------------------|------------------------------------------------------------------|
| `Authorization` | `Bearer {access_token}`    | Authenticates the request (see[Authentication](#authentication)) |
| `UserId`        | `{AdviserContactId}`       | Identifies which adviser the request is acting on behalf of      |
| `Accept`        | `application/vnd.api+json` | Tells the server you expect a JSON:API response                  |
| `Content-Type`  | `application/vnd.api+json` | Required on write requests (`POST`, `PATCH`)                     |

## Reviewing the API

You can review our [API specification online](https://api-docs.mycrm.io/) or the full OpenAPI specification is available in [MyCrmSampleClient/swagger.json](MyCrmSampleClient/swagger.json).

The API conforms to the [JSON:API](https://jsonapi.org/) standard, described below.

## JSON:API overview

[JSON:API](https://jsonapi.org/) is a standardised convention for structuring REST API responses. All MyCRM API responses follow this format, so understanding it is essential.

### Response structure

Every response wraps its payload in a `data` field. Each resource object has four key parts:

| Field           | Purpose                                                                                                           |
|-----------------|-------------------------------------------------------------------------------------------------------------------|
| `type`          | The resource type (e.g.`"contacts"`, `"deals"`)                                                                   |
| `id`            | The unique identifier for the resource                                                                            |
| `attributes`    | The resource's fields — equivalent to columns in a database table                                                 |
| `relationships` | Links to related resources, expressed as `{ "type": "...", "id": "..." }` references rather than embedded objects |

When related resources are included in the response (via the `include` query parameter), they appear in a top-level `included` array. The `relationships` section contains references (`type` + `id`) that you use to look them up in `included`. This normalisation means each resource appears exactly once even if it is referenced by multiple other resources.

For example, a single-resource response looks like:

```json
{
  "data": {
    "type": "contacts",
    "id": "8503844",
    "attributes": {
      "firstName": "Evangelia",
      "lastName": "Rodriguez",
      "email": "ev@example.com"
    },
    "relationships": {
      "contactAddress": {
        "data": [
          { "type": "contact-address", "id": "3189832" }
        ]
      }
    }
  },
  "included": [
    {
      "type": "contact-address",
      "id": "3189832",
      "attributes": {
        "addressType": "PostalAddress",
        "formattedAddress": "PO Box 121, Athol Park SA 5012, Australia"
      }
    }
  ]
}
```

A collection response wraps multiple resources as an array in `data`, and may include pagination links in a top-level `links` object.

For further reading see the [JSON:API specification](https://jsonapi.org/format/).

## Advanced queries

The following query features are available on most endpoints:

| Feature         | Applies to          | Description                                             |
|-----------------|---------------------|---------------------------------------------------------|
| Sorting         | `search`            | Control the order of results                            |
| Pagination      | `search`            | Page through large result sets                          |
| Filtering       | `search`            | Narrow results by field criteria                        |
| Includes        | `read` and `search` | Fetch related resources in the same request             |
| Field selection | `read` and `search` | Limit which fields are returned to reduce response size |

### Sorting

The following sort rules may be applied to a `search` query, however not all fields support sorting.

| Operation  | Operator | Example                   |
|------------|----------|---------------------------|
| Ascending  | *none*   | `?sort=id`                |
| Descending | **-**    | `?sort=-id`               |
| Multiple   | **,**    | `?sort=lastName,-updated` |

The default sort order is ascending by `id`. Only limited fields can be used to sort records.

### Pagination

The following pagination options are available on `search` queries.

| Operation   | Parameter      | Example           |
|-------------|----------------|-------------------|
| Page Size   | `page[size]`   | `?page[size]=50`  |
| Page Number | `page[number]` | `?page[number]=2` |

The maximum and default page size is `100` and the minimum and default page number is `1`.

### Filtering

The following filters may be applied to a `search` query, however not all fields support filtering.

| Operation                 | Function         | Example                                                                             |
|---------------------------|------------------|-------------------------------------------------------------------------------------|
| Equality                  | `equals`         | `?filter=equals(hasMarketingConsent,'true')`                                        |
| Less than                 | `lessThan`       | `?filter=lessThan(value,'25000')`                                                   |
| Less than or equal to     | `lessOrEqual`    | `?filter=lessOrEqual(created,'2020-12-31')`                                         |
| Greater than              | `greaterThan`    | `?filter=greaterThan(value,'25000')`                                                |
| Greater than or equal to  | `greaterOrEqual` | `?filter=greaterOrEqual(updated,'2021-07-01')`                                      |
| Contains text             | `contains`       | `?filter=contains(email,'acme.com')`                                                |
| Starts with text          | `startsWith`     | `?filter=startsWith(firstName,'Rob')`                                               |
| Ends with text            | `endsWith`       | `?filter=endsWith(lastName,'Smythe')`                                               |
| Equals one value from set | `any`            | `?filter=any(firstName,'Kate','Joan','Robert')`                                     |
| A relationship exists     | `has`            | `?filter=has(contactAddress)`                                                       |
| Negation                  | `not`            | `?filter=not(equals(firstName,'Mary'))`                                             |
| One criterion must apply  | `or`             | `?filter=or(equals(firstName,'Kate'),equals(preferredName,'Kate'))`                 |
| All criteria must apply   | `and`            | `?filter=and(equals(hasMarketingConsent,'true'),greaterThan(updated,'2021-07-01'))` |

Regardless of data type the filter value should be enclosed in single quotes. Dates should be in `yyyy-MM-dd` format.

### Includes

You can include related resources in the response using the `include` query parameter. Using the following query as an example:

`/jsonapi/contact-groups/7117726?include=contacts,contacts.contactAddress`

Both the contacts and their addresses will be returned in a single response. The primary resource appears in `data`, while related resources are normalised into the `included` array and linked by `type` + `id` references in the `relationships` section. The following response is shortened for brevity.

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

By default all fields are returned, but you can restrict them to improve performance. Field selection applies to both the `attributes` and `relationships` sections of the response.

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

- Including `contactAddress` in the fields for `contacts` ensures the relationship is present in the response. If omitted, the addresses would appear in `included` but with no link back to the contact. This is not strictly necessary for `read` requests but is required for `search`.
- The field selection parameter uses the entity type (`contact-address`) not the relationship name (`contactAddress`). If a contact had addresses linked via multiple relationships, the field selection would apply to all of them.

## Creating and updating resources

Write operations follow the same JSON:API structure as read responses. The request body must always include a `data` object with a `type` and, for updates, an `id`.

### Creating a resource (POST)

To create a new resource send a `POST` to the collection endpoint:

```http
POST /jsonapi/leads
Content-Type: application/vnd.api+json
Authorization: Bearer {access_token}
UserId: {AdviserContactId}

{
  "data": {
    "type": "leads",
    "attributes": {
      "firstName": "Jane",
      "lastName": "Smith",
      "email": "jane.smith@example.com",
      "phone": "+61400000000"
    }
  }
}
```

The response will contain the created resource with its server-assigned `id` and all populated attributes.

> **403 Forbidden?** A `403` response on a `POST` request is most commonly caused by a missing or invalid `UserId` header. Ensure the `AdviserContactId` value you have been given is set as the `UserId` header on every write request. See [Required request headers](#required-request-headers) and [Data access](#data-access).

To create a resource that belongs to another resource (e.g. a note on a deal), include a `relationships` section:

```json
{
  "data": {
    "type": "deal-notes",
    "attributes": {
      "title": "Follow-up required",
      "description": "Called client, left voicemail."
    },
    "relationships": {
      "deal": {
        "data": { "type": "deals", "id": "4567890" }
      }
    }
  }
}
```

### Updating a resource (PATCH)

To partially update a resource send a `PATCH` to the resource's endpoint. Include only the attributes you want to change — all other attributes remain untouched:

```http
PATCH /jsonapi/contact-marketing/8503844
Content-Type: application/vnd.api+json
Authorization: Bearer {access_token}
UserId: {AdviserContactId}

{
  "data": {
    "type": "contact-marketing",
    "id": "8503844",
    "attributes": {
      "hasMarketingConsent": false
    }
  }
}
```

A successful update returns `200 OK` with the updated resource, or `204 No Content` if no body is returned.

## Real-time Updates via Webhooks

[Webhooks](https://en.wikipedia.org/wiki/Webhook) are a common method of loose integration between web accessible systems. MyCRM supports webhooks for some scenarios, including organisation and adviser changes. Once configured, MyCRM will `POST` to your web endpoint sending a JSON body similar to:

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

Requests to the API are subject to rate limiting to ensure fair use. Once your requests hit the limit, the API will respond with `429 Too Many Requests` until you earn enough tokens to make another request. MyCRM applies a token bucket algorithm with a variable cost per request, where the cost is an estimation of the underlying capacity reserved to respond to the request.

The following response headers relating to rate limiting will be returned for each request:

| Header                  | Purpose                                                                                                                               |
|-------------------------|---------------------------------------------------------------------------------------------------------------------------------------|
| `retry-after`           | The minimum period to wait before making a follow-up request. Depending on the cost of the request it may be necessary to wait longer |
| `x-ratelimit-cost`      | The tokens required to perform the request                                                                                            |
| `x-ratelimit-limit`     | The total tokens earned over a 5 minute period                                                                                        |
| `x-ratelimit-remaining` | The number of tokens currently remaining                                                                                              |

LMG reserves the right to update the rate limiting rules to protect the system. The following factors affect the rate limiting cost calculation.

#### Time of Day

Higher limits are available outside of Business Hours. Due to the breadth of operations, Business Hours is defined as 08:00 to 18:00 AEST (Brisbane, Australia) time.

**Recommended Action:** Perform operations out of hours where possible.

#### Type of Record

As a general guide, the more complex the record type the more expensive the request. For example, querying `deals` is more expensive than querying `referrers`.

**Recommended Action:** Review your requests relating to complex records, particularly any search endpoint.

#### Type of Request

The following table approximates the base cost by type of request

| Request Type        | Example Endpoint                                     | Cost  |
|---------------------|------------------------------------------------------|-------|
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
