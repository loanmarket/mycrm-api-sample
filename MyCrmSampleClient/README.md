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

Press **Esc** to return one menu level. Within a form, Esc abandons the unfinished action and returns to its menu. At the main menu, Esc (or selecting **Exit**) asks for confirmation: **Yes** or Enter exits; **No** or Esc keeps the application open. Completed API actions are not undone when returning to a menu.

The deal edit samples prompt for values to send and apply them to the deal in shared state. These are live writes. `MyCRM:Url` should be the API host URL, without `/jsonapi`; the generated client adds that path.

Authentication is renewed before a request when the token is within one minute of expiry, including after waiting at an import confirmation prompt. Failed API calls report the HTTP method, exact URL and status without logging authentication tokens or financial request bodies. An HTTP 401 means authentication was rejected; it does not mean a snapshot was applied. Snapshot submissions are never automatically replayed after a failure.

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
| `LastDealId`         | `latest-deals`, `create-lead`                 | `create-update-deal-note`, `patch-deal-name`, `patch-deal-rationale`, `deal-custom-status`, `change-deal-status`, `create-deal-structure` |

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
- Populating the shared state for deal notes, deal edits and status samples

---

### Deal edits and custom statuses

See the [deal guide](../docs/deals.md) for payloads and movement rules; implementations are in [DealSamples.cs](DealSamples.cs) and [DealStructureSamples.cs](DealStructureSamples.cs).

Run `latest-deals` or `create-lead` first, then check `LastDealId` in the menu. Deal edits and structure creation use that deal. BID notes and structure writes require pre-submission; name-only edits are allowed at any status. Structure PATCH prompts for its own structure ID.

| Sample | Action |
|---|---|
| `patch-deal-name` | PATCH a new name. |
| `patch-deal-rationale` | Discover the adviser's country and PATCH AU or NZ BID notes. |
| `custom-statuses` | List accessible statuses sorted by `sortOrder`. |
| `deal-custom-status` | Read the deal's current status. |
| `organisation-custom-statuses` | Prompt for an organisation ID and list its statuses in pipeline order. |
| `create-deal-structure` | Create a structure on the selected deal, then read it back. |
| `patch-deal-structure` | Update a structure's repayment amount, then read it back. |
| `change-deal-status` | PUT a target custom status ID and display the result or error details. |

BID notes are write-only. Status IDs must belong to the deal's organisation. `Run all samples` includes these prompts.

Structure samples require `api.deal-structures.read` plus `api.deal-structures.create` or `api.deal-structures.update` (or broader applicable scopes). The deal's adviser and custom status are read directly through their related-resource endpoints.

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

### Financial snapshots — Excel import, export and fabricated examples

Select **`financial-snapshots`** in the main menu to open its own section. **Run all samples** skips this section; select it directly to use these options:

| Option | What it does |
|---|---|
| **Import spreadsheet** | Reads an `.xlsx` into the request, previews collection counts and modes, optionally displays the complete request, then asks before submitting it to the nominated group for server validation and application. |
| **Export current financials** | Reads the group's contacts, assets, liabilities, incomes, expenses, employments and linked addresses into the same workbook format. |
| **Generate fabricated spreadsheet** | Uses Bogus to randomise financial details and a selection of asset, income, expense and liability types for up to two adults. Real contact and lookup IDs are retained. It only writes a local file. |

