// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The OrganisationAttributes. </summary>
    public partial class OrganisationAttributes
    {
        /// <summary> Initializes a new instance of OrganisationAttributes. </summary>
        public OrganisationAttributes()
        {
        }

        /// <summary> Initializes a new instance of OrganisationAttributes. </summary>
        /// <param name="brandedCategory"></param>
        /// <param name="primaryBrandColour"></param>
        /// <param name="name"></param>
        /// <param name="tradingName"></param>
        /// <param name="website"></param>
        /// <param name="companyEmail"></param>
        /// <param name="isBrokerSearchVisible"></param>
        /// <param name="status"></param>
        /// <param name="slug"></param>
        /// <param name="businessNumber"></param>
        /// <param name="companyNumber"></param>
        /// <param name="officeDisplayName"></param>
        /// <param name="emailForCommissions"></param>
        /// <param name="phone"></param>
        /// <param name="fax"></param>
        /// <param name="brandLogoUrl"></param>
        internal OrganisationAttributes(string brandedCategory, string primaryBrandColour, string name, string tradingName, string website, string companyEmail, bool? isBrokerSearchVisible, string status, string slug, string businessNumber, string companyNumber, string officeDisplayName, string emailForCommissions, string phone, string fax, string brandLogoUrl)
        {
            BrandedCategory = brandedCategory;
            PrimaryBrandColour = primaryBrandColour;
            Name = name;
            TradingName = tradingName;
            Website = website;
            CompanyEmail = companyEmail;
            IsBrokerSearchVisible = isBrokerSearchVisible;
            Status = status;
            Slug = slug;
            BusinessNumber = businessNumber;
            CompanyNumber = companyNumber;
            OfficeDisplayName = officeDisplayName;
            EmailForCommissions = emailForCommissions;
            Phone = phone;
            Fax = fax;
            BrandLogoUrl = brandLogoUrl;
        }

        /// <summary> Gets the branded category. </summary>
        public string BrandedCategory { get; }
        /// <summary> Gets the primary brand colour. </summary>
        public string PrimaryBrandColour { get; }
        /// <summary> Gets the name. </summary>
        public string Name { get; }
        /// <summary> Gets the trading name. </summary>
        public string TradingName { get; }
        /// <summary> Gets the website. </summary>
        public string Website { get; }
        /// <summary> Gets the company email. </summary>
        public string CompanyEmail { get; }
        /// <summary> Gets the is broker search visible. </summary>
        public bool? IsBrokerSearchVisible { get; }
        /// <summary> Gets the status. </summary>
        public string Status { get; }
        /// <summary> Gets the slug. </summary>
        public string Slug { get; }
        /// <summary> Gets the business number. </summary>
        public string BusinessNumber { get; }
        /// <summary> Gets the company number. </summary>
        public string CompanyNumber { get; }
        /// <summary> Gets the office display name. </summary>
        public string OfficeDisplayName { get; }
        /// <summary> Gets the email for commissions. </summary>
        public string EmailForCommissions { get; }
        /// <summary> Gets the phone. </summary>
        public string Phone { get; }
        /// <summary> Gets the fax. </summary>
        public string Fax { get; }
        /// <summary> Gets the brand logo url. </summary>
        public string BrandLogoUrl { get; }
    }
}
