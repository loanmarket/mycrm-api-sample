using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Kiota.Abstractions;
using MyCrmSampleClient.Console;
using MyCrmSampleClient.Kiota.Models;
using MyCrmSampleClient.KiotaExtensions;
using Serilog;
using Spectre.Console;

namespace MyCrmSampleClient;

public class Samples
{
    private readonly SampleConsole _console;

    public Samples(SampleConsole console)
    {
        _console = console ?? throw new ArgumentNullException(nameof(console));
    }
    
    [Sample("advisers", "GET /jsonapi/advisers?page[size]=10&sort=-id")]
    public async Task RunLatestAdvisersSample()
    {
        if (!_console.RequireScope("api.advisers.search", "latest-advisers")) return;

        var latestAdvisers = await _console.Client.Jsonapi.Advisers
            .Page(1, 10)
            .Sort("id", SortOrder.Descending)
            .GetAsync();

        var advisers = latestAdvisers?.Data?.ToList();
        var latestAdviser = advisers?.FirstOrDefault();
        if (latestAdviser == null)
        {
            Log.Warning("No advisers returned from GET /jsonapi/advisers");
            return;
        }

        Log.Information(
            "Latest adviser from GET /jsonapi/advisers: id={AdviserId}, email={Email}, jobTitle={JobTitle}",
            latestAdviser.Id,
            latestAdviser.Attributes?.Email,
            latestAdviser.Attributes?.JobTitle);

        var table = _console.CreateTable("Id", "Email", "Job Title", "Status", "Created");

        foreach (var adviser in advisers ?? [])
        {
            table.AddRow(
                _console.Cell(adviser.Id),
                _console.Cell(adviser.Attributes?.Email),
                _console.Cell(adviser.Attributes?.JobTitle),
                _console.Cell(adviser.Attributes?.Status),
                _console.Cell(adviser.Attributes?.Created?.ToString("u")));
        }

        _console.WriteTable(table);
    }

    [Sample("adviser-details", "GET /jsonapi/adviser-details?page[size]=10&sort=-id")]
    public async Task RunLatestAdviserDetailsSample()
    {
        if (!_console.RequireScope("api.adviser-details.search", "latest-adviser-details")) return;

        var latestAdviserDetails = await _console.Client.Jsonapi.AdviserDetails
            .Page(1, 10)
            .Sort("id", SortOrder.Descending)
            .GetAsync();

        var adviserDetails = latestAdviserDetails?.Data?.ToList();
        var latest = adviserDetails?.FirstOrDefault();
        if (latest == null)
        {
            Log.Warning("No adviser details returned from GET /jsonapi/adviser-details");
            return;
        }

        Log.Information(
            "Latest adviser detail from GET /jsonapi/adviser-details: id={Id}, name={FirstName} {LastName}, email={Email}",
            latest.Id,
            latest.Attributes?.FirstName,
            latest.Attributes?.LastName,
            latest.Attributes?.Email);

        var table = _console.CreateTable("Id", "First Name", "Last Name", "Email", "Mobile", "Updated");

        foreach (var item in adviserDetails ?? [])
        {
            table.AddRow(
                _console.Cell(item.Id),
                _console.Cell(item.Attributes?.FirstName),
                _console.Cell(item.Attributes?.LastName),
                _console.Cell(item.Attributes?.Email),
                _console.Cell(item.Attributes?.MobilePhone),
                _console.Cell(item.Attributes?.Updated?.ToString("u")));
        }

        _console.WriteTable(table);
    }

    [Sample("latest-contacts", "GET /jsonapi/contacts?page[size]=10&sort=-id")]
    public async Task RunLatestContactsSample()
    {
        if (!_console.RequireScope("api.contacts.search", "latest-contacts")) return;

        var latestContacts = await _console.Client.Jsonapi.Contacts
            .Page(1, 10)
            .Sort("id", SortOrder.Descending)
            .GetAsync();

        var contacts = latestContacts?.Data?.ToList();
        var latestContact = contacts?.FirstOrDefault();
        if (latestContact == null || !int.TryParse(latestContact.Id, out var parsedContactId))
        {
            Log.Warning("No contacts returned from GET /jsonapi/contacts");
            return;
        }

        _console.UpdateState(_console.State with { LastContactId = parsedContactId });
        Log.Information(
            "Latest contact from GET /jsonapi/contacts: id={ContactId}, email={Email}",
            latestContact.Id,
            latestContact.Attributes?.Email);

        var table = _console.CreateTable("Id", "First Name", "Last Name", "Email", "Mobile");

        foreach (var contact in contacts)
        {
            table.AddRow(
                _console.Cell(contact.Id),
                _console.Cell(contact.Attributes?.FirstName),
                _console.Cell(contact.Attributes?.LastName),
                _console.Cell(contact.Attributes?.Email),
                _console.Cell(contact.Attributes?.Mobile));
        }

        _console.WriteTable(table);
    }

