# MyCRM Sample Client

This is an interactive .NET console application that lets you run live API calls against MyCRM. Its purpose is to help developers understand how to authenticate, query, filter and write data through the MyCRM API from a real C# application.

> **Before running**: you will need credentials and configuration values from Loan Market Group. See the [top-level README](../README.md) for how to obtain access, understand scopes, and configure `appsettings.json`.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)

## Running the application

```bash
cd MyCrmSampleClient
dotnet run
```

You will be presented with an interactive menu. Use the arrow keys to select a sample and press Enter to run it. Each sample calls the MyCRM API and prints a formatted table of results.

## How it works

### The Kiota-generated client

The `Generated/` folder contains a strongly-typed C# API client that was generated from the MyCRM OpenAPI specification (`swagger.json`) using [Microsoft Kiota](https://github.com/microsoft/kiota). You do not need to write any HTTP code — every endpoint is available as a method on the `MyCrmApiClient` class.

For example, to fetch contacts the generated client exposes:

```csharp
var result = await client.Jsonapi.Contacts.GetAsync();
```

### The fluent query builder

Because the Kiota-generated search methods accept raw query strings, a thin fluent wrapper in `KiotaExtensions/` makes common JSON:API query parameters easier to compose in C#:

```csharp
var result = await client.Jsonapi.Contacts
    .Sort("updated", SortOrder.Descending)
    .Filter("and(equals(hasMarketingConsent,'true'),greaterThan(updated,'2024-01-01'))")
    .Page(1, 10)
    .Include("contactAddress")
    .Fields("contacts", "firstName", "lastName", "email")
    .GetAsync();
```

Each method returns the same builder, so calls can be chained. `GetAsync()` builds the final URL and executes the request. This pattern mirrors how you would construct queries in your own application.

### Shared state

Some samples depend on an ID returned by a previous sample. For example, the `contact-with-address` sample needs a contact ID to look up. Rather than hardcoding values, the client saves certain IDs to a local state file after each run:

| State value          | Set by                                        | Used by                                           |
|----------------------|-----------------------------------------------|---------------------------------------------------|
| `LastContactId`      | `latest-contacts`, `filtered-latest-contacts` | `contact-with-address`, `patch-contact-marketing` |
| `LastContactGroupId` | `latest-contact-groups`                       | `contact-group-contacts`, `contact-group-emails`  |
| `LastDealId`         | `latest-deals`, `create-lead`                 | `create-update-deal-note`                         |

The state file is stored in your local app data directory (e.g. `~/.local/share/MyCrmSampleClient/sample-state.json` on Linux/macOS). You can view or clear the state from the menu.

**Recommended order when running samples for the first time:** run `latest-contacts`, `latest-contact-groups`, and `latest-deals` first to populate the state, then run the samples that depend on them.

---

## The samples

### `advisers` — Listing advisers with sorting and pagination

**API call:** `GET /jsonapi/advisers?page[size]=10&sort=-id`

Fetches the 10 most recently created advisers and prints their email address and job title.

**What this demonstrates:**

- How to sort results in descending order (`sort=-id` means newest first)
- How to control page size to limit the number of records returned
- The basic structure of a JSON:API collection response: a `data` array of typed resource objects, each with `id`, `type`, and `attributes`

---

### `adviser-details` — Listing adviser profile details

**API call:** `GET /jsonapi/adviser-details?page[size]=10&sort=-id`

Fetches the 10 most recently updated adviser detail records, showing names, email and mobile.

**What this demonstrates:**

- That some entities have a separate "details" resource (e.g. `adviser-details` vs `advisers`). This separation
- means you only fetch the richer data when you need it.
- How to identify the correct `AdviserContactId` to use in write operations — run this sample and note the `Id` column value for the adviser you want to act as.

---

### `latest-contacts` — Listing contacts

**API call:** `GET /jsonapi/contacts?page[size]=10&sort=-id`

Fetches the 10 most recently created contacts and stores the most recent contact ID in shared state for use by later samples.

**What this demonstrates:**

- The fundamental pattern for reading a list of resources
- How the sample state system works — running this first unlocks the `contact-with-address` and `patch-contact-marketing` samples

---

### `contact-with-address` — Fetching a related resource using includes

**API call:** `GET /jsonapi/contacts/{id}?include=contactAddress`

Fetches a single contact and, in the same request, includes their address records.

**What this demonstrates:**

- The JSON:API `include` feature: instead of making two separate API calls (one for the contact, one for addresses), you ask the API to return related resources in a single response
- How the response is structured when includes are used: the primary resource (`contact`) appears in `data`, while the related resources (`contactAddress`) appear in the `included` array, linked by ID references in the `relationships` section
- How to resolve those references in C# by matching on `Id` and `Type`

