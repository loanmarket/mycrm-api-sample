// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The DealParticipant. </summary>
    public partial class DealParticipant : IncludedResource
    {
        /// <summary> Initializes a new instance of DealParticipant. </summary>
        /// <param name="id"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/> is null. </exception>
        public DealParticipant(string id) : base(id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Meta = new ChangeTrackingDictionary<string, object>();
            Type = "deal-participants";
        }

        /// <summary> Initializes a new instance of DealParticipant. </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <param name="attributes"></param>
        /// <param name="relationships"></param>
        /// <param name="links"></param>
        /// <param name="meta"> Dictionary of &lt;any&gt;. </param>
        internal DealParticipant(string type, string id, DealParticipantAttributes attributes, DealParticipantRelationships relationships, DealParticipantLinks links, IReadOnlyDictionary<string, object> meta) : base(type, id)
        {
            Attributes = attributes;
            Relationships = relationships;
            Links = links;
            Meta = meta;
            Type = type ?? "deal-participants";
        }

        public DealParticipantAttributes Attributes { get; set; }
        public DealParticipantRelationships Relationships { get; set; }
        public DealParticipantLinks Links { get; }
        /// <summary> Dictionary of &lt;any&gt;. </summary>
        public IReadOnlyDictionary<string, object> Meta { get; }
    }
}
