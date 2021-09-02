// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The ContactExternalReference. </summary>
    public partial class ContactExternalReference : IncludedResource
    {
        /// <summary> Initializes a new instance of ContactExternalReference. </summary>
        public ContactExternalReference()
        {
            Meta = new ChangeTrackingDictionary<string, object>();
            Type = "contact-external-references";
        }

        /// <summary> Initializes a new instance of ContactExternalReference. </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="attributes"></param>
        /// <param name="relationships"></param>
        /// <param name="links"></param>
        /// <param name="meta"> Dictionary of &lt;any&gt;. </param>
        internal ContactExternalReference(string type, string id, ContactExternalReferenceAttributes attributes, ContactExternalReferenceRelationships relationships, ContactExternalReferenceLinks links, IReadOnlyDictionary<string, object> meta) : base(type, id)
        {
            Attributes = attributes;
            Relationships = relationships;
            Links = links;
            Meta = meta;
            Type = type ?? "contact-external-references";
        }

        public ContactExternalReferenceAttributes Attributes { get; set; }
        public ContactExternalReferenceRelationships Relationships { get; set; }
        public ContactExternalReferenceLinks Links { get; }
        /// <summary> Dictionary of &lt;any&gt;. </summary>
        public IReadOnlyDictionary<string, object> Meta { get; }
    }
}
