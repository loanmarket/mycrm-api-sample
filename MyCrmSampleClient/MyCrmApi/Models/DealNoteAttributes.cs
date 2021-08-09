// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The DealNoteAttributes. </summary>
    public partial class DealNoteAttributes
    {
        /// <summary> Initializes a new instance of DealNoteAttributes. </summary>
        public DealNoteAttributes()
        {
        }

        /// <summary> Initializes a new instance of DealNoteAttributes. </summary>
        /// <param name="created"></param>
        /// <param name="updated"></param>
        internal DealNoteAttributes(DateTimeOffset? created, DateTimeOffset? updated)
        {
            Created = created;
            Updated = updated;
        }

        public DateTimeOffset? Created { get; }
        public DateTimeOffset? Updated { get; }
    }
}
