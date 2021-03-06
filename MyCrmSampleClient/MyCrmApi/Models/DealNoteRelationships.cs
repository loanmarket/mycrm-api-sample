// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The DealNoteRelationships. </summary>
    public partial class DealNoteRelationships
    {
        /// <summary> Initializes a new instance of DealNoteRelationships. </summary>
        public DealNoteRelationships()
        {
        }

        /// <summary> Initializes a new instance of DealNoteRelationships. </summary>
        /// <param name="deal"></param>
        internal DealNoteRelationships(RelationshipsSingleDocument deal)
        {
            Deal = deal;
        }

        /// <summary> Gets or sets the deal. </summary>
        public RelationshipsSingleDocument Deal { get; set; }
    }
}
