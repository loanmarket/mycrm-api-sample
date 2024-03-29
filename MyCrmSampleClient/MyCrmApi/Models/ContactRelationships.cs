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
        /// <param name="ownership"></param>
        /// <param name="externalReferences"></param>
        /// <param name="deals"></param>
        /// <param name="employments"></param>
        /// <param name="contactAddress"></param>
        internal ContactRelationships(RelationshipsSingleDocument contactGroup, RelationshipsMultipleDocument ownership, RelationshipsMultipleDocument externalReferences, RelationshipsMultipleDocument deals, ContactRelationshipsEmployments employments, ContactRelationshipsContactAddress contactAddress)
        {
            ContactGroup = contactGroup;
            Ownership = ownership;
            ExternalReferences = externalReferences;
            Deals = deals;
            Employments = employments;
            ContactAddress = contactAddress;
        }

        /// <summary> Gets or sets the contact group. </summary>
        public RelationshipsSingleDocument ContactGroup { get; set; }
        /// <summary> Gets or sets the ownership. </summary>
        public RelationshipsMultipleDocument Ownership { get; set; }
        /// <summary> Gets or sets the external references. </summary>
        public RelationshipsMultipleDocument ExternalReferences { get; set; }
        /// <summary> Gets or sets the deals. </summary>
        public RelationshipsMultipleDocument Deals { get; set; }
        /// <summary> Gets or sets the employments. </summary>
        public ContactRelationshipsEmployments Employments { get; set; }
        /// <summary> Gets or sets the contact address. </summary>
        public ContactRelationshipsContactAddress ContactAddress { get; set; }
    }
}
