// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The AgreementHolderRelationships. </summary>
    public partial class AgreementHolderRelationships
    {
        /// <summary> Initializes a new instance of AgreementHolderRelationships. </summary>
        public AgreementHolderRelationships()
        {
        }

        /// <summary> Initializes a new instance of AgreementHolderRelationships. </summary>
        /// <param name="apiFamilyFranchisees"></param>
        /// <param name="organisation"></param>
        internal AgreementHolderRelationships(RelationshipsMultipleDocument apiFamilyFranchisees, AgreementHolderRelationshipsOrganisation organisation)
        {
            ApiFamilyFranchisees = apiFamilyFranchisees;
            Organisation = organisation;
        }

        /// <summary> Gets or sets the api family franchisees. </summary>
        public RelationshipsMultipleDocument ApiFamilyFranchisees { get; set; }
        /// <summary> Gets or sets the organisation. </summary>
        public AgreementHolderRelationshipsOrganisation Organisation { get; set; }
    }
}
