// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The ContactGroup. </summary>
    public partial class ContactGroup
    {
        /// <summary> Initializes a new instance of ContactGroup. </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/> is null. </exception>
        public ContactGroup(ContactGroupsType type, string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Type = type;
            Id = id;
            Meta = new ChangeTrackingDictionary<string, object>();
        }

        /// <summary> Initializes a new instance of ContactGroup. </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="attributes"></param>
        /// <param name="relationships"></param>
        /// <param name="links"></param>
        /// <param name="meta"> Dictionary of &lt;any&gt;. </param>
        internal ContactGroup(ContactGroupsType type, string id, ContactGroupAttributes attributes, ContactGroupRelationships relationships, ContactGroupLinks links, IReadOnlyDictionary<string, object> meta)
        {
            Type = type;
            Id = id;
            Attributes = attributes;
            Relationships = relationships;
            Links = links;
            Meta = meta;
        }

        public ContactGroupsType Type { get; set; }
        public string Id { get; }
        public ContactGroupAttributes Attributes { get; set; }
        public ContactGroupRelationships Relationships { get; set; }
        public ContactGroupLinks Links { get; }
        /// <summary> Dictionary of &lt;any&gt;. </summary>
        public IReadOnlyDictionary<string, object> Meta { get; }
    }
}
