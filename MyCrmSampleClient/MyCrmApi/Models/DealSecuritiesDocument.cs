// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The DealSecuritiesDocument. </summary>
    public partial class DealSecuritiesDocument
    {
        /// <summary> Initializes a new instance of DealSecuritiesDocument. </summary>
        /// <param name="data"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="data"/> is null. </exception>
        internal DealSecuritiesDocument(IEnumerable<DealSecurity> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            Meta = new ChangeTrackingDictionary<string, object>();
            JsonApi = new ChangeTrackingDictionary<string, object>();
            Data = data.ToList();
            Included = new ChangeTrackingList<IncludedResource>();
        }

        /// <summary> Initializes a new instance of DealSecuritiesDocument. </summary>
        /// <param name="meta"> Dictionary of &lt;any&gt;. </param>
        /// <param name="jsonApi"> Dictionary of &lt;any&gt;. </param>
        /// <param name="links"></param>
        /// <param name="data"></param>
        /// <param name="included"></param>
        internal DealSecuritiesDocument(IReadOnlyDictionary<string, object> meta, IReadOnlyDictionary<string, object> jsonApi, DealSecuritiesDocumentLinks links, IReadOnlyList<DealSecurity> data, IReadOnlyList<IncludedResource> included)
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
        public DealSecuritiesDocumentLinks Links { get; }
        /// <summary> Gets the data. </summary>
        public IReadOnlyList<DealSecurity> Data { get; }
        /// <summary> Gets the included. </summary>
        public IReadOnlyList<IncludedResource> Included { get; }
    }
}
