// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The DealStructureImportantDateRelationships. </summary>
    public partial class DealStructureImportantDateRelationships
    {
        /// <summary> Initializes a new instance of DealStructureImportantDateRelationships. </summary>
        public DealStructureImportantDateRelationships()
        {
        }

        /// <summary> Initializes a new instance of DealStructureImportantDateRelationships. </summary>
        /// <param name="dealStructure"></param>
        internal DealStructureImportantDateRelationships(DealStructureImportantDateRelationshipsDealStructure dealStructure)
        {
            DealStructure = dealStructure;
        }

        /// <summary> Gets or sets the deal structure. </summary>
        public DealStructureImportantDateRelationshipsDealStructure DealStructure { get; set; }
    }
}
