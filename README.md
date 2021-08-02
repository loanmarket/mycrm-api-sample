# MyCRM Sample

To use these samples you will need credentials and details supplied by Loan Market Group.

## Using the Postman Collection

The included [Postman Collection](MyCRM%20API%20Sample.postman_collection.json) can be imported into [Postman](https://www.postman.com/) to test the behaviour of the MyCRM API.

To begin, go to the `Variables` tab and populate the following values:

- Auth_Url
- ClientID
- ClientSecret
- AdviserContactId
- PublicAPI_Url

This will allow you to execute the various sample operations. The authorization flow is handled by the `Pre-request Script`.

## Using the Sample Client

In the [MyCrmSampleClient](./MyCrmSampleClient) folder edit the [appsettings.json](./MyCrmSampleClient/appsettings.json) file to populate the following values:

```json
{
    "Auth": {
        "Url": "",
        "ClientID": "",
        "ClientSecret": ""
    },
    "MyCRM": {
        "Url": "",
        "AdviserContactId": 0
    }
}
```

Then run `dotnet run`
