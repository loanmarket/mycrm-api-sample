// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The ContactMarketing. </summary>
    public partial class ContactMarketing : IncludedResource
    {
        /// <summary> Initializes a new instance of ContactMarketing. </summary>
        public ContactMarketing()
        {
            Meta = new ChangeTrackingDictionary<string, object>();
            Type = "contact-marketing";
        }

        /// <summary> Initializes a new instance of ContactMarketing. </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="attributes"></param>
        /// <param name="relationships"> Any object. </param>
        /// <param name="links"></param>
        /// <param name="meta"> Dictionary of &lt;any&gt;. </param>
        internal ContactMarketing(string type, string id, ContactMarketingAttributes attributes, object relationships, ContactMarketingLinks links, IReadOnlyDictionary<string, object> meta) : base(type, id)
        {
            Attributes = attributes;
            Relationships = relationships;
            Links = links;
            Meta = meta;
            Type = type ?? "contact-marketing";
        }

        /// <summary> Gets or sets the attributes. </summary>
        public ContactMarketingAttributes Attributes { get; set; }
        /// <summary> Any object. </summary>
        public object Relationships { get; set; }
        /// <summary> Gets the links. </summary>
        public ContactMarketingLinks Links { get; }
        /// <summary> Dictionary of &lt;any&gt;. </summary>
        public IReadOnlyDictionary<string, object> Meta { get; }
    }
}