Example generation uses [Bogus](https://github.com/bchavez/Bogus) with a local random seed. Press Enter at the seed prompt for a fresh example, or reuse a seed to repeat it with the same contacts, controls and generator/library versions. The workbook records the seed in `Snapshot` and `Read me`. Employer names, job titles, addresses, accounts, balances and repayments vary; contact names and IDs remain the real group contacts. Addresses use fabricated streets with matching AU/NZ suburb, state and postcode combinations.

Every example includes a linked investment property, savings account, employment/salary for each adult, rental income, mortgage and groceries. Available lookup data then supplies a random selection of 2–4 additional assets, 1–3 incomes, 1–3 liabilities and 3–6 expenses (fewer if compatible types are unavailable):

- Assets include deposits, boats, vehicles, bonds, cash, contents, funds, shares and superannuation, plus KiwiSaver, art and jewellery for NZ.
- Income includes dividends, interest, other gross income, bonuses, allowances, commission, overtime and non-recurring income, plus foreign and other net income for NZ.
- Liabilities include credit cards, student loans, overdrafts, personal/car loans and buy now pay later.
- Expenses include ordinary household, transport, insurance, health, recreation and utility costs. Repayments are generated on liabilities.

Generation selects supported types by their API IDs from the loaded control data. It stops with an explanation if a core type is unavailable. Real estate has a blank description and a property purpose, property type and address; deposit assets have descriptions and account details. Vehicles require a known compatible subtype, make and year. Credit cards require a subtype belonging to that type. Employment income belongs solely to the linked employment's contact; rental income and the mortgage link to the investment property. NZ student loans have one owner. AU examples use BSB and loan term in years; NZ examples use the NZ loan date and documented term in months. Unknown types and incompatible subtypes are omitted from random selection. These generation rules do not add validation or change values during import; export dropdowns are unchanged.

The implementation starts in [FinancialSnapshotSamples.cs](FinancialSnapshotSamples.cs); the workbook contract, reads, validation and example factory are in [FinancialSnapshots/](FinancialSnapshots/).

#### Access and first run

The write endpoint is `POST /jsonapi/contact-groups/{contactGroupId}/financial-snapshot`. It is a beta feature that must be enabled for the resolved adviser. Configure `MyCRM:AdviserContactId` so the existing authentication provider sends `UserId`.

Import requires `api.contactgroups.financialsnapshot` (or `api.contactgroups`, `api.financialsnapshot` or `api`). It reads the workbook, constructs the request and posts it to the nominated group without fetching contacts or control data. Ordinary contact-group update or individual financial write scopes do not authorize the snapshot action. The snapshot scope spelling has **no hyphen** in `contactgroups`.

Export uses `api.contact-groups.read`, `api.contacts.read` and search scopes for `assets`, `liabilities`, `incomes` and `expenses`. Example generation reads the group's contacts and adviser to determine AU/NZ country. Export and generation also search the control-data endpoints: asset/liability/income/expense categories and types, asset subtypes, property types, income verification types and rental verification types. Liability subtypes are included through `liability-types`. Approved broader read/search scopes can satisfy these checks. Import does not fetch control data; the snapshot service validates submitted control IDs.

1. Run `latest-contact-groups` to find a group, or enter a known group ID when prompted.
2. Export the current financials to edit existing records, or generate a fabricated example for a test group.
3. Edit the workbook in Excel, save it, then select **Import spreadsheet**. The contact group prompt defaults to the workbook's group; press Enter to use it. You may explicitly enter a different group, but the API still requires all submitted contact IDs to belong to that target group.
4. Choose the modes, review the counts and request, and confirm the live write. The result lists created, updated and deactivated resources with their IDs and any correlated `lids`.
5. Export again to capture the saved IDs for later deliberate updates. The snapshot response does not contain complete saved resources or a retrievable historical snapshot.

Output paths must end in `.xlsx`. For both exports and generated examples, saving to an existing file asks whether to overwrite it; press Enter for Yes, or choose No to keep the existing file. The replacement workbook is fully written before the existing file is replaced. No Excel installation is required; the application uses ClosedXML.

#### Workbook format (version 1)

The current workbook format remains version 1 and includes all features described below. Only the current template is supported; compatibility with older layouts will be added when requested.

All data sheets are required, even when empty. Row 1 contains exact, case-sensitive column names; columns may be reordered. Keep the complete set of headers so a misspelt or removed column cannot silently clear data. Blank rows are ignored, and formulas/error cells are rejected in input sheets: paste values instead.

Import only checks workbook structure and whether cell values can be represented by the generated request models: integers, finite numbers, booleans, dates and known enum values. It preserves entered text (including whitespace), numeric values, duplicate rows/IDs/owners and unresolved references. It does not enforce required fields, ownership eligibility, value ranges, unique IDs or local-link integrity. The snapshot service validates the submitted request and returns any errors. The explicitly nominated group determines the request URL; the workbook's group ID is displayed as a reference.

New workbooks put `lid` first, then the record ID, type ID and subtype ID where present. Other IDs and links follow alphabetically, then the remaining fields alphabetically. For example, Assets starts with `lid`, `assetId`, `assetTypeId`, `assetSubTypeId`, `address.lid`, `owner1ContactId`, `owner2ContactId`, `propertyTypeId`, then `accountName`, `accountNumber`, and the other fields.

| Sheet | Contents |
|---|---|
| `Snapshot` | `setting`/`value` rows for `formatVersion` (`1`), `contactGroupId`, `source`, and `assetsMode`, `expensesMode`, `incomesMode`, `liabilitiesMode`, `employmentsMode`. |
| `Assets` | All supported asset input fields, including `assetId`, `assetTypeId`, property, account and vehicle fields. |
| `Expenses` | Ordinary expenses, with `expenseId`, `expenseTypeId`, amount and frequency. |
| `Incomes` | Income fields, `incomeId`, gross/net and verification fields, plus asset/employment links. |
| `Liabilities` | Liability fields, `liabilityId`, balance, limit, repayment, frequency, interest rate and AU/NZ loan fields. |
| `Employments` | `employmentId`, `contactId`, employer details, employment enums, dates and address links. |
| `Addresses` | `lid`, street, suburb, state, postcode, country and optional formatted address. Every address must be referenced. |
| `Contact Lookups` | Reference-only `contactId`, `Name`, `Gender` and `isPrimary` values for adults. Names combine the contact's first, middle and last names. This sheet is not submitted on import. |
| `Lookups` | Reference control data loaded for examples and exports: `resource`, `id`, `name`, `parentResource` and `parentId`. Categories supply type descriptions and parents link liability subtypes to their types. This sheet is not submitted. |
| `Dropdown lists` (hidden) | Named ranges used by Excel data validation, including one subtype list per liability type. |
| `Excluded records`, `Read me` | Reference-only exclusions and usage/export notes. |

`Contact Lookups` includes only contacts with the `Adult` role, using contacts already returned by the group's contacts endpoint. Empty fields remain blank. Editing this reference sheet does not update contact details or change which contacts the API permits for ownership and employment.

Columns follow the generated **snapshot input** models, with these flattening rules:

- `owner1ContactId` and `owner2ContactId` each select one active Adult contact in the nominated group. Choose one or two distinct contacts; either column may be blank for a single owner. Dropdowns show `contactId — Name`, and import converts the selected IDs into the snapshot's `ownership` array. Names are display hints, while IDs identify the records. Numeric contact IDs may also be entered. Export stops if a record has more than two owners rather than dropping ownership.
- `assetId`, `expenseId`, `incomeId`, `liabilityId` and `employmentId` explicitly select existing records. Export fills them in. Leave them blank to use server matching; `lid` alone does not identify a previously saved record.
- `linkedAsset.id` or `linkedAsset.lid` links an income/liability to an existing or submitted asset. Supply exactly one. `employment.id`/`employment.lid` works the same way for income. A link by existing ID does not itself update that target.
- `address.lid` links an asset/employment to a row in `Addresses`. Local identifiers are case-sensitive and unique within their collection. Example links use names such as `investment-property`, `job-1` and `investment-address`.
- Store account numbers, BSBs, postcodes and business/company numbers as **text** to preserve leading zeroes. Amounts are numbers, flags are `TRUE`/`FALSE`, and dates are Excel dates or `YYYY-MM-DD`. Use enum dropdown values such as `Monthly` and `PAYG`.
- Amounts use the group's local currency. `interestRate = 6.25` means **6.25%**. `loanTerm` uses years; NZ documented and interest-only term fields use months. Keep frequency and gross/net meaning with each amount.

#### Control-data dropdowns and validation

Every enum column has a dropdown populated from its generated snapshot model, including frequencies, valuation basis, property purpose, rental type, mortgage priority, loan repayment type, and employment fields. All dropdowns (control data, contacts, record links, enums and booleans) cover rows 2–1000, including empty sheets, and extend further if the initial export contains more rows. Excel rejects unlisted enum values with a **Stop** message; optional fields may remain blank. Import also rejects invalid enum values pasted into the workbook. Input formats use column defaults so blank input rows do not inflate the workbook's used range.

`address.lid`, `linkedAsset.lid` and `employment.lid` offer dropdowns from the matching sheet's `lid` column. These lists follow the Excel tables: add or insert rows within the target table to extend them. Set the target row's `lid` before selecting it in another sheet. Changing a target's `lid` updates the choices but does not rewrite existing links; update those links too. Import passes the entered references through unchanged; the snapshot service validates missing or misspelt links.

`linkedAsset.id` and `employment.id` offer existing IDs from the corresponding workbook tables. Excel shows a warning, which you may accept, when typing an ID outside that list; Merge can link an existing API record that is absent from the workbook. Replace still requires retaining the linked target in the submitted collection. Employment `contactId` and both owner columns offer the same Adult contact choices with readable names. Import extracts the entered contact IDs and leaves ownership and group-access validation to the snapshot service.

Generation and export load `/jsonapi/liability-categories` and the other financial control endpoints, following pagination. Type/category relationships are requested with `include=liabilityCategory,liabilitySubTypes`, `include=incomeCategory` and `include=expenseCategory`. Category labels are read from `description`; other control labels use `name`.

These input columns offer readable choices such as `21 — Secured / Mortgage` (illustrative IDs): `assetTypeId`, `assetSubTypeId`, `expenseTypeId`, `incomeTypeId`, `liabilityTypeId`, `liabilitySubTypeId`, `propertyTypeId`, `incomeVerificationId` and `nzRentalVerificationTypeId`. Import converts the selection back to a numeric ID. The snapshot has no category-ID input columns, so categories label the applicable type choices instead of adding fields to the request. Asset type/category and asset subtype/type associations are not exposed by this Swagger; their domain compatibility remains a server check.

The liability subtype dropdown depends on the selected liability type. A type with no available subtypes offers an empty list. If you change a type, clear or reselect its subtype; the snapshot service validates type/subtype compatibility. Optional controls can remain blank. Known liability repayment expenses and unsupported addback/retired income types are excluded from dropdown suggestions; explicitly entered numeric IDs are still submitted for server validation.

The lists use named worksheet ranges, so commas and long lists do not run into Excel's 255-character inline-list limit. Control dropdowns cover every data row, including new rows added below the initial example. Excel displays a **Warning** for values outside a control list, which you can accept to enter a numeric ID. Import extracts the numeric prefix from dropdown labels and passes integer IDs through unchanged, even when invalid or absent from the lookup list. Reference sheets are not read during import. It does not reload controls or check their membership, availability or type/subtype compatibility locally. The snapshot service validates these values and returns any errors. Regenerate or export a fresh workbook to refresh the dropdown choices.

#### Merge and Replace behaviour

Each collection has an independent mode on `Snapshot`; omitted mode settings default to Merge. The import menu can retain those modes or set every collection to Merge or Replace.

**Merge preserves omitted records, not omitted fields on submitted records.** Matched rows are complete updates: blank optional cells can clear existing values and links. Ownership is replaced and divided equally among submitted contacts. Export ignores `ownershipPercentage` and treats all referenced owners as sharing equally.

**Replace applies to the entire contact group.** An empty sheet in Replace mode deactivates all eligible records in that collection. Asset removal can also deactivate linked incomes and liabilities and affect deal securities. An existing ID link is valid if its target survives the update, including when a submitted target is matched automatically. The API validates the final state after matching; the sample does not require an explicit ID on the submitted target solely because the collection uses Replace.

Standard expenses of one type can consolidate, and duplicate existing expenses of that type can be deactivated even in Merge mode. Enter liability repayments using `repayment` and `repaymentFrequency` on `Liabilities`, not as direct expense rows. Export puts known liability-managed expense types on `Excluded records`; their repayment amounts come from liabilities. Unsupported addback/retired income types are also excluded and noted. The server remains authoritative for type availability, country rules, matching, access, currency and complete-state validation.

#### How export reads the profile

Export fetches **all contacts in the nominated group**, then applies this filter to each financial collection:

```text
has(ownership,any(contact.id,'1001','1002'))
```

```http
GET /jsonapi/assets?filter=has(ownership,any(contact.id,'1001','1002'))&include=ownership.contact,addresses
GET /jsonapi/liabilities?filter=has(ownership,any(contact.id,'1001','1002'))&include=ownership.contact,linkedAsset
GET /jsonapi/incomes?filter=has(ownership,any(contact.id,'1001','1002'))&include=ownership.contact,linkedAsset,employment
GET /jsonapi/expenses?filter=has(ownership,any(contact.id,'1001','1002'))&include=ownership.contact
GET /jsonapi/contacts/1001/employments?include=address,contact
```

The sample URL-encodes filters, follows `links.next` for every collection/related read, and deduplicates records by `(type, id)` so joint ownership does not duplicate a record. An empty group never triggers an unfiltered financial search. Related resources are resolved by `(type, id)`, and missing required relationships stop export instead of silently losing them. Assets with multiple addresses cannot be represented by the snapshot's single-address input and stop export with an explanation.

Export treats a standalone `NULL` string in financial record fields as an absent value and writes a blank cell. Text containing other words is preserved. Contact, control-data, enum and record-link selections use single-line cells that shrink to fit, so wrapped labels do not hide the ID or name.

These are sequential live reads, not a transactionally consistent historical snapshot. Review the current state before importing. Read-only fields outside the snapshot contract are not editable columns.

#### Errors and retries

Workbook parsing errors identify the sheet/cell or row where possible. The importer checks only that it can read the workbook and construct the typed request. The snapshot service validates contact-group access, ownership, duplicate IDs/local identifiers, required values, ranges, dates, link references, orphan addresses, control data and other domain rules. Its errors are displayed without changing or automatically retrying the submitted data.

API errors display their codes, details and source pointers. `423` requires beta/user-context enablement; `401`/`403` requires scope/access checks. A `409` signals a concurrent update. Automatic retries are disabled on the snapshot POST: after a timeout or server error, read the profile before retrying because the write may already have committed.

#### Regeneration and verification

```bash
dotnet tool restore
dotnet tool run kiota update --output ./Generated/KiotaClient --clean-output
dotnet build MyCrmSampleClient.sln
dotnet test MyCrmSampleClient.sln
```

The tests exercise AU/NZ workbook round trips, existing IDs, leading zeroes, cell parsing, preservation of unresolved links and duplicate IDs, snapshot scopes, JSON:API request/response serialization, multi-page exports and linked-resource mapping. Control-data tests verify saved Excel validation rules and named ranges, readable selections converting to IDs, category pagination, dependent subtype lists and the current version 1 template. Import tests verify that the entered data is submitted unchanged to the nominated group without contact/control reads, and that API validation errors are surfaced. They use a fake HTTP handler and do not change live MyCRM data. Keep the workbook version at 1 until versioning and older-format support are explicitly requested.

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
