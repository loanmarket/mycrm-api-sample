# MyCRM Sample

To use these samples you will need credentials and details supplied by Loan Market Group.

## Gaining access

Customers and partners wishing to access the api will need to request credentials for each interacting application. 

The following information will be required to support your request:
- The operation(s) you wish to perform (e.g. read contacts)
- The context of the access. For example, it may be for a broker organisation or based on a subscription agreement

## Using the Postman Collection

The included [Postman Collection](MyCRM%20API%20Sample.postman_collection.json) can be imported into [Postman](https://www.postman.com/) to test the behaviour of the MyCRM API.

To begin, go to the `Variables` tab and populate the following values:

- Auth_Url
- ClientID
- ClientSecret
- AdviserContactId
- PublicAPI_Url
- Scopes

This will allow you to execute the various sample operations. The authorization flow is handled by the `Pre-request Script`.

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

Significant differences between application and user access are:
- Credentials for an application are not issued for a single user. At a minimum, it is issued for an organisation
- Only records owned by the organisation/user are accessible. Specifically, records shared with a user/organisation you do have access to by a user/organisation that you can't access will not be accessible

The nominated `AdviserContactId` is sent as the `UserId` header with each request.

- During maintenance operations (POST, PATCH, DELETE) they will be treated as the creator/last modified user of the record. For leads as an example, this will result in the lead being allocated to them.
- In most GET operations the `UserId` header has no impact
- In some search operations it may limit responses to those linked to the user

## Understanding Scopes

[OAuth scopes](https://datatracker.ietf.org/doc/html/rfc6749#section-3.3) are used to control the access you have for the MyCRM api. You will be informed of the scopes available to you when your credentials are issued. 

Example scopes are:

- `api`: full access to all endpoints
- `api.read`: read access to all entities by their key (e.g. GET `/jsonapi/contacts/{id}`)
- `api.seach`: access to query for an entity (e.g. GET `/jsonapi/contacts`)
- `api.leads`: access to the `/jsonapi/leads` endpoints to create and monitor leads
- `api.marketing`: access to the `/jsonapi/contact-marketing` endpoints to update marketing consent
- `api.contacts`: access to the `/jsonapi/contacts` endpoints to manage contacts
- `api.contacts.read`: access to the read endpoints under `/jsonapi/contacts`

You will only be able to access an endpoint if you request an applicable scope, regardless of the scopes available to you.
