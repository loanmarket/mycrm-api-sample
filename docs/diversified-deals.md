# Diversified deal read samples

These samples use the generated Kiota client for Asset Finance, Commercial and Asset Referral summaries. They make GET requests only.

| Resource | Endpoint | Search/read scope prefix | Saved state |
|---|---|---|---|
| Asset Finance | `/jsonapi/asset-deal` | `api.diversifiedassetdeals` | `LastAssetDealId` |
| Commercial | `/jsonapi/commercial-deal` | `api.diversifiedcommercialdeals` | `LastCommercialDealId` |
| Asset Referral | `/jsonapi/asset-referral` | `api.diversifiedassetreferrals` | `LastAssetReferralId` |

Append `.search` for collections or `.read` for members. Applicable broader scopes also work. `api.deals` does not grant diversified access. Configure credentials as described in the root README. `MyCRM:Url` is the API host without `/jsonapi`; a configured adviser contact ID supplies `UserId`. These reads can operate with mandate-only access when no user is resolved; a resolved user further narrows access. The adviser relationship uses an adviser family ID, distinct from the adviser contact ID.

## Run the samples

From `MyCrmSampleClient`, run `dotnet run` and choose a sample. Implementations are in [DiversifiedSamples.cs](../MyCrmSampleClient/DiversifiedSamples.cs).

| Example | Asset Finance | Commercial | Asset Referral |
|---|---|---|---|
| Latest page | `latest-asset-deals` | `latest-commercial-deals` | `latest-asset-referrals` |
| Page 2, size 3 | `paged-asset-deals` | `paged-commercial-deals` | — |
| Filtered page | `filtered-asset-deals` | `filtered-commercial-deals` | `filtered-asset-referrals` |
| Member summary | `get-asset-deal` | `get-commercial-deal` | `get-asset-referral` |
| Adviser and applicants | `asset-deal-includes` | `commercial-deal-includes` | `asset-referral-includes` |
| Sparse fields | `asset-deal-fields` | `commercial-deal-fields` | `asset-referral-fields` |

Start with the latest-page sample, then run the member, includes or fields sample for that resource. Collection samples remember the first returned ID. All three member variants use the saved ID or prompt for a positive Engagement V1 ID when none is saved, so they can also run independently. Use Show shared state to inspect IDs; Clear shared state clears all saved selections if you need to choose a different ID. No sample overwrites the separate `LastDealId` used by existing deal-write samples.

The filtered Asset Finance sample selects records with status `Settled`, and the filtered Commercial sample selects records with status `NewApplication`; neither restricts creation date. The filtered Asset Referral sample selects records created on or after the UTC date 365 days ago without restricting status, so records with a missing status can also match. Empty results preserve saved IDs. Pages are limited to 10 records (3 for `paged-asset-deals` and `paged-commercial-deals`), ordered by creation timestamp descending; these samples do not traverse all pages.

To check bracketed pagination, run `latest-asset-deals`, then `paged-asset-deals`, or `latest-commercial-deals`, then `paged-commercial-deals`. The latter uses `.Page(2, 3)` and should return records 4–6 from the same ordering when the data has not changed and creation timestamps are distinct. It returns at most three records. In Rider, put a breakpoint on `rawUrl` in `JsonApiSearchBuilder.GetAsync` and inspect the outgoing URL for `page%5Bnumber%5D=2` and `page%5Bsize%5D=3` (decoded: `page[number]=2&page[size]=3`). The menu description alone does not prove the parameters were sent.

## Requests demonstrated

Expressions below are shown before URL encoding. The query builder encodes them before sending.

```http
GET /jsonapi/asset-deal?page[number]=1&page[size]=10&sort=-createdDate
GET /jsonapi/asset-deal?page[number]=2&page[size]=3&sort=-createdDate
GET /jsonapi/commercial-deal?page[number]=2&page[size]=3&sort=-createdDate
GET /jsonapi/commercial-deal?filter=greaterOrEqual(createdDate,'2026-01-01')&page[number]=1&page[size]=10&sort=-createdDate
GET /jsonapi/asset-referral/7001
GET /jsonapi/asset-referral/7001?include=adviser,applicants,applicantBusinesses
GET /jsonapi/asset-deal/5001?include=adviser,applicants,applicantBusinesses&fields[asset-deal]=title,status,amount,createdDate,adviser,applicants,applicantBusinesses&fields[advisers]=id&fields[contacts]=id&fields[business]=id
```

The includes sample prints relationship identities and whether each exists in `included`, matching both resource type and ID. A person and a business with the same numeric ID are different resources. Handle missing advisers and empty applicant arrays. The fields sample prints only the requested summary fields and related identities.

## Contract details

- Asset Finance amount comes from the first active loan split (ordering is not guaranteed). Commercial amount sums non-deleted split amounts. Asset Referral amount is the proposed loan amount.
- Asset and Commercial lifecycle fields are `submittedDate`, `decisionDate` and `settlementDate`. Referrals use `referralSentDate`, `approvalDate` and `settlementDate`. Missing dates are null; estimated settlement is not exposed.
- Referral statuses are `NewReview`, `Submitted`, `Approved`, `Committed`, `ClosedWon`, `ClosedLost`, `NotProceededWith`. Source `Draft`/`Review` map to `NewReview`, and `Closed - Won`/`Closed - Lost` map to `ClosedWon`/`ClosedLost`. Filter with published values; labels can retain source wording.
- Use a single `filter` with `and(...)`. Supported criteria are status, adviser ID, involved person/business IDs and inclusive creation-date bounds. Generic examples using `greaterThan(id,...)` or arbitrary fields do not apply to these resources.
- `has(applicants,equals(id,'1001'))` and its business equivalent match active involved-party links, including Guarantors. Returned applicant relationships require the Applicant role.
- Standalone relationship routes and diversified writes are unavailable. Task relationships target Asset Finance and Commercial deals; there is no dedicated Asset Referral task relationship.

Use the [Asset Finance](https://api-docs.mycrm.io/guides/data/asset-deals/), [Commercial](https://api-docs.mycrm.io/guides/data/commercial-deals/) and [Asset Referral](https://api-docs.mycrm.io/guides/data/asset-referrals/) guides for the full contracts. Availability depends on your target environment and granted access.

## Errors and build verification

Missing scopes skip samples before requests. API errors propagate to the existing console error handler; failed reads do not change saved IDs. A 404 can indicate missing, inaccessible, inactive or wrong-category records. Check token/scopes for 401/403 and query values for 400. Use positive IDs; zero adviser/person filter IDs can trigger a current API parsing error. Do not automatically retry unchanged validation failures.

From the repository root:

```bash
dotnet build MyCrmSampleClient/MyCrmSampleClient.csproj
```

To verify requests against your environment, run the sample client and select the diversified samples described above. These requests require configured credentials and access to the relevant records.
