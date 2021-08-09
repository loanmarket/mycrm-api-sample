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
        /// <param name="dealNotes"></param>
        /// <param name="participants"></param>
        /// <param name="externalReferences"></param>
        /// <param name="dealStructures"></param>
        /// <param name="dealStatus"></param>
        /// <param name="contacts"></param>
        /// <param name="importantDates"></param>
        internal DealRelationships(RelationshipsSingleDocument dealScenario, RelationshipsMultipleDocument dealNotes, RelationshipsMultipleDocument participants, RelationshipsMultipleDocument externalReferences, RelationshipsMultipleDocument dealStructures, RelationshipsSingleDocument dealStatus, DealRelationshipsContacts contacts, RelationshipsMultipleDocument importantDates)
        {
            DealScenario = dealScenario;
            DealNotes = dealNotes;
            Participants = participants;
            ExternalReferences = externalReferences;
            DealStructures = dealStructures;
            DealStatus = dealStatus;
            Contacts = contacts;
            ImportantDates = importantDates;
        }

        public RelationshipsSingleDocument DealScenario { get; set; }
        public RelationshipsMultipleDocument DealNotes { get; set; }
        public RelationshipsMultipleDocument Participants { get; set; }
        public RelationshipsMultipleDocument ExternalReferences { get; set; }
        public RelationshipsMultipleDocument DealStructures { get; set; }
        public RelationshipsSingleDocument DealStatus { get; set; }
        public DealRelationshipsContacts Contacts { get; set; }
        public RelationshipsMultipleDocument ImportantDates { get; set; }
    }
}
