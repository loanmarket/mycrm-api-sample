// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The ReferrerRelationships. </summary>
    public partial class ReferrerRelationships
    {
        /// <summary> Initializes a new instance of ReferrerRelationships. </summary>
        public ReferrerRelationships()
        {
        }

        /// <summary> Initializes a new instance of ReferrerRelationships. </summary>
        /// <param name="contactGroups"></param>
        internal ReferrerRelationships(RelationshipsMultipleDocument contactGroups)
        {
            ContactGroups = contactGroups;
        }

        public RelationshipsMultipleDocument ContactGroups { get; set; }
    }
}
