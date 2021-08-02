// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The DealParticipantAttributes. </summary>
    public partial class DealParticipantAttributes
    {
        /// <summary> Initializes a new instance of DealParticipantAttributes. </summary>
        public DealParticipantAttributes()
        {
        }

        /// <summary> Initializes a new instance of DealParticipantAttributes. </summary>
        /// <param name="isGuarantor"></param>
        /// <param name="created"></param>
        /// <param name="isAccountant"></param>
        /// <param name="isSolicitor"></param>
        /// <param name="isApplicant"></param>
        /// <param name="detail"></param>
        /// <param name="updated"></param>
        /// <param name="isDependent"></param>
        /// <param name="isAssetSupplier"></param>
        /// <param name="isClientSoleTrader"></param>
        internal DealParticipantAttributes(int? isGuarantor, DateTimeOffset? created, bool? isAccountant, bool? isSolicitor, bool? isApplicant, string detail, DateTimeOffset? updated, bool? isDependent, bool? isAssetSupplier, bool? isClientSoleTrader)
        {
            IsGuarantor = isGuarantor;
            Created = created;
            IsAccountant = isAccountant;
            IsSolicitor = isSolicitor;
            IsApplicant = isApplicant;
            Detail = detail;
            Updated = updated;
            IsDependent = isDependent;
            IsAssetSupplier = isAssetSupplier;
            IsClientSoleTrader = isClientSoleTrader;
        }

        public int? IsGuarantor { get; set; }
        public DateTimeOffset? Created { get; }
        public bool? IsAccountant { get; set; }
        public bool? IsSolicitor { get; set; }
        public bool? IsApplicant { get; set; }
        public string Detail { get; }
        public DateTimeOffset? Updated { get; }
        public bool? IsDependent { get; set; }
        public bool? IsAssetSupplier { get; set; }
        public bool? IsClientSoleTrader { get; set; }
    }
}