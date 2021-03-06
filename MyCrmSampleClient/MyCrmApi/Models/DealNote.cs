// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The DealNote. </summary>
    public partial class DealNote : IncludedResource
    {
        /// <summary> Initializes a new instance of DealNote. </summary>
        public DealNote()
        {
            Meta = new ChangeTrackingDictionary<string, object>();
            Type = "deal-notes";
        }

        /// <summary> Initializes a new instance of DealNote. </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="attributes"></param>
        /// <param name="relationships"></param>
        /// <param name="links"></param>
        /// <param name="meta"> Dictionary of &lt;any&gt;. </param>
        internal DealNote(string type, string id, DealNoteAttributes attributes, DealNoteRelationships relationships, DealNoteLinks links, IReadOnlyDictionary<string, object> meta) : base(type, id)
        {
            Attributes = attributes;
            Relationships = relationships;
            Links = links;
            Meta = meta;
            Type = type ?? "deal-notes";
        }

        /// <summary> Gets or sets the attributes. </summary>
        public DealNoteAttributes Attributes { get; set; }
        /// <summary> Gets or sets the relationships. </summary>
        public DealNoteRelationships Relationships { get; set; }
        /// <summary> Gets the links. </summary>
        public DealNoteLinks Links { get; }
        /// <summary> Dictionary of &lt;any&gt;. </summary>
        public IReadOnlyDictionary<string, object> Meta { get; }
    }
}
