// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The ContactAddressAttributes. </summary>
    public partial class ContactAddressAttributes
    {
        /// <summary> Initializes a new instance of ContactAddressAttributes. </summary>
        public ContactAddressAttributes()
        {
        }

        /// <summary> Initializes a new instance of ContactAddressAttributes. </summary>
        /// <param name="updated"></param>
        /// <param name="created"></param>
        /// <param name="addressType"></param>
        /// <param name="formattedAddress"></param>
        /// <param name="streetAddress"></param>
        /// <param name="country"></param>
        /// <param name="suburb"></param>
        /// <param name="postCode"></param>
        /// <param name="state"></param>
        internal ContactAddressAttributes(DateTimeOffset? updated, DateTimeOffset? created, ContactAddressAttributesAddressType? addressType, string formattedAddress, string streetAddress, string country, string suburb, string postCode, string state)
        {
            Updated = updated;
            Created = created;
            AddressType = addressType;
            FormattedAddress = formattedAddress;
            StreetAddress = streetAddress;
            Country = country;
            Suburb = suburb;
            PostCode = postCode;
            State = state;
        }

        /// <summary> Gets the updated. </summary>
        public DateTimeOffset? Updated { get; }
        /// <summary> Gets the created. </summary>
        public DateTimeOffset? Created { get; }
        /// <summary> Gets or sets the address type. </summary>
        public ContactAddressAttributesAddressType? AddressType { get; set; }
        /// <summary> Gets the formatted address. </summary>
        public string FormattedAddress { get; }
        /// <summary> Gets the street address. </summary>
        public string StreetAddress { get; }
        /// <summary> Gets the country. </summary>
        public string Country { get; }
        /// <summary> Gets the suburb. </summary>
        public string Suburb { get; }
        /// <summary> Gets the post code. </summary>
        public string PostCode { get; }
        /// <summary> Gets the state. </summary>
        public string State { get; }
    }
}
