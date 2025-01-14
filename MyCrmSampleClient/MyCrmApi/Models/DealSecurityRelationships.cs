// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The DealSecurityRelationships. </summary>
    public partial class DealSecurityRelationships
    {
        /// <summary> Initializes a new instance of DealSecurityRelationships. </summary>
        public DealSecurityRelationships()
        {
        }

        /// <summary> Initializes a new instance of DealSecurityRelationships. </summary>
        /// <param name="titleIdentifications"></param>
        /// <param name="address"></param>
        /// <param name="asset"></param>
        /// <param name="ownership"></param>
        internal DealSecurityRelationships(RelationshipsMultipleDocument titleIdentifications, DealSecurityRelationshipsAddress address, DealSecurityRelationshipsAsset asset, RelationshipsMultipleDocument ownership)
        {
            TitleIdentifications = titleIdentifications;
            Address = address;
            Asset = asset;
            Ownership = ownership;
        }

        /// <summary> Gets or sets the title identifications. </summary>
        public RelationshipsMultipleDocument TitleIdentifications { get; set; }
        /// <summary> Gets or sets the address. </summary>
        public DealSecurityRelationshipsAddress Address { get; set; }
        /// <summary> Gets or sets the asset. </summary>
        public DealSecurityRelationshipsAsset Asset { get; set; }
        /// <summary> Gets or sets the ownership. </summary>
        public RelationshipsMultipleDocument Ownership { get; set; }
    }
}
