using Azure.Core.Pipeline;
using MyCrmSampleClient.MyCrmApi;

namespace MyCrmSampleClient
{
    // ls -1 *Client.cs | grep -v RestClient | sed 's/^\(.*\)\.cs$/        public \1Client \1 { get; }/'
    // ls -1 *Client.cs | grep -v RestClient | sed 's/^\(.*\)\.cs$/            \1 = new \1Client(ClientDiagnostics, Pipeline, options.Endpoint);/'
    internal class MyCrmApiClient
    {
        private HttpPipeline Pipeline { get; }
        private ClientDiagnostics ClientDiagnostics { get; }

        public AdviserClient Adviser { get; }
        public AdviserDetailClient AdviserDetail { get; }
        public AdviserDetailRelatedClient AdviserDetailRelated { get; }
        public AdviserDetailRelationshipClient AdviserDetailRelationship { get; }
        public AdviserDetailsClient AdviserDetails { get; }
        public AdviserRelatedClient AdviserRelated { get; }
        public AdviserRelationshipClient AdviserRelationship { get; }
        public AdvisersClient Advisers { get; }
        public ContactClient Contact { get; }
        public ContactExternalReferenceClient ContactExternalReference { get; }
        public ContactExternalReferenceRelatedClient ContactExternalReferenceRelated { get; }
        public ContactExternalReferenceRelationshipClient ContactExternalReferenceRelationship { get; }
        public ContactExternalReferencesClient ContactExternalReferences { get; }
        public ContactGroupClient ContactGroup { get; }
        public ContactGroupReferrerClient ContactGroupReferrer { get; }
        public ContactGroupReferrerRelatedClient ContactGroupReferrerRelated { get; }
        public ContactGroupReferrerRelationshipClient ContactGroupReferrerRelationship { get; }
        public ContactGroupReferrersClient ContactGroupReferrers { get; }
        public ContactGroupRelatedClient ContactGroupRelated { get; }
        public ContactGroupRelationshipClient ContactGroupRelationship { get; }
        public ContactGroupsClient ContactGroups { get; }
        public ContactMarketingClient ContactMarketing { get; }
        public ContactRelatedClient ContactRelated { get; }
        public ContactRelationshipClient ContactRelationship { get; }
        public ContactsClient Contacts { get; }
        public DealClient Deal { get; }
        public DealExternalReferenceClient DealExternalReference { get; }
        public DealExternalReferenceRelatedClient DealExternalReferenceRelated { get; }
        public DealExternalReferenceRelationshipClient DealExternalReferenceRelationship { get; }
        public DealExternalReferencesClient DealExternalReferences { get; }
        public DealNoteClient DealNote { get; }
        public DealNoteRelatedClient DealNoteRelated { get; }
        public DealNoteRelationshipClient DealNoteRelationship { get; }
        public DealNotesClient DealNotes { get; }
        public DealParticipantClient DealParticipant { get; }
        public DealParticipantRelatedClient DealParticipantRelated { get; }
        public DealParticipantRelationshipClient DealParticipantRelationship { get; }
        public DealParticipantsClient DealParticipants { get; }
        public DealRelatedClient DealRelated { get; }
        public DealRelationshipClient DealRelationship { get; }
        public DealsClient Deals { get; }
        public IntegrationClient Integration { get; }
        public IntegrationRelatedClient IntegrationRelated { get; }
        public IntegrationRelationshipClient IntegrationRelationship { get; }
        public IntegrationsClient Integrations { get; }
        public LeadClient Lead { get; }

