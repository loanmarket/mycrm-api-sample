// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The ReferrerMemberRelationshipsContactGroups. </summary>
    public partial class ReferrerMemberRelationshipsContactGroups : RelationshipsMultipleDocument
    {
        /// <summary> Initializes a new instance of ReferrerMemberRelationshipsContactGroups. </summary>
        /// <param name="data"></param>
        public ReferrerMemberRelationshipsContactGroups(IEnumerable<ResourceIdentifier> data) : base(data)
        {
        }

        /// <summary> Initializes a new instance of ReferrerMemberRelationshipsContactGroups. </summary>
        /// <param name="links"></param>
        /// <param name="meta"> Dictionary of &lt;any&gt;. </param>
        /// <param name="data"></param>
        internal ReferrerMemberRelationshipsContactGroups(RelationshipsMultipleDocumentLinks links, IReadOnlyDictionary<string, object> meta, IList<ResourceIdentifier> data) : base(links, meta, data)
        {
        }
    }
}
