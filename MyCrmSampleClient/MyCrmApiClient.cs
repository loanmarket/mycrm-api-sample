using Azure.Core.Pipeline;
using MyCrmSampleClient.MyCrmApi;

namespace MyCrmSampleClient
{
    internal class MyCrmApiClient
    {
        private HttpPipeline Pipeline { get; }
        private ClientDiagnostics ClientDiagnostics { get; }

        public ContactExternalReferenceRelatedClient ContactExternalReferenceRelated { get; }
        public ContactExternalReferenceRelationshipClient ContactExternalReferenceRelationship { get; }
        public ContactExternalReferenceClient ContactExternalReference { get; }
        public ContactExternalReferencesClient ContactExternalReferences { get; }
        public ContactGroupRelatedClient ContactGroupRelated { get; }
        public ContactGroupRelationshipClient ContactGroupRelationship { get; }
        public ContactGroupClient ContactGroup { get; }
        public ContactGroupsClient ContactGroups { get; }
        public ContactRelatedClient ContactRelated { get; }
        public ContactRelationshipClient ContactRelationship { get; }
        public ContactClient Contact { get; }
        public ContactsClient Contacts { get; }
        public DealExternalReferenceRelatedClient DealExternalReferenceRelated { get; }
        public DealExternalReferenceRelationshipClient DealExternalReferenceRelationship { get; }
        public DealExternalReferenceClient DealExternalReference { get; }
        public DealExternalReferencesClient DealExternalReferences { get; }
        public DealParticipantRelatedClient DealParticipantRelated { get; }
        public DealParticipantRelationshipClient DealParticipantRelationship { get; }
        public DealParticipantClient DealParticipant { get; }
        public DealParticipantsClient DealParticipants { get; }
        public DealRelatedClient DealRelated { get; }
        public DealRelationshipClient DealRelationship { get; }
        public DealClient Deal { get; }
        public DealsClient Deals { get; }
        public IntegrationRelatedClient IntegrationRelated { get; }
        public IntegrationRelationshipClient IntegrationRelationship { get; }
        public IntegrationClient Integration { get; }
        public IntegrationsClient Integrations { get; }

        public MyCrmApiClient(MyCrmApiClientCredential credential, MyCrmApiClientOptions options)
        {
            Pipeline = options.Build(credential, options);
            ClientDiagnostics = new ClientDiagnostics(options);
            ContactExternalReferenceRelated = new ContactExternalReferenceRelatedClient(ClientDiagnostics, Pipeline, options.Endpoint);
            ContactExternalReferenceRelationship = new ContactExternalReferenceRelationshipClient(ClientDiagnostics, Pipeline, options.Endpoint);
            ContactExternalReference = new ContactExternalReferenceClient(ClientDiagnostics, Pipeline, options.Endpoint);
            ContactExternalReferences = new ContactExternalReferencesClient(ClientDiagnostics, Pipeline, options.Endpoint);
            ContactGroupRelated = new ContactGroupRelatedClient(ClientDiagnostics, Pipeline, options.Endpoint);
            ContactGroupRelationship = new ContactGroupRelationshipClient(ClientDiagnostics, Pipeline, options.Endpoint);
            ContactGroup = new ContactGroupClient(ClientDiagnostics, Pipeline, options.Endpoint);
            ContactGroups = new ContactGroupsClient(ClientDiagnostics, Pipeline, options.Endpoint);
            ContactRelated = new ContactRelatedClient(ClientDiagnostics, Pipeline, options.Endpoint);
            ContactRelationship = new ContactRelationshipClient(ClientDiagnostics, Pipeline, options.Endpoint);
            Contact = new ContactClient(ClientDiagnostics, Pipeline, options.Endpoint);
            Contacts = new ContactsClient(ClientDiagnostics, Pipeline, options.Endpoint);
            DealExternalReferenceRelated = new DealExternalReferenceRelatedClient(ClientDiagnostics, Pipeline, options.Endpoint);
            DealExternalReferenceRelationship = new DealExternalReferenceRelationshipClient(ClientDiagnostics, Pipeline, options.Endpoint);
            DealExternalReference = new DealExternalReferenceClient(ClientDiagnostics, Pipeline, options.Endpoint);
            DealExternalReferences = new DealExternalReferencesClient(ClientDiagnostics, Pipeline, options.Endpoint);
            DealParticipantRelated = new DealParticipantRelatedClient(ClientDiagnostics, Pipeline, options.Endpoint);
            DealParticipantRelationship = new DealParticipantRelationshipClient(ClientDiagnostics, Pipeline, options.Endpoint);
            DealParticipant = new DealParticipantClient(ClientDiagnostics, Pipeline, options.Endpoint);
            DealParticipants = new DealParticipantsClient(ClientDiagnostics, Pipeline, options.Endpoint);
            DealRelated = new DealRelatedClient(ClientDiagnostics, Pipeline, options.Endpoint);
            DealRelationship = new DealRelationshipClient(ClientDiagnostics, Pipeline, options.Endpoint);
            Deal = new DealClient(ClientDiagnostics, Pipeline, options.Endpoint);
            Deals = new DealsClient(ClientDiagnostics, Pipeline, options.Endpoint);
            IntegrationRelated = new IntegrationRelatedClient(ClientDiagnostics, Pipeline, options.Endpoint);
            IntegrationRelationship = new IntegrationRelationshipClient(ClientDiagnostics, Pipeline, options.Endpoint);
            Integration = new IntegrationClient(ClientDiagnostics, Pipeline, options.Endpoint);
            Integrations = new IntegrationsClient(ClientDiagnostics, Pipeline, options.Endpoint);
        }
    }
}