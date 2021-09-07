// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The Organisation. </summary>
    public partial class Organisation : IncludedResource
    {
        /// <summary> Initializes a new instance of Organisation. </summary>
        public Organisation()
        {
            Meta = new ChangeTrackingDictionary<string, object>();
            Type = "organisation";
        }

        /// <summary> Initializes a new instance of Organisation. </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="attributes"></param>
        /// <param name="relationships"></param>
        /// <param name="links"></param>
        /// <param name="meta"> Dictionary of &lt;any&gt;. </param>
        internal Organisation(string type, string id, OrganisationAttributes attributes, OrganisationRelationships relationships, OrganisationLinks links, IReadOnlyDictionary<string, object> meta) : base(type, id)
        {
            Attributes = attributes;
            Relationships = relationships;
            Links = links;
            Meta = meta;
            Type = type ?? "organisation";
        }

        public OrganisationAttributes Attributes { get; set; }
        public OrganisationRelationships Relationships { get; set; }
        public OrganisationLinks Links { get; }
        /// <summary> Dictionary of &lt;any&gt;. </summary>
        public IReadOnlyDictionary<string, object> Meta { get; }
    }
}
