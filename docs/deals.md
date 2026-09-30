# Editing deals, structures and statuses

Use the usual [authentication and `UserId` headers](../README.md#authentication). Deal edits and status changes require `api.deals.update`; structure writes require `api.deal-structures.create` or `api.deal-structures.update` (or broader applicable scopes). Replace the example IDs with your own.

BID notes and structure creation/updates are allowed **only in pre-submission**. Name-only edits are allowed at any status. Status changes follow the movement rules below.

## Editing a deal

`PATCH /jsonapi/deals/{id}` supports `name` (up to 200 characters) and country-specific BID notes. Send only the fields to change; omitted attributes and nested rationale fields remain unchanged.

```http
PATCH /jsonapi/deals/4567890
Content-Type: application/vnd.api+json
Accept: application/vnd.api+json

{
  "data": {
    "type": "deals",
    "id": "4567890",
    "attributes": {
      "name": "Home loan refinance",
      "auRationale": {
        "summary": "Rationale for the lending recommendation."
      }
    }
  }
}
```

### BID notes and country

`auRationale` and `nzRationale` are **write-only BID notes**: neither appears in GET or PATCH responses.

Read `data.attributes.countryCode` from `GET /jsonapi/deals/{id}/adviser`. Use `auRationale` for `AU` or `nzRationale` for `NZ`; the wrong country's field is rejected.

For NZ, use these `attributes` instead:

```json
{
  "nzRationale": {
    "summary": "Rationale for the lending recommendation.",
    "lenderPreferences": "The client's lender preferences.",
    "loanStructure": "Reasons for the recommended structure.",
    "productFeatures": "Reasons for the recommended features."
  }
}
```

PATCH returns `200 OK` with the resource or `204 No Content`. The name can be read back; BID notes cannot.

## Creating and editing deal structures

Use `POST /jsonapi/deal-structures` with a `deal` relationship:

```json
{
  "data": {
    "type": "deal-structures",
    "attributes": {
      "loanStructureType": "PrincipalInterest",
      "loanTermYears": 30,
      "amount": 500000,
      "rateType": "Variable",
      "interestRate": 0.055,
      "paymentAmount": 2800,
      "repaymentFrequency": "Monthly"
    },
    "relationships": {
      "deal": { "data": { "type": "deals", "id": "4567890" } }
    }
  }
}
```

Send `Content-Type` and `Accept: application/vnd.api+json`. POST returns `201 Created` with the structure's ID. Use it for PATCH and GET:

```http
PATCH /jsonapi/deal-structures/1234567
Content-Type: application/vnd.api+json
Accept: application/vnd.api+json

{
  "data": {
    "type": "deal-structures",
    "id": "1234567",
    "attributes": { "paymentAmount": 2900 }
  }
}
```

Omitted attributes remain unchanged. The `deal` relationship is set on creation and cannot be changed by PATCH. Read back with `GET /jsonapi/deal-structures/{id}`; existing IDs are also available from `/jsonapi/deals/{id}/dealStructures`.

Rates use decimal fractions: `0.055` means 5.5%. Other editable fields cover account number, interest-only term, fixed-rate dates and term (`rateTermYears`), repayment frequency and LMI. `rateTypePeriodMonths` and `interestOnlyExpiryDate` are read-only.

## Reading custom statuses

Custom statuses are an organisation's pipeline stages, each with an `id`, `name` and `sortOrder`. Use `?sort=sortOrder` for pipeline order and follow pagination links for further results.

| GET endpoint | Returns |
|---|---|
| `/jsonapi/custom-statuses?sort=sortOrder` | Accessible statuses |
| `/jsonapi/custom-statuses/{id}` | One status |
| `/jsonapi/deals/{id}/customStatus` | The deal's current status |
| `/jsonapi/deals/{id}/relationships/customStatus` | Its status identifier |
| `/jsonapi/organisations/{id}/customStatuses?sort=sortOrder` | The organisation's statuses |
| `/jsonapi/organisations/{id}/relationships/customStatuses` | Its status identifiers |

To find the deal's organisation, read `/deals/{id}/adviser`, then `/advisers/{adviserId}/organisation` under `/jsonapi`.

Alternatively, `/jsonapi/deals/{id}?include=customStatus` returns the current status in `included`, linked by `data.relationships.customStatus.data`.

## Changing a deal's status

Use the target **custom status ID**, not its name or the general `dealStatus` value. It must be active and belong to the **deal's organisation**.

This endpoint uses **plain JSON**, without a `data` wrapper:

```http
PUT /jsonapi/deals/4567890/status
Content-Type: application/json
Accept: application/json

{ "customStatusId": 12345 }
```

A successful request, including one for the existing status, returns `200 OK`:

```json
{ "id": 4567890, "customStatusId": 12345, "statusName": "Submitted to lender" }
```

Invalid targets or movements return `400`; missing or inaccessible deals return `404`. Details appear in `errors`, exposed by Kiota as `ErrorDocument`.

### Allowed movements

Rules depend on the underlying stage, not the custom name or sort order:

| Current stage | Allowed targets |
|---|---|
| Pre-submission | Any stage |
| Submitted, including approvals and pending settlement | Submitted, settled, post-settlement, cancelled |
| Settled | Settled, post-settlement |
| Post-settlement (Varied, Loan Repaid) | Post-settlement |
| Cancelled / not proceeding | Cancelled / not proceeding |

Once submitted, deals cannot return to pre-submission. Settled and post-settlement deals cannot be cancelled.

See the [runnable C# examples](../MyCrmSampleClient/README.md#deal-edits-and-custom-statuses) in [DealSamples.cs](../MyCrmSampleClient/DealSamples.cs) and [DealStructureSamples.cs](../MyCrmSampleClient/DealStructureSamples.cs).