    [Sample("contact-with-address", "GET /jsonapi/contacts/{id}?include=contactAddress")]
    public async Task RunContactWithAddressSample()
    {
        if (!_console.RequireScope("api.contacts.read", "contact-with-address")) return;

        if (!_console.State.TryGetContactId("contact-with-address", out var contactId))
        {
            return;
        }

        var contactWithAddress = await _console.Client.Jsonapi.Contacts[contactId]
            .Include("contactAddress")
            .GetAsync();

        var contact = contactWithAddress?.Data;
        var fullName = $"{contact?.Attributes?.FirstName} {contact?.Attributes?.LastName}".Trim();
        var table = _console.CreateTable("Contact Id", "Name", "Email", "Address Type", "Formatted Address");
        var found = false;
        foreach (var addressRef in contact?.Relationships?.ContactAddress?.Data ?? [])
        {
            var address = contactWithAddress
                .Included
                .Where(x => x.Id == addressRef.Id && x.Type == addressRef.Type)
                .FirstOrDefault() as ContactAddress;
            
            table.AddRow(
                _console.Cell(contact?.Id),
                _console.Cell(fullName),
                _console.Cell(contact?.Attributes?.Email),
                _console.Cell(address?.Attributes?.AddressType),
                _console.Cell(address?.Attributes?.FormattedAddress));

            found = true;
        }

        if (!found)
        {
            table.AddRow(
                _console.Cell(contact?.Id),
                _console.Cell(fullName),
                _console.Cell(contact?.Attributes?.Email),
                _console.Cell((string)null),
                _console.Cell((string)null));
        }
        
        _console.WriteTable(table);
    }

    [Sample("filtered-latest-contacts", "GET /jsonapi/contacts?sort=-updated&filter=and(equals(hasMarketingConsent,'true'),greaterThan(updated,'<today-30d>'))")]
    public async Task RunFilteredLatestContactsSample()
    {
        if (!_console.RequireScope("api.contacts.search", "filtered-latest-contacts")) return;

        var updatedSinceDate = DateTimeOffset.Now.AddDays(-30).ToString("yyyy-MM-dd");
        var filter = $"and(equals(hasMarketingConsent,'true'),greaterThan(updated,'{updatedSinceDate}'))";

        var response = await _console.Client.Jsonapi.Contacts
            .Sort("updated", SortOrder.Descending)
            .Filter(filter)
            .GetAsync();

        var contacts = response?.Data?.ToList();
        var latestContact = contacts?.FirstOrDefault();
        if (latestContact == null || !int.TryParse(latestContact.Id, out var parsedContactId))
        {
            Log.Warning(
                "No contacts returned from filtered GET /jsonapi/contacts. updatedSince={UpdatedSinceDate}, filter={Filter}",
                updatedSinceDate,
                filter);
            return;
        }

        _console.UpdateState(_console.State with { LastContactId = parsedContactId });

        Log.Information(
            "Filtered latest contact: id={ContactId}, email={Email}, updatedSince={UpdatedSinceDate}",
            latestContact.Id,
            latestContact.Attributes?.Email,
            updatedSinceDate);

        var table = _console.CreateTable("Id", "First Name", "Last Name", "Email", "Mobile");

        foreach (var contact in contacts)
        {
            table.AddRow(
                _console.Cell(contact.Id),
                _console.Cell(contact.Attributes?.FirstName),
                _console.Cell(contact.Attributes?.LastName),
                _console.Cell(contact.Attributes?.Email),
                _console.Cell(contact.Attributes?.Mobile));
        }

        _console.WriteTable(table);
    }
    
