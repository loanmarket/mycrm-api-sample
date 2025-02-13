// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The LeadContact. </summary>
    public partial class LeadContact
    {
        /// <summary> Initializes a new instance of LeadContact. </summary>
        public LeadContact()
        {
            Addresses = new ChangeTrackingList<LeadAddressReference>();
            Employments = new ChangeTrackingList<LeadEmploymentReference>();
        }

        /// <summary> Initializes a new instance of LeadContact. </summary>
        /// <param name="lid"></param>
        /// <param name="title"></param>
        /// <param name="firstName"></param>
        /// <param name="preferredName"></param>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="mobile"></param>
        /// <param name="homePhone"></param>
        /// <param name="businessPhone"></param>
        /// <param name="dateOfBirthAsString"></param>
        /// <param name="dateOfBirth"></param>
        /// <param name="gender"></param>
        /// <param name="maritalStatus"></param>
        /// <param name="hasMarketingConsent"></param>
        /// <param name="isGuarantor"></param>
        /// <param name="isDependant"></param>
        /// <param name="isPrimary"></param>
        /// <param name="addresses"></param>
        /// <param name="employments"></param>
        internal LeadContact(string lid, Title? title, string firstName, string preferredName, string lastName, string email, string mobile, string homePhone, string businessPhone, DateTimeOffset? dateOfBirthAsString, DateTimeOffset? dateOfBirth, Gender? gender, MaritalStatus? maritalStatus, bool? hasMarketingConsent, bool? isGuarantor, bool? isDependant, bool? isPrimary, IList<LeadAddressReference> addresses, IList<LeadEmploymentReference> employments)
        {
            Lid = lid;
            Title = title;
            FirstName = firstName;
            PreferredName = preferredName;
            LastName = lastName;
            Email = email;
            Mobile = mobile;
            HomePhone = homePhone;
            BusinessPhone = businessPhone;
            DateOfBirthAsString = dateOfBirthAsString;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            MaritalStatus = maritalStatus;
            HasMarketingConsent = hasMarketingConsent;
            IsGuarantor = isGuarantor;
            IsDependant = isDependant;
            IsPrimary = isPrimary;
            Addresses = addresses;
            Employments = employments;
        }

        /// <summary> Gets or sets the lid. </summary>
        public string Lid { get; set; }
        /// <summary> Gets or sets the title. </summary>
        public Title? Title { get; set; }
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
        /// <summary> Gets or sets the home phone. </summary>
        public string HomePhone { get; set; }
        /// <summary> Gets or sets the business phone. </summary>
        public string BusinessPhone { get; set; }
        /// <summary> Gets or sets the date of birth as string. </summary>
        public DateTimeOffset? DateOfBirthAsString { get; set; }
        /// <summary> Gets or sets the date of birth. </summary>
        public DateTimeOffset? DateOfBirth { get; set; }
        /// <summary> Gets or sets the gender. </summary>
        public Gender? Gender { get; set; }
        /// <summary> Gets or sets the marital status. </summary>
        public MaritalStatus? MaritalStatus { get; set; }
        /// <summary> Gets or sets the has marketing consent. </summary>
        public bool? HasMarketingConsent { get; set; }
        /// <summary> Gets or sets the is guarantor. </summary>
        public bool? IsGuarantor { get; set; }
        /// <summary> Gets or sets the is dependant. </summary>
        public bool? IsDependant { get; set; }
        /// <summary> Gets or sets the is primary. </summary>
        public bool? IsPrimary { get; set; }
        /// <summary> Gets or sets the addresses. </summary>
        public IList<LeadAddressReference> Addresses { get; set; }
        /// <summary> Gets or sets the employments. </summary>
        public IList<LeadEmploymentReference> Employments { get; set; }
    }
}
