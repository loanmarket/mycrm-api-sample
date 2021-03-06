// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The StructuredLeadDocument. </summary>
    public partial class StructuredLeadDocument
    {
        /// <summary> Initializes a new instance of StructuredLeadDocument. </summary>
        /// <param name="data"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="data"/> is null. </exception>
        public StructuredLeadDocument(StructuredLead data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            Meta = new ChangeTrackingDictionary<string, object>();
            JsonApi = new ChangeTrackingDictionary<string, object>();
            Data = data;
            Included = new ChangeTrackingList<IncludedResource>();
        }

        /// <summary> Initializes a new instance of StructuredLeadDocument. </summary>
        /// <param name="meta"> Dictionary of &lt;any&gt;. </param>
        /// <param name="jsonApi"> Dictionary of &lt;any&gt;. </param>
        /// <param name="links"></param>
        /// <param name="data"></param>
        /// <param name="included"></param>
        internal StructuredLeadDocument(IReadOnlyDictionary<string, object> meta, IReadOnlyDictionary<string, object> jsonApi, StructuredLeadDocumentLinks links, StructuredLead data, IReadOnlyList<IncludedResource> included)
        {
            Meta = meta;
            JsonApi = jsonApi;
            Links = links;
            Data = data;
            Included = included;
        }

        /// <summary> Dictionary of &lt;any&gt;. </summary>
        public IReadOnlyDictionary<string, object> Meta { get; }
        /// <summary> Dictionary of &lt;any&gt;. </summary>
        public IReadOnlyDictionary<string, object> JsonApi { get; }
        /// <summary> Gets the links. </summary>
        public StructuredLeadDocumentLinks Links { get; }
        /// <summary> Gets or sets the data. </summary>
        public StructuredLead Data { get; set; }
        /// <summary> Gets the included. </summary>
        public IReadOnlyList<IncludedResource> Included { get; }
    }
}