    [Sample("latest-contact-groups", "GET /jsonapi/contact-groups?sort=-id&include=contacts")]
    public async Task RunLatestContactGroupsSample()
    {
        if (!_console.RequireScope("api.contact-groups.search", "latest-contact-groups")) return;

        var contactGroups = await _console.Client.Jsonapi.ContactGroups
            .Sort("id", SortOrder.Descending)
            .Include("contacts")
            .GetAsync();

        var groups = contactGroups?.Data?.ToList();
        var latestGroup = groups?.FirstOrDefault();
        if (latestGroup == null || !int.TryParse(latestGroup.Id, out var parsedGroupId))
        {
            Log.Warning("No contact groups returned from GET /jsonapi/contact-groups");
            return;
        }

        _console.UpdateState(_console.State with { LastContactGroupId = parsedGroupId });
        Log.Information("Latest contact group id={ContactGroupId}", latestGroup.Id);

        var table = _console.CreateTable("Id", "Type", "Contact");
        foreach (var group in groups)
        {
            var foundContact = false;
            foreach (var contactRef in group.Relationships?.Contacts?.Data ?? [])
            {
                var contact = contactGroups
                    ?.Included
                    ?.Where(x => x.Id == contactRef.Id && x.Type == contactRef.Type)
                    ?.FirstOrDefault() as Contact;
                
                var fullName = $"{contact?.Attributes?.FirstName} {contact?.Attributes?.LastName}".Trim();

                if (foundContact)
                {
                    table.AddRow(
                        _console.Cell(" "),
                        _console.Cell(" "),
                        _console.Cell(fullName));
                }
                else
                {
                    table.AddRow(
                        _console.Cell(group.Id),
                        _console.Cell(group.Attributes?.ContactType),
                        _console.Cell(fullName));
                }
                
                foundContact = true;
            }

            if (!foundContact)
            {
                table.AddRow(
                    _console.Cell(group.Id),
                    _console.Cell(group.Attributes?.ContactType),
                    _console.Cell((string)null));
            }
        }

        _console.WriteTable(table);
    }

    [Sample("contact-group-contacts", "GET /jsonapi/contact-groups/{id}/contacts")]
    public async Task RunContactGroupContactsSample()
    {
        if (!_console.RequireScope("api.contact-groups.read", "contact-group-contacts")) return;

        if (!_console.State.TryGetContactGroupId("contact-group-contacts", out var contactGroupId))
        {
            return;
        }

        var groupContacts = await _console.Client.Jsonapi.ContactGroups[contactGroupId].Contacts.GetAsync();
        var contacts = groupContacts?.Data?.ToList() ?? [];

        Log.Information(
            "GET /jsonapi/contact-groups/{ContactGroupId}/contacts returned {Count} contacts",
            contactGroupId,
            contacts.Count);

        var table = _console.CreateTable("Id", "First Name", "Last Name", "Email", "Mobile");
        foreach (var contact in contacts)
        {
            table.AddRow(
                _console.Cell(contact.Id),
                _console.Cell(contact.Attributes?.FirstName),
                _console.Cell(contact.Attributes?.LastName),
                _console.Cell(contact.Attributes?.Email),
                _console.Cell(contact.Attributes?.Mobile));
        }

        _console.WriteTable(table);
    }

    [Sample("contact-group-emails", "GET /jsonapi/contact-groups/{id}/contacts?fields[contacts]=email")]
    public async Task RunContactGroupEmailsSample()
    {
        if (!_console.RequireScope("api.contact-groups.read", "contact-group-emails")) return;

        if (!_console.State.TryGetContactGroupId("contact-group-emails", out var contactGroupId))
        {
            return;
        }

        var groupContactEmails = await _console.Client.Jsonapi.ContactGroups[contactGroupId].Contacts
            .Fields("contacts", "email")
            .GetAsync();

        var table = _console.CreateTable("Contact Id", "Email");
        foreach (var contact in groupContactEmails?.Data?.ToList() ?? [])
        {
            if (!string.IsNullOrWhiteSpace(contact.Attributes?.Email))
            {
                table.AddRow(_console.Cell(contact.Id), _console.Cell(contact.Attributes?.Email));
            }
        }

        _console.WriteTable(table);
    }

