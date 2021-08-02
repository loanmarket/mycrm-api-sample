// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The DealExternalReferenceRelationships. </summary>
    public partial class DealExternalReferenceRelationships
    {
        /// <summary> Initializes a new instance of DealExternalReferenceRelationships. </summary>
        public DealExternalReferenceRelationships()
        {
        }

        /// <summary> Initializes a new instance of DealExternalReferenceRelationships. </summary>
        /// <param name="deal"></param>
        /// <param name="integration"></param>
        internal DealExternalReferenceRelationships(RelationshipsSingleDocument deal, RelationshipsSingleDocument integration)
        {
            Deal = deal;
            Integration = integration;
        }

        public RelationshipsSingleDocument Deal { get; set; }
        public RelationshipsSingleDocument Integration { get; set; }
    }
}
