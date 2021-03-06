// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The DealExternalReference. </summary>
    public partial class DealExternalReference : IncludedResource
    {
        /// <summary> Initializes a new instance of DealExternalReference. </summary>
        public DealExternalReference()
        {
            Meta = new ChangeTrackingDictionary<string, object>();
            Type = "deal-external-references";
        }

        /// <summary> Initializes a new instance of DealExternalReference. </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="attributes"></param>
        /// <param name="relationships"></param>
        /// <param name="links"></param>
        /// <param name="meta"> Dictionary of &lt;any&gt;. </param>
        internal DealExternalReference(string type, string id, DealExternalReferenceAttributes attributes, DealExternalReferenceRelationships relationships, DealExternalReferenceLinks links, IReadOnlyDictionary<string, object> meta) : base(type, id)
        {
            Attributes = attributes;
            Relationships = relationships;
            Links = links;
            Meta = meta;
            Type = type ?? "deal-external-references";
        }

        /// <summary> Gets or sets the attributes. </summary>
        public DealExternalReferenceAttributes Attributes { get; set; }
        /// <summary> Gets or sets the relationships. </summary>
        public DealExternalReferenceRelationships Relationships { get; set; }
        /// <summary> Gets the links. </summary>
        public DealExternalReferenceLinks Links { get; }
        /// <summary> Dictionary of &lt;any&gt;. </summary>
        public IReadOnlyDictionary<string, object> Meta { get; }
    }
}
