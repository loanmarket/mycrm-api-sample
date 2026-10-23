# MyCRM API samples

> **[Read the MyCRM API documentation](https://api-docs.mycrm.io/)** for access, authentication, scopes, API behaviour and endpoint reference material.

This repository contains Postman collections and an interactive C# client for trying MyCRM API calls.

## Repository contents

| Item                                                                           | Purpose                                                                                                                    |
| ------------------------------------------------------------------------------ | -------------------------------------------------------------------------------------------------------------------------- |
| [MyCRM API Sample](MyCRM%20API%20Sample.postman_collection.json)               | Postman requests for advisers, contacts, groups, deals, leads and related resources.                                       |
| [MyCRM Leads API Sample](MyCRM%20Leads%20API%20Sample.postman_collection.json) | A smaller Postman collection for creating and retrieving leads.                                                            |
| [MyCrmSampleClient](MyCrmSampleClient/README.md)                               | A .NET 10 console application with interactive samples, including deal/task workflows and financial snapshot spreadsheets. |
| [swagger.json](MyCrmSampleClient/swagger.json)                                 | The OpenAPI specification used to generate the C# client bundled in this repository.                                       |

## Before running the samples

You need MyCRM API credentials, service URLs, approved scopes and an adviser contact ID for your environment. See [Get access to the API](https://api-docs.mycrm.io/guides/get-started/access/) if you do not have these values.

The collections and client make live API calls. Write samples create or update records in the configured environment.

## Using the Postman collections

1. Import either collection above into [Postman](https://www.postman.com/).
2. Open the collection's **Variables** tab and populate:

   | Variable           | Value                                                                            |
   | ------------------ | -------------------------------------------------------------------------------- |
   | `Auth_Url`         | Your OAuth token endpoint URL.                                                   |
   | `ClientID`         | Your application client ID.                                                      |
   | `ClientSecret`     | Your application client secret.                                                  |
   | `Scopes`           | The approved scopes needed by the requests you want to run, separated by spaces. |
   | `AdviserContactId` | The adviser contact ID used for the request context.                             |
   | `PublicAPI_Url`    | Your API base URL.                                                               |

3. Select and send a request. The pre-request scripts obtain and reuse access tokens automatically.

Some requests use IDs saved by earlier requests, such as `lastContactId` or `lastDealId`. Run the corresponding read/create request first, or supply an accessible record ID.

For the API concepts behind the requests, see [Explore with Postman](https://api-docs.mycrm.io/guides/get-started/postman/) and the [API documentation](https://api-docs.mycrm.io/).

## Using the sample client

Install the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10), then follow the [client setup instructions](MyCrmSampleClient/README.md#running-the-application) to configure and run the application.

The [client README](MyCrmSampleClient/README.md) covers the menu, saved sample IDs, financial snapshot workbooks, tests and client regeneration.
