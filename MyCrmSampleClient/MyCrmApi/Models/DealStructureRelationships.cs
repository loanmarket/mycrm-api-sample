// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The DealStructureRelationships. </summary>
    public partial class DealStructureRelationships
    {
        /// <summary> Initializes a new instance of DealStructureRelationships. </summary>
        public DealStructureRelationships()
        {
        }

        /// <summary> Initializes a new instance of DealStructureRelationships. </summary>
        /// <param name="dealImportantDates"></param>
        /// <param name="deal"></param>
        internal DealStructureRelationships(RelationshipsMultipleDocument dealImportantDates, RelationshipsSingleDocument deal)
        {
            DealImportantDates = dealImportantDates;
            Deal = deal;
        }

        /// <summary> Gets or sets the deal important dates. </summary>
        public RelationshipsMultipleDocument DealImportantDates { get; set; }
        /// <summary> Gets or sets the deal. </summary>
        public RelationshipsSingleDocument Deal { get; set; }
    }
}
