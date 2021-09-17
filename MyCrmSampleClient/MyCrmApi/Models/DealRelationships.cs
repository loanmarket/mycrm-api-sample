// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The DealRelationships. </summary>
    public partial class DealRelationships
    {
        /// <summary> Initializes a new instance of DealRelationships. </summary>
        public DealRelationships()
        {
        }

        /// <summary> Initializes a new instance of DealRelationships. </summary>
        /// <param name="dealScenario"></param>
        /// <param name="participants"></param>
        /// <param name="externalReferences"></param>
        /// <param name="dealStructures"></param>
        /// <param name="contacts"></param>
        /// <param name="importantDates"></param>
        /// <param name="adviser"></param>
        /// <param name="dealNotes"></param>
        internal DealRelationships(RelationshipsSingleDocument dealScenario, RelationshipsMultipleDocument participants, RelationshipsMultipleDocument externalReferences, RelationshipsMultipleDocument dealStructures, DealRelationshipsContacts contacts, RelationshipsMultipleDocument importantDates, RelationshipsSingleDocument adviser, RelationshipsMultipleDocument dealNotes)
        {
            DealScenario = dealScenario;
            Participants = participants;
            ExternalReferences = externalReferences;
            DealStructures = dealStructures;
            Contacts = contacts;
            ImportantDates = importantDates;
            Adviser = adviser;
            DealNotes = dealNotes;
        }

        /// <summary> Gets or sets the deal scenario. </summary>
        public RelationshipsSingleDocument DealScenario { get; set; }
        /// <summary> Gets or sets the participants. </summary>
        public RelationshipsMultipleDocument Participants { get; set; }
        /// <summary> Gets or sets the external references. </summary>
        public RelationshipsMultipleDocument ExternalReferences { get; set; }
        /// <summary> Gets or sets the deal structures. </summary>
        public RelationshipsMultipleDocument DealStructures { get; set; }
        /// <summary> Gets or sets the contacts. </summary>
        public DealRelationshipsContacts Contacts { get; set; }
        /// <summary> Gets or sets the important dates. </summary>
        public RelationshipsMultipleDocument ImportantDates { get; set; }
        /// <summary> Gets or sets the adviser. </summary>
        public RelationshipsSingleDocument Adviser { get; set; }
        /// <summary> Gets or sets the deal notes. </summary>
        public RelationshipsMultipleDocument DealNotes { get; set; }
    }
}
