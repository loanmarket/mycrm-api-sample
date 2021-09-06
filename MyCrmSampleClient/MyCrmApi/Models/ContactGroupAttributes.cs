// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The ContactGroupAttributes. </summary>
    public partial class ContactGroupAttributes
    {
        /// <summary> Initializes a new instance of ContactGroupAttributes. </summary>
        public ContactGroupAttributes()
        {
            Categories = new ChangeTrackingList<string>();
        }

        /// <summary> Initializes a new instance of ContactGroupAttributes. </summary>
        /// <param name="updated"></param>
        /// <param name="created"></param>
        /// <param name="utmSource"></param>
        /// <param name="utmMedium"></param>
        /// <param name="utmTerm"></param>
        /// <param name="utmContent"></param>
        /// <param name="utmCampaign"></param>
        /// <param name="categories"></param>
        /// <param name="notes"></param>
        internal ContactGroupAttributes(DateTimeOffset? updated, DateTimeOffset? created, string utmSource, string utmMedium, string utmTerm, string utmContent, string utmCampaign, IReadOnlyList<string> categories, string notes)
        {
            Updated = updated;
            Created = created;
            UtmSource = utmSource;
            UtmMedium = utmMedium;
            UtmTerm = utmTerm;
            UtmContent = utmContent;
            UtmCampaign = utmCampaign;
            Categories = categories;
            Notes = notes;
        }

        public DateTimeOffset? Updated { get; }
        public DateTimeOffset? Created { get; }
        public string UtmSource { get; set; }
        public string UtmMedium { get; set; }
        public string UtmTerm { get; set; }
        public string UtmContent { get; set; }
        public string UtmCampaign { get; set; }
        public IReadOnlyList<string> Categories { get; }
        public string Notes { get; set; }
    }
}