    [Sample("patch-contact-marketing", "PATCH /jsonapi/contact-marketing/{id}")]
    public async Task RunPatchContactMarketingSample()
    {
        if (!_console.RequireAdviserContactId("patch-contact-marketing")) return;
        if (!_console.RequireScope("api.contact-marketing.update", "patch-contact-marketing")) return;

        if (!_console.State.TryGetContactId("patch-contact-marketing", out var contactId))
        {
            return;
        }

        var body = new ContactMarketingDocument
        {
            Data = new ContactMarketing
            {
                Type = "contact-marketing",
                Id = contactId.ToString(),
                Attributes = new ContactMarketingAttributes
                {
                    HasMarketingConsent = false
                }
            }
        };

        ContactMarketingDocument response;
        try
        {
            response = await _console.Client.Jsonapi.ContactMarketing[contactId].PatchAsync(body);
        }
        catch (ApiException ex) when (SampleConsole.IsForbidden(ex))
        {
            _console.WarnAdviserContactIdForbidden("patch-contact-marketing");
            return;
        }

        Log.Information(
            "PATCH /jsonapi/contact-marketing/{ContactId} succeeded. hasMarketingConsent={HasMarketingConsent}",
            contactId,
            response?.Data?.Attributes?.HasMarketingConsent);

        var table = _console.CreateTable("Contact Id", "Type", "Has Marketing Consent");
        table.AddRow(
            _console.Cell(response?.Data?.Id ?? contactId.ToString()),
            _console.Cell(response?.Data?.Type),
            _console.Cell(response?.Data?.Attributes?.HasMarketingConsent));

        _console.WriteTable(table);
    }

   [Sample("latest-deals", "GET /jsonapi/deals?sort=-id")]
    public async Task RunLatestDealsSample()
    {
        if (!_console.RequireScope("api.deals.search", "latest-deals")) return;

        var latestDeals = await _console.Client.Jsonapi.Deals
            .Sort("id", SortOrder.Descending)
            .GetAsync();

        var deals = latestDeals?.Data?.ToList();
        var latestDeal = deals?.FirstOrDefault();
        if (latestDeal == null || !int.TryParse(latestDeal.Id, out var parsedDealId))
        {
            Log.Warning("No deals returned from GET /jsonapi/deals?sort=-id");
            return;
        }

        _console.UpdateState(_console.State with { LastDealId = parsedDealId });

        Log.Information(
            "Latest deal from GET /jsonapi/deals: id={DealId}, name={Name}, status={Status}",
            latestDeal.Id,
            latestDeal.Attributes?.Name,
            latestDeal.Attributes?.DealStatus);

        var table = _console.CreateTable("Id", "Name", "Status", "Lender", "Total Loan Amount");

        foreach (var deal in deals ?? [])
        {
            table.AddRow(
                _console.Cell(deal.Id),
                _console.Cell(deal.Attributes?.Name),
                _console.Cell(deal.Attributes?.DealStatus),
                _console.Cell(deal.Attributes?.LenderName),
                _console.Cell(deal.Attributes?.TotalLoanAmount?.ToString("0.##")));
        }

        _console.WriteTable(table);
    }

