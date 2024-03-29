// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The AdviserRelationships. </summary>
    public partial class AdviserRelationships
    {
        /// <summary> Initializes a new instance of AdviserRelationships. </summary>
        public AdviserRelationships()
        {
        }

        /// <summary> Initializes a new instance of AdviserRelationships. </summary>
        /// <param name="adviserDetails"></param>
        /// <param name="organisation"></param>
        /// <param name="agreementHolders"></param>
        /// <param name="familyFranchisees"></param>
        /// <param name="addresses"></param>
        /// <param name="contactGroups"></param>
        internal AdviserRelationships(RelationshipsSingleDocument adviserDetails, RelationshipsSingleDocument organisation, RelationshipsMultipleDocument agreementHolders, RelationshipsMultipleDocument familyFranchisees, AdviserRelationshipsAddresses addresses, RelationshipsMultipleDocument contactGroups)
        {
            AdviserDetails = adviserDetails;
            Organisation = organisation;
            AgreementHolders = agreementHolders;
            FamilyFranchisees = familyFranchisees;
            Addresses = addresses;
            ContactGroups = contactGroups;
        }

        /// <summary> Gets or sets the adviser details. </summary>
        public RelationshipsSingleDocument AdviserDetails { get; set; }
        /// <summary> Gets or sets the organisation. </summary>
        public RelationshipsSingleDocument Organisation { get; set; }
        /// <summary> Gets or sets the agreement holders. </summary>
        public RelationshipsMultipleDocument AgreementHolders { get; set; }
        /// <summary> Gets or sets the family franchisees. </summary>
        public RelationshipsMultipleDocument FamilyFranchisees { get; set; }
        /// <summary> Gets or sets the addresses. </summary>
        public AdviserRelationshipsAddresses Addresses { get; set; }
        /// <summary> Gets or sets the contact groups. </summary>
        public RelationshipsMultipleDocument ContactGroups { get; set; }
    }
}