        public MyCrmApiClient(MyCrmApiClientCredential credential, MyCrmApiClientOptions options)
        {
            Pipeline = options.Build(credential, options);
            ClientDiagnostics = new ClientDiagnostics(options);
            Adviser = new AdviserClient(ClientDiagnostics, Pipeline, options.Endpoint);
            AdviserDetail = new AdviserDetailClient(ClientDiagnostics, Pipeline, options.Endpoint);
            AdviserDetailRelated = new AdviserDetailRelatedClient(ClientDiagnostics, Pipeline, options.Endpoint);
            AdviserDetailRelationship =
                new AdviserDetailRelationshipClient(ClientDiagnostics, Pipeline, options.Endpoint);
            AdviserDetails = new AdviserDetailsClient(ClientDiagnostics, Pipeline, options.Endpoint);
            AdviserRelated = new AdviserRelatedClient(ClientDiagnostics, Pipeline, options.Endpoint);
            AdviserRelationship = new AdviserRelationshipClient(ClientDiagnostics, Pipeline, options.Endpoint);
            Advisers = new AdvisersClient(ClientDiagnostics, Pipeline, options.Endpoint);
            Contact = new ContactClient(ClientDiagnostics, Pipeline, options.Endpoint);
            ContactExternalReference =
                new ContactExternalReferenceClient(ClientDiagnostics, Pipeline, options.Endpoint);
            ContactExternalReferenceRelated =
                new ContactExternalReferenceRelatedClient(ClientDiagnostics, Pipeline, options.Endpoint);
            ContactExternalReferenceRelationship =
                new ContactExternalReferenceRelationshipClient(ClientDiagnostics, Pipeline, options.Endpoint);
            ContactExternalReferences =
                new ContactExternalReferencesClient(ClientDiagnostics, Pipeline, options.Endpoint);
            ContactGroup = new ContactGroupClient(ClientDiagnostics, Pipeline, options.Endpoint);
            ContactGroupReferrer = new ContactGroupReferrerClient(ClientDiagnostics, Pipeline, options.Endpoint);
            ContactGroupReferrerRelated =
                new ContactGroupReferrerRelatedClient(ClientDiagnostics, Pipeline, options.Endpoint);
            ContactGroupReferrerRelationship =
                new ContactGroupReferrerRelationshipClient(ClientDiagnostics, Pipeline, options.Endpoint);
            ContactGroupReferrers = new ContactGroupReferrersClient(ClientDiagnostics, Pipeline, options.Endpoint);
            ContactGroupRelated = new ContactGroupRelatedClient(ClientDiagnostics, Pipeline, options.Endpoint);
            ContactGroupRelationship =
                new ContactGroupRelationshipClient(ClientDiagnostics, Pipeline, options.Endpoint);
            ContactGroups = new ContactGroupsClient(ClientDiagnostics, Pipeline, options.Endpoint);
            ContactMarketing = new ContactMarketingClient(ClientDiagnostics, Pipeline, options.Endpoint);
            ContactRelated = new ContactRelatedClient(ClientDiagnostics, Pipeline, options.Endpoint);
            ContactRelationship = new ContactRelationshipClient(ClientDiagnostics, Pipeline, options.Endpoint);
            Contacts = new ContactsClient(ClientDiagnostics, Pipeline, options.Endpoint);
            Deal = new DealClient(ClientDiagnostics, Pipeline, options.Endpoint);
            DealExternalReference = new DealExternalReferenceClient(ClientDiagnostics, Pipeline, options.Endpoint);
            DealExternalReferenceRelated =
                new DealExternalReferenceRelatedClient(ClientDiagnostics, Pipeline, options.Endpoint);
            DealExternalReferenceRelationship =
                new DealExternalReferenceRelationshipClient(ClientDiagnostics, Pipeline, options.Endpoint);
            DealExternalReferences = new DealExternalReferencesClient(ClientDiagnostics, Pipeline, options.Endpoint);
            DealNote = new DealNoteClient(ClientDiagnostics, Pipeline, options.Endpoint);
            DealNoteRelated = new DealNoteRelatedClient(ClientDiagnostics, Pipeline, options.Endpoint);
            DealNoteRelationship = new DealNoteRelationshipClient(ClientDiagnostics, Pipeline, options.Endpoint);
            DealNotes = new DealNotesClient(ClientDiagnostics, Pipeline, options.Endpoint);
            DealParticipant = new DealParticipantClient(ClientDiagnostics, Pipeline, options.Endpoint);
            DealParticipantRelated = new DealParticipantRelatedClient(ClientDiagnostics, Pipeline, options.Endpoint);
            DealParticipantRelationship =
                new DealParticipantRelationshipClient(ClientDiagnostics, Pipeline, options.Endpoint);
            DealParticipants = new DealParticipantsClient(ClientDiagnostics, Pipeline, options.Endpoint);
            DealRelated = new DealRelatedClient(ClientDiagnostics, Pipeline, options.Endpoint);
            DealRelationship = new DealRelationshipClient(ClientDiagnostics, Pipeline, options.Endpoint);
            Deals = new DealsClient(ClientDiagnostics, Pipeline, options.Endpoint);
            Integration = new IntegrationClient(ClientDiagnostics, Pipeline, options.Endpoint);
            IntegrationRelated = new IntegrationRelatedClient(ClientDiagnostics, Pipeline, options.Endpoint);
            IntegrationRelationship = new IntegrationRelationshipClient(ClientDiagnostics, Pipeline, options.Endpoint);
            Integrations = new IntegrationsClient(ClientDiagnostics, Pipeline, options.Endpoint);
            Lead = new LeadClient(ClientDiagnostics, Pipeline, options.Endpoint);
        }
    }
}