    [Sample("create-lead", "POST /jsonapi/leads")]
    public async Task RunCreateLeadSample()
    {
        if (!_console.RequireAdviserContactId("create-lead")) return;
        if (!_console.RequireScope("api.leads.create", "create-lead")) return;

        var unique = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var leadBody = new LeadDocument
        {
            Data = new Lead
            {
                Type = "leads",
                Attributes = new LeadAttributes
                {
                    Title = "Mr",
                    FirstName = "Fred",
                    LastName = "Flintstone",
                    Email = $"fred.{unique}@bedrock.example",
                    Mobile = "0412345678",
                    DateOfBirth = new Date(DateTime.Today.AddYears(-31)),
                    Gender = "Male",
                    HasMarketingConsent = true,
                    IsGuarantor = false,
                    IsDependant = false,
                    IsPrimary = true,
                    StreetAddress = "111 Eagle St",
                    Suburb = "Brisbane",
                    State = "Queensland",
                    PostCode = "4000",
                    Country = "Australia",
                    AddressType = "Current",
                    NoteTitle = "About the lead",
                    NoteDetails = "Created from MyCRM sample",
                    UtmSource = "facebook",
                    UtmMedium = "social",
                    UtmCampaign = "mycrm-sample",
                    UtmTerm = "investment",
                    UtmContent = "lm",
                    SourceSystemUrl = "https://www.loanmarket.com.au",
                    ExternalReference = $"mycrm-{unique}",
                    ExternalIntegration = "MyIntegrationSource"
                }
            }
        };

        LeadDocument createdLead;
        try
        {
            createdLead = await _console.Client.Jsonapi.Leads.PostAsync(leadBody);
        }
        catch (ApiException ex) when (SampleConsole.IsForbidden(ex))
        {
            _console.WarnAdviserContactIdForbidden("create-lead");
            return;
        }

        Log.Information("POST /jsonapi/leads created lead id={LeadId}", createdLead?.Data?.Id);

        if (int.TryParse(createdLead?.Data?.Id, out var parsedDealId))
        {
            // A lead is a deal in the system, so we can set the last deal id to this new lead for use in other samples that require a deal id
            _console.UpdateState(_console.State with { LastDealId = parsedDealId });
        }

        var table = _console.CreateTable("Lead Id", "Status");
        table.AddRow(
            _console.Cell(createdLead?.Data?.Id),
            _console.Cell(createdLead?.Data?.Attributes?.DealStatus));

        _console.WriteTable(table);
    }

    [Sample("create-update-deal-note", "POST then PATCH /jsonapi/deal-notes")]
    public async Task RunCreateAndUpdateDealNoteSample()
    {
        if (!_console.RequireAdviserContactId("create-update-deal-note")) return;
        if (!_console.RequireScope("api.deal-notes.create", "create-update-deal-note")) return;

        if (!_console.State.TryGetDealId("create-update-deal-note", out var dealId))
        {
            return;
        }

        DealNoteDocument created;
        try
        {
            created = await _console.Client.Jsonapi.DealNotes.PostAsync(new DealNoteDocument
            {
                Data = new DealNote
                {
                    Type = "deal-notes",
                    Attributes = new DealNoteAttributes
                    {
                        Title = "A note on the last deal",
                        Detail = "These are some deal note details"
                    },
                    Relationships = new DealNoteRelationships
                    {
                        Deal = new RelationshipsSingleDocument
                        {
                            Data = new ResourceIdentifier
                            {
                                Type = "deals",
                                Id = dealId.ToString()
                            }
                        }
                    }
                }
            });
        }
        catch (ApiException ex) when (SampleConsole.IsForbidden(ex))
        {
            _console.WarnAdviserContactIdForbidden("create-update-deal-note");
            return;
        }

        if (created?.Data == null || !int.TryParse(created.Data.Id, out var dealNoteId))
        {
            Log.Warning("POST /jsonapi/deal-notes succeeded but no id was returned. Skipping PATCH example");
            return;
        }

        if (!_console.RequireScope("api.deal-notes.update", "create-update-deal-note")) return;

        Log.Information("POST /jsonapi/deal-notes created deal note id={DealNoteId}", dealNoteId);
    
        var updated = await _console.Client.Jsonapi.DealNotes[dealNoteId].PatchAsync(new DealNoteDocument
        {
            Data = new DealNote
            {
                Type = "deal-notes",
                Id = dealNoteId.ToString(),
                Attributes = new DealNoteAttributes
                {
                    Title = $"Title update for deal note {dealNoteId}",
                    Detail = $"Body update for deal note {dealNoteId}"
                }
            }
        });

        Log.Information(
            "PATCH /jsonapi/deal-notes/{DealNoteId} updated note title={Title}",
            dealNoteId,
            updated?.Data?.Attributes?.Title);

        var table = _console.CreateTable("Deal Id", "Deal Note Id", "Created Title", "Updated Title");
        table.AddRow(
            _console.Cell(dealId.ToString()),
            _console.Cell(updated?.Data?.Id ?? created.Data.Id),
            _console.Cell(created.Data.Attributes?.Title),
            _console.Cell(updated?.Data?.Attributes?.Title));

        _console.WriteTable(table);
    }
}
