// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The LeadAttributes. </summary>
    public partial class LeadAttributes
    {
        /// <summary> Initializes a new instance of LeadAttributes. </summary>
        public LeadAttributes()
        {
        }

        /// <summary> Initializes a new instance of LeadAttributes. </summary>
        /// <param name="dateOfBirth"></param>
        /// <param name="title"></param>
        /// <param name="firstName"></param>
        /// <param name="preferredName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="mobile"></param>
        /// <param name="gender"></param>
        /// <param name="hasMarketingConsent"></param>
        /// <param name="isGuarantor"></param>
        /// <param name="isDependant"></param>
        /// <param name="isPrimary"></param>
        /// <param name="streetAddress"></param>
        /// <param name="suburb"></param>
        /// <param name="state"></param>
        /// <param name="postCode"></param>
        /// <param name="country"></param>
        /// <param name="addressType"></param>
        /// <param name="formattedAddress"></param>
        /// <param name="noteTitle"></param>
        /// <param name="noteDetails"></param>
        /// <param name="utmSource"></param>
        /// <param name="utmMedium"></param>
        /// <param name="utmCampaign"></param>
        /// <param name="utmTerm"></param>
        /// <param name="utmContent"></param>
        /// <param name="sourceSystemUrl"></param>
        /// <param name="sourceId"></param>
        /// <param name="sourceCategoryId"></param>
        /// <param name="sourceAdditionalDetails"></param>
        /// <param name="externalReference"></param>
        /// <param name="externalIntegration"></param>
        /// <param name="externalIntegrationAllowCreate"></param>
        /// <param name="sendNotification"></param>
        /// <param name="customStatusName"></param>
        /// <param name="dealStatus"></param>
        internal LeadAttributes(DateTimeOffset? dateOfBirth, LeadAttributesTitle? title, string firstName, string preferredName, string lastName, string email, string mobile, LeadAttributesGender? gender, bool? hasMarketingConsent, bool? isGuarantor, bool? isDependant, bool? isPrimary, string streetAddress, string suburb, string state, string postCode, string country, LeadAttributesAddressType? addressType, string formattedAddress, string noteTitle, string noteDetails, string utmSource, string utmMedium, string utmCampaign, string utmTerm, string utmContent, string sourceSystemUrl, int? sourceId, int? sourceCategoryId, string sourceAdditionalDetails, string externalReference, string externalIntegration, bool? externalIntegrationAllowCreate, bool? sendNotification, string customStatusName, LeadAttributesDealStatus? dealStatus)
        {
            DateOfBirth = dateOfBirth;
            Title = title;
            FirstName = firstName;
            PreferredName = preferredName;
            LastName = lastName;
            Email = email;
            Mobile = mobile;
            Gender = gender;
            HasMarketingConsent = hasMarketingConsent;
            IsGuarantor = isGuarantor;
            IsDependant = isDependant;
            IsPrimary = isPrimary;
            StreetAddress = streetAddress;
            Suburb = suburb;
            State = state;
            PostCode = postCode;
            Country = country;
            AddressType = addressType;
            FormattedAddress = formattedAddress;
            NoteTitle = noteTitle;
            NoteDetails = noteDetails;
            UtmSource = utmSource;
            UtmMedium = utmMedium;
            UtmCampaign = utmCampaign;
            UtmTerm = utmTerm;
            UtmContent = utmContent;
            SourceSystemUrl = sourceSystemUrl;
            SourceId = sourceId;
            SourceCategoryId = sourceCategoryId;
            SourceAdditionalDetails = sourceAdditionalDetails;
            ExternalReference = externalReference;
            ExternalIntegration = externalIntegration;
            ExternalIntegrationAllowCreate = externalIntegrationAllowCreate;
            SendNotification = sendNotification;
            CustomStatusName = customStatusName;
            DealStatus = dealStatus;
        }

        /// <summary> Gets or sets the date of birth. </summary>
        public DateTimeOffset? DateOfBirth { get; set; }
        /// <summary> Gets or sets the title. </summary>
        public LeadAttributesTitle? Title { get; set; }
        /// <summary> Gets or sets the first name. </summary>
        public string FirstName { get; set; }
        /// <summary> Gets or sets the preferred name. </summary>
        public string PreferredName { get; set; }
        /// <summary> Gets or sets the last name. </summary>
        public string LastName { get; set; }
        /// <summary> Gets or sets the email. </summary>
        public string Email { get; set; }
        /// <summary> Gets or sets the mobile. </summary>
        public string Mobile { get; set; }
        /// <summary> Gets or sets the gender. </summary>
        public LeadAttributesGender? Gender { get; set; }
        /// <summary> Gets or sets the has marketing consent. </summary>
        public bool? HasMarketingConsent { get; set; }
        /// <summary> Gets or sets the is guarantor. </summary>
        public bool? IsGuarantor { get; set; }
        /// <summary> Gets or sets the is dependant. </summary>
        public bool? IsDependant { get; set; }
        /// <summary> Gets or sets the is primary. </summary>
        public bool? IsPrimary { get; set; }
        /// <summary> Gets or sets the street address. </summary>
        public string StreetAddress { get; set; }
        /// <summary> Gets or sets the suburb. </summary>
        public string Suburb { get; set; }
        /// <summary> Gets or sets the state. </summary>
        public string State { get; set; }
        /// <summary> Gets or sets the post code. </summary>
        public string PostCode { get; set; }
        /// <summary> Gets or sets the country. </summary>
        public string Country { get; set; }
        /// <summary> Gets or sets the address type. </summary>
        public LeadAttributesAddressType? AddressType { get; set; }
        /// <summary> Gets or sets the formatted address. </summary>
        public string FormattedAddress { get; set; }
        /// <summary> Gets or sets the note title. </summary>
        public string NoteTitle { get; set; }
        /// <summary> Gets or sets the note details. </summary>
        public string NoteDetails { get; set; }
        /// <summary> Gets or sets the utm source. </summary>
        public string UtmSource { get; set; }
        /// <summary> Gets or sets the utm medium. </summary>
        public string UtmMedium { get; set; }
        /// <summary> Gets or sets the utm campaign. </summary>
        public string UtmCampaign { get; set; }
        /// <summary> Gets or sets the utm term. </summary>
        public string UtmTerm { get; set; }
        /// <summary> Gets or sets the utm content. </summary>
        public string UtmContent { get; set; }
        /// <summary> Gets or sets the source system url. </summary>
        public string SourceSystemUrl { get; set; }
        /// <summary> Gets or sets the source id. </summary>
        public int? SourceId { get; set; }
        /// <summary> Gets or sets the source category id. </summary>
        public int? SourceCategoryId { get; set; }
        /// <summary> Gets or sets the source additional details. </summary>
        public string SourceAdditionalDetails { get; set; }
        /// <summary> Gets or sets the external reference. </summary>
        public string ExternalReference { get; set; }
        /// <summary> Gets or sets the external integration. </summary>
        public string ExternalIntegration { get; set; }
        /// <summary> Gets or sets the external integration allow create. </summary>
        public bool? ExternalIntegrationAllowCreate { get; set; }
        /// <summary> Gets or sets the send notification. </summary>
        public bool? SendNotification { get; set; }
        /// <summary> Gets the custom status name. </summary>
        public string CustomStatusName { get; }
        /// <summary> Gets the deal status. </summary>
        public LeadAttributesDealStatus? DealStatus { get; }
    }
}
