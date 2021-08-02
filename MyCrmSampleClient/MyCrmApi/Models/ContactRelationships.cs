// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The ContactRelationships. </summary>
    public partial class ContactRelationships
    {
        /// <summary> Initializes a new instance of ContactRelationships. </summary>
        public ContactRelationships()
        {
        }

        /// <summary> Initializes a new instance of ContactRelationships. </summary>
        /// <param name="contactGroup"></param>
        /// <param name="contactAddress"></param>
        /// <param name="externalReferences"></param>
        /// <param name="deals"></param>
        internal ContactRelationships(RelationshipsSingleDocument contactGroup, RelationshipsMultipleDocument contactAddress, RelationshipsMultipleDocument externalReferences, RelationshipsMultipleDocument deals)
        {
            ContactGroup = contactGroup;
            ContactAddress = contactAddress;
            ExternalReferences = externalReferences;
            Deals = deals;
        }

        public RelationshipsSingleDocument ContactGroup { get; set; }
        public RelationshipsMultipleDocument ContactAddress { get; set; }
        public RelationshipsMultipleDocument ExternalReferences { get; set; }
        public RelationshipsMultipleDocument Deals { get; set; }
    }
}