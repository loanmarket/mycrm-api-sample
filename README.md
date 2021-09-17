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
        "Scopes": "api"
    },
    "MyCRM": {
        "Url": "",
        "AdviserContactId": 0
    }
}
```

Then run `dotnet run`

The `AdviserContactId` is solely used during maintenance operations (POST, PATCH, DELETE) and is sent as a `UserId` header. The specified identifier must be a member of the broker organisation corresponding to the entity being managed.

## Understanding Scopes

[OAuth scopes](https://datatracker.ietf.org/doc/html/rfc6749#section-3.3) are used to control the access you have for the MyCRM api. You will be informed of the scopes available to you when your credentials are issued. 

Example scopes are:

- `api`: full access to all endpoints
- `api.leads`: access to the `/jsonapi/leads` endpoints to create and monitor leads
- `api.marketing`: access to the `/jsonapi/contact-marketing` endpoints to update marketing consent
- `api.contacts`: access to the `/jsonapi/contacts` endpoints to manage contacts
- `api.contacts.read`: access to the GET endpoints under `/jsonapi/contacts`

You will only be able to access an endpoint if you request an applicable scope, regardless of the scopes available to you.
