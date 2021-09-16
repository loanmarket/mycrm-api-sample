// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The OrganisationRelationships. </summary>
    public partial class OrganisationRelationships
    {
        /// <summary> Initializes a new instance of OrganisationRelationships. </summary>
        public OrganisationRelationships()
        {
        }

        /// <summary> Initializes a new instance of OrganisationRelationships. </summary>
        /// <param name="advisers"></param>
        /// <param name="contactCategories"></param>
        internal OrganisationRelationships(RelationshipsMultipleDocument advisers, RelationshipsMultipleDocument contactCategories)
        {
            Advisers = advisers;
            ContactCategories = contactCategories;
        }

        /// <summary> Gets or sets the advisers. </summary>
        public RelationshipsMultipleDocument Advisers { get; set; }
        /// <summary> Gets or sets the contact categories. </summary>
        public RelationshipsMultipleDocument ContactCategories { get; set; }
    }
}
