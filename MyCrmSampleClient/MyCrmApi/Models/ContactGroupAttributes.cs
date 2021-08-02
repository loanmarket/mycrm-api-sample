// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The ContactGroupAttributes. </summary>
    public partial class ContactGroupAttributes
    {
        /// <summary> Initializes a new instance of ContactGroupAttributes. </summary>
        public ContactGroupAttributes()
        {
        }

        /// <summary> Initializes a new instance of ContactGroupAttributes. </summary>
        /// <param name="name"></param>
        /// <param name="created"></param>
        /// <param name="updated"></param>
        /// <param name="utmSource"></param>
        /// <param name="utmMedium"></param>
        /// <param name="utmTerm"></param>
        /// <param name="utmContent"></param>
        /// <param name="utmCampaign"></param>
        internal ContactGroupAttributes(string name, DateTimeOffset? created, DateTimeOffset? updated, string utmSource, string utmMedium, string utmTerm, string utmContent, string utmCampaign)
        {
            Name = name;
            Created = created;
            Updated = updated;
            UtmSource = utmSource;
            UtmMedium = utmMedium;
            UtmTerm = utmTerm;
            UtmContent = utmContent;
            UtmCampaign = utmCampaign;
        }

        public string Name { get; set; }
        public DateTimeOffset? Created { get; }
        public DateTimeOffset? Updated { get; }
        public string UtmSource { get; set; }
        public string UtmMedium { get; set; }
        public string UtmTerm { get; set; }
        public string UtmContent { get; set; }
        public string UtmCampaign { get; set; }
    }
}