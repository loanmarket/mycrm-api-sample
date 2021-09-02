// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The ApiOrgPipelineStatus. </summary>
    internal partial class ApiOrgPipelineStatus
    {
        /// <summary> Initializes a new instance of ApiOrgPipelineStatus. </summary>
        /// <param name="id"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="id"/> is null. </exception>
        internal ApiOrgPipelineStatus(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            Id = id;
        }

        public string Id { get; }
    }
}