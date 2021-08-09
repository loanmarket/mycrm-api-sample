// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The DealNoteDocument. </summary>
    internal partial class DealNoteDocument
    {
        /// <summary> Initializes a new instance of DealNoteDocument. </summary>
        /// <param name="data"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="data"/> is null. </exception>
        internal DealNoteDocument(DealNote data)
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

        /// <summary> Dictionary of &lt;any&gt;. </summary>
        public IReadOnlyDictionary<string, object> Meta { get; }
        /// <summary> Dictionary of &lt;any&gt;. </summary>
        public IReadOnlyDictionary<string, object> JsonApi { get; }
        public DealNoteDocumentLinks Links { get; }
        public DealNote Data { get; }
        public IReadOnlyList<IncludedResource> Included { get; }
    }
}
