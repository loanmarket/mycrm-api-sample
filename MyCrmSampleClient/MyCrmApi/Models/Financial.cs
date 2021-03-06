// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The Financial. </summary>
    public partial class Financial : IncludedResource
    {
        /// <summary> Initializes a new instance of Financial. </summary>
        public Financial()
        {
            Meta = new ChangeTrackingDictionary<string, object>();
            Type = "financials";
        }

        /// <summary> Initializes a new instance of Financial. </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="attributes"> Any object. </param>
        /// <param name="relationships"></param>
        /// <param name="links"></param>
        /// <param name="meta"> Dictionary of &lt;any&gt;. </param>
        internal Financial(string type, string id, object attributes, FinancialRelationships relationships, FinancialLinks links, IReadOnlyDictionary<string, object> meta) : base(type, id)
        {
            Attributes = attributes;
            Relationships = relationships;
            Links = links;
            Meta = meta;
            Type = type ?? "financials";
        }

        /// <summary> Any object. </summary>
        public object Attributes { get; set; }
        /// <summary> Gets or sets the relationships. </summary>
        public FinancialRelationships Relationships { get; set; }
        /// <summary> Gets the links. </summary>
        public FinancialLinks Links { get; }
        /// <summary> Dictionary of &lt;any&gt;. </summary>
        public IReadOnlyDictionary<string, object> Meta { get; }
    }
}
