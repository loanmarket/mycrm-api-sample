// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The IntegrationRelationships. </summary>
    public partial class IntegrationRelationships
    {
        /// <summary> Initializes a new instance of IntegrationRelationships. </summary>
        public IntegrationRelationships()
        {
        }

        /// <summary> Initializes a new instance of IntegrationRelationships. </summary>
        /// <param name="contactExternalReferences"></param>
        /// <param name="dealExternalReferences"></param>
        internal IntegrationRelationships(RelationshipsMultipleDocument contactExternalReferences, RelationshipsMultipleDocument dealExternalReferences)
        {
            ContactExternalReferences = contactExternalReferences;
            DealExternalReferences = dealExternalReferences;
        }

        public RelationshipsMultipleDocument ContactExternalReferences { get; set; }
        public RelationshipsMultipleDocument DealExternalReferences { get; set; }
    }
}