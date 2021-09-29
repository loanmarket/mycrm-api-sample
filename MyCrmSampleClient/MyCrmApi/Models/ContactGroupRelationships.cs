// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The ContactGroupRelationships. </summary>
    public partial class ContactGroupRelationships
    {
        /// <summary> Initializes a new instance of ContactGroupRelationships. </summary>
        public ContactGroupRelationships()
        {
        }

        /// <summary> Initializes a new instance of ContactGroupRelationships. </summary>
        /// <param name="contacts"></param>
        /// <param name="adviser"></param>
        /// <param name="referrerOrganisation"></param>
        /// <param name="referrer"></param>
        internal ContactGroupRelationships(RelationshipsMultipleDocument contacts, RelationshipsSingleDocument adviser, RelationshipsSingleDocument referrerOrganisation, RelationshipsSingleDocument referrer)
        {
            Contacts = contacts;
            Adviser = adviser;
            ReferrerOrganisation = referrerOrganisation;
            Referrer = referrer;
        }

        /// <summary> Gets or sets the contacts. </summary>
        public RelationshipsMultipleDocument Contacts { get; set; }
        /// <summary> Gets or sets the adviser. </summary>
        public RelationshipsSingleDocument Adviser { get; set; }
        /// <summary> Gets or sets the referrer organisation. </summary>
        public RelationshipsSingleDocument ReferrerOrganisation { get; set; }
        /// <summary> Gets or sets the referrer. </summary>
        public RelationshipsSingleDocument Referrer { get; set; }
    }
}
