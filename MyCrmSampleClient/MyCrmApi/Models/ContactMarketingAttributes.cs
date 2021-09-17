// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The ContactMarketingAttributes. </summary>
    public partial class ContactMarketingAttributes
    {
        /// <summary> Initializes a new instance of ContactMarketingAttributes. </summary>
        public ContactMarketingAttributes()
        {
        }

        /// <summary> Initializes a new instance of ContactMarketingAttributes. </summary>
        /// <param name="updated"></param>
        /// <param name="hasMarketingConsent"></param>
        internal ContactMarketingAttributes(DateTimeOffset? updated, bool? hasMarketingConsent)
        {
            Updated = updated;
            HasMarketingConsent = hasMarketingConsent;
        }

        /// <summary> Gets the updated. </summary>
        public DateTimeOffset? Updated { get; }
        /// <summary> Gets or sets the has marketing consent. </summary>
        public bool? HasMarketingConsent { get; set; }
    }
}
