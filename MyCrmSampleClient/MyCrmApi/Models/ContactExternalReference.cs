// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The ContactExternalReference. </summary>
    public partial class ContactExternalReference
    {
        /// <summary> Initializes a new instance of ContactExternalReference. </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/> is null. </exception>
        public ContactExternalReference(ContactExternalReferencesType type, string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Type = type;
            Id = id;
            Meta = new ChangeTrackingDictionary<string, object>();
        }

        /// <summary> Initializes a new instance of ContactExternalReference. </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="attributes"></param>
        /// <param name="relationships"></param>
        /// <param name="links"></param>
        /// <param name="meta"> Dictionary of &lt;any&gt;. </param>
        internal ContactExternalReference(ContactExternalReferencesType type, string id, ContactExternalReferenceAttributes attributes, ContactExternalReferenceRelationships relationships, ContactExternalReferenceLinks links, IReadOnlyDictionary<string, object> meta)
        {
            Type = type;
            Id = id;
            Attributes = attributes;
            Relationships = relationships;
            Links = links;
            Meta = meta;
        }

        public ContactExternalReferencesType Type { get; set; }
        public string Id { get; }
        public ContactExternalReferenceAttributes Attributes { get; set; }
        public ContactExternalReferenceRelationships Relationships { get; set; }
        public ContactExternalReferenceLinks Links { get; }
        /// <summary> Dictionary of &lt;any&gt;. </summary>
        public IReadOnlyDictionary<string, object> Meta { get; }
    }
}