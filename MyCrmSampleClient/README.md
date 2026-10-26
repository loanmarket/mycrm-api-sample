# MyCRM Sample Client

An interactive .NET console application for running the C# examples in this repository against MyCRM.

> **[MyCRM API documentation](https://api-docs.mycrm.io/)** covers authentication, scopes, JSON:API, request costs and endpoint behaviour. This README covers running and extending the sample application.

## Running the application

Install the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10) and obtain the values described in [Get access to the API](https://api-docs.mycrm.io/guides/get-started/access/).

From the repository root:

```bash
cd MyCrmSampleClient
```

Populate [appsettings.json](appsettings.json) with your environment's values:

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

Use the API host URL for `MyCRM:Url`, without `/jsonapi`; the generated client adds that path. `Auth:Scopes` is a space-separated list of approved scopes. The application also loads .NET user secrets and command-line configuration, which override `appsettings.json`.

Then start the application:

```bash
dotnet run
```

Use the arrow keys to select a sample and **Enter** to run it. **Esc** returns one menu level or abandons an unfinished form; at the main menu it asks for confirmation before exiting. Completed API actions are not undone.

**Write samples change live records**, including when selected through **Run all samples**. Check the configured environment and **Show shared state** before running them. Samples check their required scopes and display missing prerequisites.

## Samples and shared state

IDs returned by samples are saved between runs in `MyCrmSampleClient/sample-state.json` under the platform's local application data directory. Use **Show shared state** or **Clear shared state** in the menu to inspect or reset them.

| Saved value              | Set by                                                                                                                                                        | Used by                                                                                          |
|--------------------------|---------------------------------------------------------------------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------------|
| `LastContactId`          | `latest-contacts`, `filtered-latest-contacts`                                                                                                                 | Contact/address reads and marketing updates.                                                     |
| `LastContactGroupId`     | `latest-contact-groups`                                                                                                                                       | Group contact/email reads, contact-group tasks and financial snapshot group prompts.             |
| `LastDealId`             | `latest-deals`, `create-lead`                                                                                                                                 | Deal notes, name/BID edits, deal status reads/changes, structure creation and residential tasks. |
| `LastTaskId`             | Either task-creation sample, `get-task`, `promote-contact-task`                                                                                               | Task field selection and updates; default ID for `get-task`.                                     |
| `LastResidentialTaskId`  | `create-residential-task`                                                                                                                                     | Remembering the original residential task when switching tasks.                                  |
| `LastContactGroupTaskId` | `create-contact-group-task`                                                                                                                                   | `promote-contact-task`, even if another task has been selected.                                  |
| `LastAssetDealId`        | `latest-asset-deals`, `filtered-asset-deals`, `paged-asset-deals`, `get-asset-deal`, `asset-deal-includes`, `asset-deal-fields`                               | Asset finance deal reads, related adviser/applicant reads and field selection.                   |
| `LastCommercialDealId`   | `latest-commercial-deals`, `filtered-commercial-deals`, `paged-commercial-deals`, `get-commercial-deal`, `commercial-deal-includes`, `commercial-deal-fields` | Commercial deal reads, related adviser/applicant reads and field selection.                      |
| `LastAssetReferralId`    | `latest-asset-referrals`, `filtered-asset-referrals`, `get-asset-referral`, `asset-referral-includes`, `asset-referral-fields`                                | Asset referral reads, related adviser/applicant reads and field selection.                       |

Start with `latest-contacts`, `latest-contact-groups` and `latest-deals` to populate the shared IDs.

### Contacts, groups and leads

These examples are implemented in [Samples.cs](Samples.cs). See the documentation site's [query guide](https://api-docs.mycrm.io/guides/json-api/querying/) and [writing guide](https://api-docs.mycrm.io/guides/json-api/writing/) for request syntax and behaviour.

| Sample                     | What it does                                                                  |
| -------------------------- | ----------------------------------------------------------------------------- |
| `advisers`                 | Lists advisers and their IDs.                                                 |
| `adviser-details`          | Lists adviser profiles, including the contact ID used for `AdviserContactId`. |
| `latest-contacts`          | Lists recent contacts and saves `LastContactId`.                              |
| `contact-with-address`     | Reads the saved contact with included addresses.                              |
| `filtered-latest-contacts` | Filters and sorts contacts, then saves `LastContactId`.                       |
| `latest-contact-groups`    | Lists groups with their contacts and saves `LastContactGroupId`.              |
| `contact-group-contacts`   | Reads all contacts in the saved group.                                        |
| `contact-group-emails`     | Reads the saved group's contacts with only their email fields.                |
| `patch-contact-marketing`  | Updates the saved contact's marketing consent.                                |
| `latest-deals`             | Lists recent deals and saves `LastDealId`.                                    |
| `create-lead`              | Creates a lead and saves the returned deal ID.                                |
| `create-update-deal-note`  | Creates and updates a note on the saved deal.                                 |

### Deal edits and custom statuses

Implementations are in [DealSamples.cs](DealSamples.cs) and [DealStructureSamples.cs](DealStructureSamples.cs). The [deal guide](https://api-docs.mycrm.io/guides/data/deals/) covers write permissions, country-specific BID fields, structures and status transitions.

Run `latest-deals` or `create-lead` first and check `LastDealId`. Structure reads/updates and organisation status reads prompt for their own IDs.

| Sample                         | What it does                                                    |
| ------------------------------ | --------------------------------------------------------------- |
| `patch-deal-name`              | Prompts for a new name for the saved deal.                      |
| `patch-deal-rationale`         | Reads the adviser's country and prompts for AU or NZ BID notes. |
| `custom-statuses`              | Lists accessible statuses in pipeline order.                    |
| `deal-custom-status`           | Reads the saved deal's current custom status.                   |
| `organisation-custom-statuses` | Prompts for an organisation and lists its statuses.             |
| `get-deal-structure`           | Prompts for a structure ID and displays it.                     |
| `create-deal-structure`        | Creates a structure on the saved deal and reads it back.        |
| `patch-deal-structure`         | Updates a structure's repayment amount and reads it back.       |
| `change-deal-status`           | Prompts for a target custom status for the saved deal.          |

### Diversified deals

Implementations are in [DiversifiedSamples.cs](DiversifiedSamples.cs). These 17 samples make read-only requests for Asset Finance, Commercial and Asset Referral summaries. See the [Asset Finance](https://api-docs.mycrm.io/guides/data/asset-deals/), [Commercial](https://api-docs.mycrm.io/guides/data/commercial-deals/) and [Asset Referral](https://api-docs.mycrm.io/guides/data/asset-referrals/) guides for API fields, access rules and supported queries.

| Sample                      | What it does                                                                               |
|-----------------------------|--------------------------------------------------------------------------------------------|
| `latest-asset-deals`        | Lists recent asset finance deals and saves the first ID as `LastAssetDealId`.              |
| `filtered-asset-deals`      | Lists asset finance deals with status `Settled`, without a creation-date restriction.      |
| `paged-asset-deals`         | Lists page 2 of asset finance deals with 3 records per page.                               |
| `get-asset-deal`            | Reads the saved asset finance deal, or prompts for an ID when none is saved.               |
| `asset-deal-includes`       | Reads an asset finance deal with its adviser and person/business applicants.               |
| `asset-deal-fields`         | Reads selected asset finance fields and related record IDs.                                |
| `latest-commercial-deals`   | Lists recent commercial deals and saves the first ID as `LastCommercialDealId`.            |
| `filtered-commercial-deals` | Lists commercial deals with status `NewApplication`, without a creation-date restriction.  |
| `paged-commercial-deals`    | Lists page 2 of commercial deals with 3 records per page.                                  |
| `get-commercial-deal`       | Reads the saved commercial deal, or prompts for an ID when none is saved.                  |
| `commercial-deal-includes`  | Reads a commercial deal with its adviser and person/business applicants.                   |
| `commercial-deal-fields`    | Reads selected commercial fields and related record IDs.                                   |
| `latest-asset-referrals`    | Lists recent asset referrals and saves the first ID as `LastAssetReferralId`.              |
| `filtered-asset-referrals`  | Lists asset referrals created on or after the UTC date 365 days ago, regardless of status. |
| `get-asset-referral`        | Reads the saved asset referral, or prompts for an ID when none is saved.                   |
| `asset-referral-includes`   | Reads an asset referral with its adviser and person/business applicants.                   |
| `asset-referral-fields`     | Reads selected asset referral fields and related record IDs.                               |

Collection samples require the resource's diversified `.search` scope; member, includes and fields samples require `.read`. Scope prefixes are `api.diversifiedassetdeals`, `api.diversifiedcommercialdeals` and `api.diversifiedassetreferrals`. Applicable broader scopes also work; `api.deals` does not grant diversified access. The sample checks its scope before sending a request.

Start with `latest-asset-deals` → `asset-deal-includes`, `latest-commercial-deals` → `commercial-deal-fields`, or `latest-asset-referrals` → `get-asset-referral`. Collections save the first returned ID in the corresponding shared-state value. Member samples use that saved ID or prompt for a positive Engagement V1 ID when none is saved; successful reads save the returned ID. They can therefore run independently. Empty results preserve saved IDs, and diversified samples do not replace `LastDealId`. Use **Clear shared state** before a member sample if you need to enter another ID.

The Asset Finance filter selects `Settled` records; the Commercial filter selects `NewApplication` records. Neither restricts creation date. The Asset Referral filter selects records created on or after the UTC date 365 days ago without restricting status. Empty results can mean no accessible records match the selected filter.

Latest and filtered samples request page 1 with 10 records. The two paged samples request page 2 with 3 records. All sort by creation date descending and display one page rather than traversing the whole collection. Run a resource's `latest-*` sample followed by its `paged-*` sample to compare results: the latter should show rows 4–6 when the data is unchanged and creation timestamps are distinct.

To check the bracketed pagination parameters in Rider, inspect the outgoing URL in `JsonApiSearchBuilder.GetAsync` for `page[number]=2&page[size]=3` (URL-encoded brackets appear as `%5B` and `%5D`). The menu description alone does not confirm which parameters were sent.

### Tasks

Implementations are in [TaskSamples.cs](TaskSamples.cs). See the [task guide](https://api-docs.mycrm.io/guides/data/tasks/) for API scopes, request bodies and relationship rules.

| Sample                      | What it does                                                              | Prerequisite                                                                                                |
| --------------------------- | ------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------- |
| `create-residential-task`   | Creates a task and saves its ID.                                          | `LastDealId` from `latest-deals` or `create-lead`.                                                          |
| `create-contact-group-task` | Creates a contact-group task and saves its ID.                            | `LastContactGroupId` from `latest-contact-groups`.                                                          |
| `get-task`                  | Reads a prompted task ID and selects it as `LastTaskId`.                  | Any accessible task ID; no earlier sample required.                                                         |
| `task-fields`               | Reads selected fields from the saved task.                                | `LastTaskId`.                                                                                               |
| `patch-task`                | Prompts for title/detail, updates the saved task and reads it back.       | `LastTaskId`.                                                                                               |
| `task-progress`             | Prompts for progress, updates the saved task and reads it back.           | `LastTaskId`.                                                                                               |
| `promote-contact-task`      | Prompts for a residential deal and promotes the saved contact-group task. | `LastContactGroupTaskId` from `create-contact-group-task`; the sample checks that it can still be promoted. |

Creation prompts for an assignee ID from `advisers`; this is separate from the configured `AdviserContactId`. All task samples require `AdviserContactId`, and update samples also need read scope to display the result.

For an existing task, run `get-task`, then any of `task-fields`, `patch-task` or `task-progress`. For promotion, run `latest-contact-groups` → `create-contact-group-task` → `promote-contact-task`. Shared IDs persist, so samples do not need to run consecutively.

### Financial snapshots — Excel import, export and fabricated examples

Select **financial-snapshots** in the main menu. **Run all samples** skips this section. The implementation starts in [FinancialSnapshotSamples.cs](FinancialSnapshotSamples.cs), with workbook helpers in [FinancialSnapshots/](FinancialSnapshots/).

Read the [financial snapshot guide](https://api-docs.mycrm.io/guides/data/financials/#what-the-financial-snapshot-endpoint-does) for access requirements, Merge/Replace modes, matching, ownership and validation before submitting a workbook.

| Option                                          | What it does                                                                                                                                    |
| ----------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------- |
| **Import spreadsheet**                          | Reads an `.xlsx`, previews counts and modes, optionally shows the request, then asks for confirmation before submitting to the nominated group. |
| **Export current financials**                   | Reads the group's financial profile into an editable workbook.                                                                                  |
| **Generate fabricated spreadsheet**             | Writes a local example with fabricated financial details and real contact/control IDs.                                                          |
| **Compare spreadsheet with current financials** | Compares workbook values with live data without changing either.                                                                                |

#### First run

1. Run `latest-contact-groups` or enter a known group ID when prompted.
2. Export current financials to retain existing record IDs, or generate a fabricated example for a test group.
3. Edit and save the workbook, then select **Import spreadsheet**. The group prompt defaults to the workbook's nominated group.
4. Keep the workbook's collection modes or select Merge/Replace for all collections, review the request and confirm the write.
5. Export again to capture saved IDs for later updates.

Import uses `api.contactgroups.financialsnapshot` and the configured `AdviserContactId`. It does not fetch contacts or control data. Export and comparison read the group, contacts, adviser and financial records; export and generation also read the control data used in dropdowns. The menu reports missing scopes.

Files must use the `.xlsx` extension. Saving over an existing file asks for confirmation. No Excel installation is needed to generate or read files; the client uses ClosedXML.

#### Workbook format (version 1)

Start with a fresh export or generated workbook. All data sheets and the supplied country-specific headers are required for import, even when empty. Columns may be reordered, but names are case-sensitive. Blank rows are ignored; formulas and error cells are rejected, so paste values. Older workbooks using `valuationBasis` must use the current `valueBasis` header.

| Sheet                                                         | Contents                                                                                      |
| ------------------------------------------------------------- | --------------------------------------------------------------------------------------------- |
| `Snapshot`                                                    | Format version, group ID, source, collection modes and optional country/example metadata.     |
| `Assets`, `Expenses`, `Incomes`, `Liabilities`, `Employments` | Editable rows with existing record IDs, local IDs and fields from the generated input models. |
| `Addresses`                                                   | Address rows referenced by asset/employment `address.lid` cells.                              |
| `Contact Lookups`, `Lookups`                                  | Reference data for contacts and control IDs; editing these sheets does not submit changes.    |
| `Dropdown lists`                                              | Hidden named ranges for Excel validation.                                                     |
| `Excluded records`, `Read me`                                 | Export exclusions and workbook-specific notes.                                                |

The selected group's adviser determines the country for export and generation. AU templates omit `nz` columns; NZ templates retain them. Import preserves explicitly supplied values and leaves domain validation to the API.

- Owner columns show `contactId — Name`; import uses the numeric IDs. Export stops if more than two owners would be needed.
- Existing record IDs are populated by export. Local links use `address.lid`, `linkedAsset.lid` and `employment.lid`; existing links use `linkedAsset.id` or `employment.id`. See the [API linking rules](https://api-docs.mycrm.io/guides/data/financials/#link-new-and-existing-records) when editing them.
- Keep account numbers, BSBs, postcodes and business/company numbers as text to preserve leading zeroes. Enter dates as Excel dates or `YYYY-MM-DD`, booleans as `TRUE`/`FALSE`, and enum values using the dropdowns.
- Control dropdowns show readable ID/name labels, and import extracts the ID. Numeric IDs outside the lists can be entered for server validation. Refresh choices by exporting or generating a new workbook.
- Local-link dropdowns follow the Excel tables. Add rows inside the target table and set their `lid` before linking them. Renaming a `lid` does not rewrite existing links. Reselect a liability subtype after changing its type.
- Dropdowns cover rows 2–1000 and extend for larger initial exports. The importer checks workbook structure and cell types, then submits the entered data unchanged.

Exports follow all pages and deduplicate records. Unrepresentable ownership, multiple asset addresses or missing required relationship data stop export with an explanation. Managed repayment expenses and unsupported incomes appear in the export notes/exclusions; edit repayments on the Liabilities sheet. A standalone `NULL` string is written as a blank cell. Exported reads are sequential live calls.

#### Generated examples

Generation uses [Bogus](https://github.com/bchavez/Bogus) with a random seed. Keep the suggested seed for a fresh example, or reuse it with the same contacts, controls and generator version to reproduce one.

Accept the suggested size or enter 1–1000 financial items. The count combines assets, liabilities, incomes, expenses and employments; addresses and lookup rows are additional. The seed and any explicit size are recorded in the workbook. Contact names and IDs remain real; financial values are fabricated. Generation writes only a local file.

#### Comparing a workbook

Comparison defaults to the workbook's group and reports changed fields, unmatched records and ambiguous matches. Explicit record IDs take priority; without them, the client uses identifying fields, ownership and linked records. Add explicit IDs where a match is ambiguous.

Unlike import, comparison allows removed columns and checks only those present. Blank cells in present columns are compared as empty values; owner order is ignored, and missing owner columns leave that many owners unspecified. Existing-ID and local-ID link columns are alternatives; a blank link with its alternative column missing is unspecified.

The group's actual adviser country controls which AU/NZ fields are compared. Comparison ignores workbook metadata, collection modes, local-ID spelling and reference sheets. It compares stored values, not the predicted result of an import, and does not require write or control-data scopes.

#### Errors and retries

Workbook errors identify the sheet/cell or row where possible. API errors display codes, details and source pointers. Import retries explicit HTTP 429 responses up to eight times with progressive delays and respects a longer `Retry-After`. Other write failures are not automatically retried. See [snapshot results and retries](https://api-docs.mycrm.io/guides/data/financials/#read-the-result-and-handle-retries) and [rate limits](https://api-docs.mycrm.io/guides/integrate/rate-limits/) for API guidance.

## Extending the client

The [generated client](Generated/KiotaClient/) comes from [swagger.json](swagger.json) using Microsoft Kiota. [KiotaExtensions/](KiotaExtensions/) provides fluent query helpers:

```csharp
var contacts = await client.Jsonapi.Contacts
    .Sort("updated", SortOrder.Descending)
    .Filter("greaterThan(updated,'2024-01-01')")
    .Page(1, 10)
    .Fields("contacts", "firstName", "lastName", "email")
    .GetAsync();
```

Use `GetAllAsync<TResource>()` for a complete collection. [JsonApiPagination.cs](KiotaExtensions/JsonApiPagination.cs) follows `links.next`, deduplicates records and supports cancellation. Builders without fluent wrappers can call `JsonApiPagination.ReadPagesAsync<TResource>()`; see the custom-status samples.

### Adding your own samples

Add a method to the partial `Samples` class with a `[Sample]` attribute:

```csharp
[Sample("my-sample", "GET /jsonapi/my-resource")]
public async Task RunMySample()
{
    if (!_console.RequireScope("api.my-resource.search", "my-sample")) return;
    // Use _console.Client to make API calls.
}
```

The attribute registers the method with the menu. Use a unique sample name, declare the required scopes and check any saved IDs before making a request.

### Build and test

Run from `MyCrmSampleClient`:

```bash
dotnet build MyCrmSampleClient.sln
dotnet test MyCrmSampleClient.sln
```

The [tests](Tests/) use fake HTTP handlers and cover authentication, navigation, pagination, rate-limit retries and financial workbook import/export/comparison without changing live MyCRM data.

### Regenerating the API client

The [client update workflow](../.github/workflows/update-client.yml) copies the approved specification from `mycrm-api`, normalises it and regenerates the client. To regenerate locally from the bundled `swagger.json`, run:

```bash
dotnet tool restore
dotnet tool run kiota update --output ./Generated/KiotaClient --clean-output
dotnet build MyCrmSampleClient.sln
dotnet test MyCrmSampleClient.sln
```

The Kiota version is pinned in [dotnet-tools.json](dotnet-tools.json).
