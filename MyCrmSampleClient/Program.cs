using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MyCrmSampleClient.Auth;
using MyCrmSampleClient.MyCrmApi.Models;
using Serilog;

namespace MyCrmSampleClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            try
            {
                var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddUserSecrets(typeof(Program).Assembly)
                    .AddCommandLine(args)
                    .Build();

                var authConfig = config.GetSection("Auth").Get<AuthConfig>();
                var mycrmConfig = config.GetSection("MyCRM").Get<MyCrmConfig>();

                var contactGroup = new ContactGroupAttributes();
                
                var contact = new ContactAttributes
                {
                    Title = "Mr",
                    FirstName = "Fred",
                    LastName = "Flinstone",
                    PreferredName = "Freddy",
                    Mobile = "0400123456",
                    Email = "freddy.flintstone@outlook.com",
                    Gender = ContactAttributesGender.Male 
                };
                
                var authClient = new AuthClient();
                var token = await authClient.GetAuthorizationToken(authConfig);
                if (!token.Success) return;

                var mycrmClient = new MyCrmApiClient(new MyCrmApiClientCredential(token), new MyCrmApiClientOptions(mycrmConfig.AdviserContactId, mycrmConfig.Url));
                
                var (groupId, contactId) = await FindOrCreateContact(mycrmClient, contact);

                var contactRefsForGroup = await mycrmClient.ContactGroupRelationship.GetContactsAsync(int.Parse(groupId));
                Log.Information("Contact References for Contact Group {GroupId} returned {@Details}", groupId, contactRefsForGroup.Value.Data?.Select(x => x.Id));

                var contactsByGroup = mycrmClient.ContactGroupRelated.GetContacts(int.Parse(groupId));
                Log.Information("Contacts for Contact Group {GroupId} returned {@Details}", groupId, contactsByGroup.Value.Data?.Select(x => x.Attributes));
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Unexpected error");
            }
        }
        
        private static async Task<(string contactGroupId, string contactId)> FindOrCreateContact(MyCrmApiClient mycrmClient, ContactAttributes contact)
        {
            var getContactsResp = await mycrmClient.Contacts.GetAsync(filter: 
                new[] {$"equals({nameof(ContactAttributes.Email).ToCamelCase()},'{contact.Email}')"});
            
            if (getContactsResp.Value == null)
            {
                Log.Fatal("Get contacts failed {Status}", getContactsResp.GetRawResponse().Status);
                return (null, null);
            }

            string contactId, contactGroupId;
            if (getContactsResp.Value.Data.Count != 0)
            {
                contactId = getContactsResp.Value.Data.First().Id;
                Log.Information("Found existing contact {ContactId}", contactId);

                var getContactGroupsResp =
                    await mycrmClient.ContactRelationship.GetContactGroupsAsync(int.Parse(contactId));

                if (getContactGroupsResp.Value == null)
                {
                    Log.Fatal("Get contacts associated group failed {Status}", getContactGroupsResp.GetRawResponse().Status);
                    return (null, null);
                }
                
                contactGroupId = getContactGroupsResp.Value.Data.Id;
            }
            else
            {
                var addGroupResp = await mycrmClient.ContactGroup.PostAsync(
                    new ContactGroupDocument(
                        new ContactGroup
                        {
                            Attributes = new ContactGroupAttributes()
                        }
                    ));

                if (addGroupResp.Value == null)
                {
                    Log.Fatal("Add contact group failed {Status}", addGroupResp.GetRawResponse().Status);
                    return (null, null);
                }
            
                contactGroupId = addGroupResp.Value.Data.Id;
                Log.Information("Created contact group {ContactGroupId}", contactGroupId);
                
                var addContactResp = await mycrmClient.Contact.PostAsync(
                    new ContactDocument(
                        new Contact
                        {
                            Attributes = contact,
                            Relationships = new ContactRelationships
                            {
                                ContactGroup = new RelationshipsSingleDocument(
                                    new ResourceIdentifier("contact-groups", contactGroupId))
                            }
                        }
                    ));

                if (addContactResp.Value == null)
                {
                    Log.Fatal("Add contact failed {Status}", addContactResp.GetRawResponse().Status);
                    return (null, null);
                }

                contactId = addContactResp.Value.Data.Id;
                Log.Information("Created contact {ContactId}", contactId);
            }

            return (contactGroupId, contactId);
        }
    }
}
