// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The ContactAddress. </summary>
    public partial class ContactAddress : IncludedResource
    {
        /// <summary> Initializes a new instance of ContactAddress. </summary>
        public ContactAddress()
        {
            Meta = new ChangeTrackingDictionary<string, object>();
            Type = "contact-address";
        }

        /// <summary> Initializes a new instance of ContactAddress. </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="attributes"></param>
        /// <param name="relationships"></param>
        /// <param name="links"></param>
        /// <param name="meta"> Dictionary of &lt;any&gt;. </param>
        internal ContactAddress(string type, string id, ContactAddressAttributes attributes, ContactAddressRelationships relationships, ContactAddressLinks links, IReadOnlyDictionary<string, object> meta) : base(type, id)
        {
            Attributes = attributes;
            Relationships = relationships;
            Links = links;
            Meta = meta;
            Type = type ?? "contact-address";
        }

        /// <summary> Gets or sets the attributes. </summary>
        public ContactAddressAttributes Attributes { get; set; }
        /// <summary> Gets or sets the relationships. </summary>
        public ContactAddressRelationships Relationships { get; set; }
        /// <summary> Gets the links. </summary>
        public ContactAddressLinks Links { get; }
        /// <summary> Dictionary of &lt;any&gt;. </summary>
        public IReadOnlyDictionary<string, object> Meta { get; }
    }
}
