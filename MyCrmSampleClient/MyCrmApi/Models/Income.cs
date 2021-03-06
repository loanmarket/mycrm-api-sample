// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The Income. </summary>
    public partial class Income : IncludedResource
    {
        /// <summary> Initializes a new instance of Income. </summary>
        public Income()
        {
            Meta = new ChangeTrackingDictionary<string, object>();
            Type = "incomes";
        }

        /// <summary> Initializes a new instance of Income. </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="attributes"></param>
        /// <param name="relationships"></param>
        /// <param name="links"></param>
        /// <param name="meta"> Dictionary of &lt;any&gt;. </param>
        internal Income(string type, string id, IncomeAttributes attributes, IncomeRelationships relationships, IncomeLinks links, IReadOnlyDictionary<string, object> meta) : base(type, id)
        {
            Attributes = attributes;
            Relationships = relationships;
            Links = links;
            Meta = meta;
            Type = type ?? "incomes";
        }

        /// <summary> Gets or sets the attributes. </summary>
        public IncomeAttributes Attributes { get; set; }
        /// <summary> Gets or sets the relationships. </summary>
        public IncomeRelationships Relationships { get; set; }
        /// <summary> Gets the links. </summary>
        public IncomeLinks Links { get; }
        /// <summary> Dictionary of &lt;any&gt;. </summary>
        public IReadOnlyDictionary<string, object> Meta { get; }
    }
}
