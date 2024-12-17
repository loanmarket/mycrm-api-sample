// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The ContactGroupRelationshipsReferrerOrganisation. </summary>
    public partial class ContactGroupRelationshipsReferrerOrganisation : RelationshipsSingleDocument
    {
        /// <summary> Initializes a new instance of ContactGroupRelationshipsReferrerOrganisation. </summary>
        /// <param name="data"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="data"/> is null. </exception>
        public ContactGroupRelationshipsReferrerOrganisation(ResourceIdentifier data) : base(data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
        }

        /// <summary> Initializes a new instance of ContactGroupRelationshipsReferrerOrganisation. </summary>
        /// <param name="links"></param>
        /// <param name="meta"> Dictionary of &lt;any&gt;. </param>
        /// <param name="data"></param>
        internal ContactGroupRelationshipsReferrerOrganisation(RelationshipsSingleDocumentLinks links, IReadOnlyDictionary<string, object> meta, ResourceIdentifier data) : base(links, meta, data)
        {
        }
    }
}