This is one of the most important patterns to understand. Includes let you reduce round-trips, but they also increase the cost of the request — see the [rate limiting guidance](../README.md#rate-limiting).

---

### `filtered-latest-contacts` — Filtering a search

**API call:** `GET /jsonapi/contacts?sort=-updated&filter=and(equals(hasMarketingConsent,'true'),greaterThan(updated,'<today-30d>'))`

Fetches contacts that have marketing consent and were updated in the last 30 days, sorted by most recently updated.

**What this demonstrates:**

- How to build compound filters using `and(...)` with multiple criteria
- Combining `equals(field,'value')` with `greaterThan(field,'date')` to narrow results
- Sorting by a field other than `id` (note: this incurs a higher rate limiting cost, see [rate limiting](../README.md#rate-limiting))
- A realistic use case: finding contacts that are eligible for a marketing campaign

---

### `latest-contact-groups` — Includes on a collection search

**API call:** `GET /jsonapi/contact-groups?sort=-id&include=contacts`

Fetches recent contact groups and includes the contacts in each group in the same response.

**What this demonstrates:**

- That includes can be used on collection (`search`) responses, not just single-resource (`read`) responses
- How to iterate over a group's `relationships.contacts.data` array and resolve each contact from the `included` section — the same approach used in any JSON:API many-to-many relationship
- A contact group represents a household or borrower group in MyCRM; contacts within the group can be primary or secondary applicants

---

### `contact-group-contacts` — Fetching related resources via a sub-path

**API call:** `GET /jsonapi/contact-groups/{id}/contacts`

Fetches the contacts belonging to a specific contact group using a relationship sub-path.

**What this demonstrates:**

- An alternative to includes: instead of fetching the group and including contacts in one call, you navigate directly to the related resource endpoint
- When to prefer this over includes: use this approach when you only need the related resources and do not need the parent resource's attributes, or when the parent is already cached
- The difference in rate limiting cost between `Fetch Related` and `Fetch Relationships` — see [rate limiting](../README.md#rate-limiting)

---

### `contact-group-emails` — Restricting fields returned

**API call:** `GET /jsonapi/contact-groups/{id}/contacts?fields[contacts]=email`

Fetches the contacts in a group but only returns the `email` field for each contact.

**What this demonstrates:**

- The JSON:API sparse fieldsets (field selection) feature: `fields[<type>]=<comma-separated-fields>` limits which attributes are returned
- Why you would use this: when you only need one or two fields from a large resource, sending and parsing a full response is wasteful. For bulk operations over many records this can significantly improve performance.
- The syntax difference between the resource type (`contacts`) used in the `fields` parameter and the relationship name (`contacts`) used in the URL path — they happen to match here but may differ for other relationships

---

### `patch-contact-marketing` — Updating a resource with PATCH

**API call:** `PATCH /jsonapi/contact-marketing/{id}`

Updates the marketing consent flag for a contact to `false`.

**What this demonstrates:**

- How to perform a partial update (PATCH) in JSON:API: you send only the `type`, `id`, and the `attributes` you want to change — other attributes are left untouched
- The `contact-marketing` resource type, which is a focused write endpoint for updating marketing preferences without touching other contact data
- The `AdviserContactId` requirement for write operations: the nominated adviser will be recorded as the last modifier of the record
- How a 403 Forbidden response is handled — this typically means the `AdviserContactId` does not have permission to modify the record, and the sample explains how to resolve it

---

### `latest-deals` — Listing deals

**API call:** `GET /jsonapi/deals?sort=-id`

Fetches the most recently created deals and stores the latest deal ID in shared state.

**What this demonstrates:**

- Querying the `deals` resource, which is one of the more complex (and rate-limit-expensive) resource types
- The basic attributes of a deal: name, status, lender name, and total loan amount
- Populating the shared state so the `create-update-deal-note` sample can run

---

### `create-lead` — Creating a new record with POST

**API call:** `POST /jsonapi/leads`

Creates a new lead with a full set of attributes including address, UTM tracking fields, and an external reference ID.

**What this demonstrates:**

- How to construct a JSON:API `POST` request body: a `data` object with `type` and `attributes`
- A lead is a lightweight entry point for creating a new client record in MyCRM; internally it creates a deal and associated contacts
- UTM fields (`utmSource`, `utmMedium`, `utmCampaign`) for tracking the marketing source of the lead
- The `externalReference` and `externalIntegration` fields for linking MyCRM records back to records in an external system — useful for reconciliation and avoiding duplicate creation
- The `AdviserContactId` determines which adviser the lead is allocated to

---

### `create-update-deal-note` — Creating then updating a record

**API call:** `POST then PATCH /jsonapi/deal-notes`

Creates a note on an existing deal, then immediately updates its title and body.

**What this demonstrates:**

- How to create a resource that has a **relationship** to another resource: the `deal-notes` POST body includes a `relationships.deal` section pointing to the parent deal by `type` and `id`
- The full create-then-update lifecycle: POST returns the newly created resource with its server-assigned ID, which is then used immediately in a PATCH
- How to structure a PATCH body: include `type`, `id`, and only the attributes to change
- A practical pattern for integrations that create a record and then want to annotate or correct it without fetching it again

---

## Adding your own samples

Each sample is a method on the `Samples` class decorated with a `[Sample]` attribute:

```csharp
[Sample("my-sample", "GET /jsonapi/my-resource")]
public async Task RunMySample()
{
    // use _console.Client to make API calls
}
```

The `[Sample("name", "description")]` attribute registers the method with the interactive menu automatically — no other wiring is required. The `name` is shown in the menu and must be unique. The description is logged when the sample runs to help correlate log output with the API call.

Use `_console.RequireScope("api.my-resource.search", "my-sample")` at the start of your method to skip gracefully if the required OAuth scope is not present in the configured credentials.
