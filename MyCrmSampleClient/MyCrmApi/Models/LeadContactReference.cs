// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The LeadContactReference. </summary>
    public partial class LeadContactReference
    {
        /// <summary> Initializes a new instance of LeadContactReference. </summary>
        /// <param name="lid"></param>
        /// <exception cref="ArgumentNullException"> <paramref name="lid"/> is null. </exception>
        public LeadContactReference(string lid)
        {
            if (lid == null)
            {
                throw new ArgumentNullException(nameof(lid));
            }

            Lid = lid;
        }

        /// <summary> Gets or sets the lid. </summary>
        public string Lid { get; set; }
    }
}