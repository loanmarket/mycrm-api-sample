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

                var contactGroup = new ContactGroupAttributes
                {
                    Name = "The Flinstones"
                };
                
                var contact = new ContactAttributes
                {
                    Title = "Mr",
                    FirstName = "Fred",
                    LastName = "Flinstone",
                    PreferredName = "Freddy",
                    Mobile = "0400123456",
                    Email = "freddy.flintstone@outlook.com"
                };
                
                var authClient = new AuthClient();
                var token = await authClient.GetAuthorizationToken(authConfig);
                if (!token.Success) return;

                var mycrmClient = new MyCrmApiClient(new MyCrmApiClientCredential(token), new MyCrmApiClientOptions(mycrmConfig.AdviserContactId, mycrmConfig.Url));
                
                var groupId = await FindOrCreateContactGroup(mycrmClient, contactGroup);
                
                if (string.IsNullOrEmpty(groupId)) return;

                var contactId = await FindOrCreateContact(mycrmClient, groupId, contact);
                
                    
                var getContactsResp = await mycrmClient.Contacts.GetAsync(sort: new[] {"-id"});
                if (getContactsResp.Value == null)
                {
                    Log.Fatal("Get contacts failed {Status}", getContactsResp.GetRawResponse().Status);
                    return;
                }

                Log.Information("Get contacts returned {Count} contacts", getContactsResp.Value.Data.Count);
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Unexpected error");
            }
        }

        private static async Task<string> FindOrCreateContactGroup(MyCrmApiClient mycrmClient, ContactGroupAttributes contactGroup)
        {
            var getGroupsResp = await mycrmClient.ContactGroups.GetAsync(filter: 
                new[] { $"equals({nameof(ContactGroupAttributes.Name).ToCamelCase()},'{contactGroup.Name}')" });
            
            if (getGroupsResp.Value == null)
            {
                Log.Fatal("Get contact groups failed {Status}", getGroupsResp.GetRawResponse().Status);
                return null;
            }

            string contactGroupId;
            if (getGroupsResp.Value.Data.Count != 0)
            {
                contactGroupId = getGroupsResp.Value.Data.First().Id;
                Log.Information("Found existing contact group {ContactGroupId}", contactGroupId);
            }
            else
            {
            
                var addGroupResp = await mycrmClient.ContactGroup.PostAsync(
                    new ContactGroupDocument(
                        new ContactGroup(ContactGroupsType.ContactGroups, null,
                            contactGroup,null, null, null)
                    ));

                if (addGroupResp.Value == null)
                {
                    Log.Fatal("Add contact group failed {Status}", addGroupResp.GetRawResponse().Status);
                    return null;
                }
            
                contactGroupId = addGroupResp.Value.Data.Id;
                Log.Information("Created contact group {ContactGroupId}", contactGroupId);
            }

            return contactGroupId;
        }
        
        private static async Task<string> FindOrCreateContact(MyCrmApiClient mycrmClient, string groupId, ContactAttributes contact)
        {
            var getContactsResp = await mycrmClient.Contacts.GetAsync(filter: 
                new[] {$"equals({nameof(ContactAttributes.Email).ToCamelCase()},'{contact.Email}')"});
            
            if (getContactsResp.Value == null)
            {
                Log.Fatal("Get contacts failed {Status}", getContactsResp.GetRawResponse().Status);
                return null;
            }

            string contactId;
            if (getContactsResp.Value.Data.Count != 0)
            {
                contactId = getContactsResp.Value.Data.First().Id;
                Log.Information("Found existing contact {ContactId}", contactId);
            }
            else
            {
                var addContactResp = await mycrmClient.Contact.PostAsync(
                    new ContactDocument(
                        new Contact(ContactsType.Contacts, null,
                            contact,
                            new ContactRelationships
                            {
                                ContactGroup = new RelationshipsSingleDocument(
                                    new ResourceIdentifier(ContactGroupsType.ContactGroups.ToString(), groupId))
                            }, null, null)
                    ));

                if (addContactResp.Value == null)
                {
                    Log.Fatal("Add contact failed {Status}", addContactResp.GetRawResponse().Status);
                    return null;
                }

                contactId = addContactResp.Value.Data.Id;
                Log.Information("Created contact {ContactId}", contactId);
            }

            return contactId;
        }
    }
}
