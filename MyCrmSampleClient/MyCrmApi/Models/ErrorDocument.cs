// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The ErrorDocument. </summary>
    internal partial class ErrorDocument
    {
        /// <summary> Initializes a new instance of ErrorDocument. </summary>
        internal ErrorDocument()
        {
            Errors = new ChangeTrackingList<Error>();
        }

        /// <summary> Initializes a new instance of ErrorDocument. </summary>
        /// <param name="errors"></param>
        internal ErrorDocument(IReadOnlyList<Error> errors)
        {
            Errors = errors;
        }

        public IReadOnlyList<Error> Errors { get; }
    }
}
