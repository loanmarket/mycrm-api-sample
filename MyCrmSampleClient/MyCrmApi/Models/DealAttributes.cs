// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The DealAttributes. </summary>
    public partial class DealAttributes
    {
        /// <summary> Initializes a new instance of DealAttributes. </summary>
        public DealAttributes()
        {
            Splits = new ChangeTrackingList<Split>();
        }

        /// <summary> Initializes a new instance of DealAttributes. </summary>
        /// <param name="updated"></param>
        /// <param name="created"></param>
        /// <param name="customStatusName"></param>
        /// <param name="dealType"></param>
        /// <param name="name"></param>
        /// <param name="dealStatus"></param>
        /// <param name="opportunity"></param>
        /// <param name="splits"></param>
        /// <param name="lenderName"></param>
        internal DealAttributes(DateTimeOffset? updated, DateTimeOffset? created, string customStatusName, DealType? dealType, string name, SystemStatus? dealStatus, Opportunity opportunity, IReadOnlyList<Split> splits, string lenderName)
        {
            Updated = updated;
            Created = created;
            CustomStatusName = customStatusName;
            DealType = dealType;
            Name = name;
            DealStatus = dealStatus;
            Opportunity = opportunity;
            Splits = splits;
            LenderName = lenderName;
        }

        public DateTimeOffset? Updated { get; }
        public DateTimeOffset? Created { get; }
        public string CustomStatusName { get; }
        public DealType? DealType { get; set; }
        public string Name { get; set; }
        public SystemStatus? DealStatus { get; set; }
        public Opportunity Opportunity { get; set; }
        public IReadOnlyList<Split> Splits { get; }
        public string LenderName { get; }
    }
}
