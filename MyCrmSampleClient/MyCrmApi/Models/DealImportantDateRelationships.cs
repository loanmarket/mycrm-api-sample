// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The DealImportantDateRelationships. </summary>
    public partial class DealImportantDateRelationships
    {
        /// <summary> Initializes a new instance of DealImportantDateRelationships. </summary>
        public DealImportantDateRelationships()
        {
        }

        /// <summary> Initializes a new instance of DealImportantDateRelationships. </summary>
        /// <param name="deal"></param>
        internal DealImportantDateRelationships(RelationshipsSingleDocument deal)
        {
            Deal = deal;
        }

        public RelationshipsSingleDocument Deal { get; set; }
    }
}
