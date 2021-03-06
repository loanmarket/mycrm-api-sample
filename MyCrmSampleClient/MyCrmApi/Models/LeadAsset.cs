// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The LeadAsset. </summary>
    public partial class LeadAsset
    {
        /// <summary> Initializes a new instance of LeadAsset. </summary>
        public LeadAsset()
        {
            Ownership = new ChangeTrackingList<LeadContactReference>();
        }

        /// <summary> Initializes a new instance of LeadAsset. </summary>
        /// <param name="lid"></param>
        /// <param name="assetTypeId"></param>
        /// <param name="assetSubTypeId"></param>
        /// <param name="description"></param>
        /// <param name="value"></param>
        /// <param name="institution"></param>
        /// <param name="accountName"></param>
        /// <param name="bsb"></param>
        /// <param name="accountNumber"></param>
        /// <param name="vehicleMake"></param>
        /// <param name="vehicleYear"></param>
        /// <param name="propertyZoningId"></param>
        /// <param name="propertyTypeId"></param>
        /// <param name="propertyPrimaryPurpose"></param>
        /// <param name="valueBasis"></param>
        /// <param name="address"></param>
        /// <param name="ownership"></param>
        internal LeadAsset(string lid, int? assetTypeId, int? assetSubTypeId, string description, double? value, string institution, string accountName, string bsb, string accountNumber, string vehicleMake, int? vehicleYear, int? propertyZoningId, int? propertyTypeId, PropertyPrimaryPurpose? propertyPrimaryPurpose, PropertyValueEstimatedBasis? valueBasis, LeadAddressReference address, IList<LeadContactReference> ownership)
        {
            Lid = lid;
            AssetTypeId = assetTypeId;
            AssetSubTypeId = assetSubTypeId;
            Description = description;
            Value = value;
            Institution = institution;
            AccountName = accountName;
            Bsb = bsb;
            AccountNumber = accountNumber;
            VehicleMake = vehicleMake;
            VehicleYear = vehicleYear;
            PropertyZoningId = propertyZoningId;
            PropertyTypeId = propertyTypeId;
            PropertyPrimaryPurpose = propertyPrimaryPurpose;
            ValueBasis = valueBasis;
            Address = address;
            Ownership = ownership;
        }

        /// <summary> Gets or sets the lid. </summary>
        public string Lid { get; set; }
        /// <summary> Gets or sets the asset type id. </summary>
        public int? AssetTypeId { get; set; }
        /// <summary> Gets or sets the asset sub type id. </summary>
        public int? AssetSubTypeId { get; set; }
        /// <summary> Gets or sets the description. </summary>
        public string Description { get; set; }
        /// <summary> Gets or sets the value. </summary>
        public double? Value { get; set; }
        /// <summary> Gets or sets the institution. </summary>
        public string Institution { get; set; }
        /// <summary> Gets or sets the account name. </summary>
        public string AccountName { get; set; }
        /// <summary> Gets or sets the bsb. </summary>
        public string Bsb { get; set; }
        /// <summary> Gets or sets the account number. </summary>
        public string AccountNumber { get; set; }
        /// <summary> Gets or sets the vehicle make. </summary>
        public string VehicleMake { get; set; }
        /// <summary> Gets or sets the vehicle year. </summary>
        public int? VehicleYear { get; set; }
        /// <summary> Gets or sets the property zoning id. </summary>
        public int? PropertyZoningId { get; set; }
        /// <summary> Gets or sets the property type id. </summary>
        public int? PropertyTypeId { get; set; }
        /// <summary> Gets or sets the property primary purpose. </summary>
        public PropertyPrimaryPurpose? PropertyPrimaryPurpose { get; set; }
        /// <summary> Gets or sets the value basis. </summary>
        public PropertyValueEstimatedBasis? ValueBasis { get; set; }
        /// <summary> Gets or sets the address. </summary>
        public LeadAddressReference Address { get; set; }
        /// <summary> Gets or sets the ownership. </summary>
        public IList<LeadContactReference> Ownership { get; set; }
    }
}
