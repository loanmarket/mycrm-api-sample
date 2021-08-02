// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The IntegrationDocument. </summary>
    public partial class IntegrationDocument
    {
        /// <summary> Initializes a new instance of IntegrationDocument. </summary>
        /// <param name="data"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="data"/> is null. </exception>
        public IntegrationDocument(Integration data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            Meta = new ChangeTrackingDictionary<string, object>();
            JsonApi = new ChangeTrackingDictionary<string, object>();
            Data = data;
        }

        /// <summary> Initializes a new instance of IntegrationDocument. </summary>
        /// <param name="meta"> Dictionary of &lt;any&gt;. </param>
        /// <param name="jsonApi"> Dictionary of &lt;any&gt;. </param>
        /// <param name="links"></param>
        /// <param name="data"></param>
        internal IntegrationDocument(IReadOnlyDictionary<string, object> meta, IReadOnlyDictionary<string, object> jsonApi, IntegrationDocumentLinks links, Integration data)
        {
            Meta = meta;
            JsonApi = jsonApi;
            Links = links;
            Data = data;
        }

        /// <summary> Dictionary of &lt;any&gt;. </summary>
        public IReadOnlyDictionary<string, object> Meta { get; }
        /// <summary> Dictionary of &lt;any&gt;. </summary>
        public IReadOnlyDictionary<string, object> JsonApi { get; }
        public IntegrationDocumentLinks Links { get; }
        public Integration Data { get; set; }
    }
}
