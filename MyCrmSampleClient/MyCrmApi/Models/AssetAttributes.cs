// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The AssetAttributes. </summary>
    public partial class AssetAttributes
    {
        /// <summary> Initializes a new instance of AssetAttributes. </summary>
        public AssetAttributes()
        {
        }

        /// <summary> Initializes a new instance of AssetAttributes. </summary>
        /// <param name="updated"></param>
        /// <param name="created"></param>
        /// <param name="description"></param>
        /// <param name="value"></param>
        /// <param name="accountName"></param>
        /// <param name="accountNumber"></param>
        /// <param name="bsb"></param>
        /// <param name="vehicleMake"></param>
        /// <param name="vehicleYear"></param>
        /// <param name="propertyType"></param>
        /// <param name="isRentalIncome"></param>
        /// <param name="isExistingMortgages"></param>
        /// <param name="assetTypeId"></param>
        /// <param name="isAccountDetailsKnown"></param>
        /// <param name="institution"></param>
        /// <param name="assetSubType"></param>
        /// <param name="assetSubTypeId"></param>
        /// <param name="assetType"></param>
        /// <param name="assetCategory"></param>
        /// <param name="currencyCode"></param>
        /// <param name="valuationBasis"></param>
        /// <param name="propertyZoningType"></param>
        /// <param name="addressPropertyType"></param>
        /// <param name="assetCategoryId"></param>
        internal AssetAttributes(DateTimeOffset? updated, DateTimeOffset? created, string description, double? value, string accountName, string accountNumber, string bsb, string vehicleMake, int? vehicleYear, string propertyType, bool? isRentalIncome, bool? isExistingMortgages, int? assetTypeId, bool? isAccountDetailsKnown, string institution, string assetSubType, int? assetSubTypeId, string assetType, string assetCategory, string currencyCode, string valuationBasis, string propertyZoningType, string addressPropertyType, int? assetCategoryId)
        {
            Updated = updated;
            Created = created;
            Description = description;
            Value = value;
            AccountName = accountName;
            AccountNumber = accountNumber;
            Bsb = bsb;
            VehicleMake = vehicleMake;
            VehicleYear = vehicleYear;
            PropertyType = propertyType;
            IsRentalIncome = isRentalIncome;
            IsExistingMortgages = isExistingMortgages;
            AssetTypeId = assetTypeId;
            IsAccountDetailsKnown = isAccountDetailsKnown;
            Institution = institution;
            AssetSubType = assetSubType;
            AssetSubTypeId = assetSubTypeId;
            AssetType = assetType;
            AssetCategory = assetCategory;
            CurrencyCode = currencyCode;
            ValuationBasis = valuationBasis;
            PropertyZoningType = propertyZoningType;
            AddressPropertyType = addressPropertyType;
            AssetCategoryId = assetCategoryId;
        }

        /// <summary> Gets the updated. </summary>
        public DateTimeOffset? Updated { get; }
        /// <summary> Gets the created. </summary>
        public DateTimeOffset? Created { get; }
        /// <summary> Gets the description. </summary>
        public string Description { get; }
        /// <summary> Gets the value. </summary>
        public double? Value { get; }
        /// <summary> Gets the account name. </summary>
        public string AccountName { get; }
        /// <summary> Gets the account number. </summary>
        public string AccountNumber { get; }
        /// <summary> Gets the bsb. </summary>
        public string Bsb { get; }
        /// <summary> Gets the vehicle make. </summary>
        public string VehicleMake { get; }
        /// <summary> Gets the vehicle year. </summary>
        public int? VehicleYear { get; }
        /// <summary> Gets the property type. </summary>
        public string PropertyType { get; }
        /// <summary> Gets the is rental income. </summary>
        public bool? IsRentalIncome { get; }
        /// <summary> Gets the is existing mortgages. </summary>
        public bool? IsExistingMortgages { get; }
        /// <summary> Gets the asset type id. </summary>
        public int? AssetTypeId { get; }
        /// <summary> Gets the is account details known. </summary>
        public bool? IsAccountDetailsKnown { get; }
        /// <summary> Gets the institution. </summary>
        public string Institution { get; }
        /// <summary> Gets the asset sub type. </summary>
        public string AssetSubType { get; }
        /// <summary> Gets the asset sub type id. </summary>
        public int? AssetSubTypeId { get; }
        /// <summary> Gets the asset type. </summary>
        public string AssetType { get; }
        /// <summary> Gets the asset category. </summary>
        public string AssetCategory { get; }
        /// <summary> Gets the currency code. </summary>
        public string CurrencyCode { get; }
        /// <summary> Gets the valuation basis. </summary>
        public string ValuationBasis { get; }
        /// <summary> Gets the property zoning type. </summary>
        public string PropertyZoningType { get; }
        /// <summary> Gets the address property type. </summary>
        public string AddressPropertyType { get; }
        /// <summary> Gets the asset category id. </summary>
        public int? AssetCategoryId { get; }
    }
}
