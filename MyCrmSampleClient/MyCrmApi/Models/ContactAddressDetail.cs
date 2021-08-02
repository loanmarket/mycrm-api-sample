// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The ContactAddressDetail. </summary>
    internal partial class ContactAddressDetail
    {
        /// <summary> Initializes a new instance of ContactAddressDetail. </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/> is null. </exception>
        internal ContactAddressDetail(ContactAddressDetailsType type, string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Type = type;
            Id = id;
            Meta = new ChangeTrackingDictionary<string, object>();
        }

        public ContactAddressDetailsType Type { get; }
        public string Id { get; }
        public ContactAddressDetailAttributes Attributes { get; }
        public ContactAddressDetailRelationships Relationships { get; }
        public ContactAddressDetailLinks Links { get; }
        /// <summary> Dictionary of &lt;any&gt;. </summary>
        public IReadOnlyDictionary<string, object> Meta { get; }
    }
}
