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
    /// <summary> The AgreementHoldersDocument. </summary>
    internal partial class AgreementHoldersDocument
    {
        /// <summary> Initializes a new instance of AgreementHoldersDocument. </summary>
        /// <param name="data"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="data"/> is null. </exception>
        internal AgreementHoldersDocument(IEnumerable<AgreementHolder> data)
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

        /// <summary> Dictionary of &lt;any&gt;. </summary>
        public IReadOnlyDictionary<string, object> Meta { get; }
        /// <summary> Dictionary of &lt;any&gt;. </summary>
        public IReadOnlyDictionary<string, object> JsonApi { get; }
        /// <summary> Gets the links. </summary>
        public AgreementHoldersDocumentLinks Links { get; }
        /// <summary> Gets the data. </summary>
        public IReadOnlyList<AgreementHolder> Data { get; }
        /// <summary> Gets the included. </summary>
        public IReadOnlyList<IncludedResource> Included { get; }
    }
}