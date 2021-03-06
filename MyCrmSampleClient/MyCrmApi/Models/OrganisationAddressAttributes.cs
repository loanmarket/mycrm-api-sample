// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The OrganisationAddressAttributes. </summary>
    public partial class OrganisationAddressAttributes
    {
        /// <summary> Initializes a new instance of OrganisationAddressAttributes. </summary>
        public OrganisationAddressAttributes()
        {
        }

        /// <summary> Initializes a new instance of OrganisationAddressAttributes. </summary>
        /// <param name="isMailing"></param>
        /// <param name="isCustomAddress"></param>
        /// <param name="formattedAddress"></param>
        /// <param name="streetAddress"></param>
        /// <param name="country"></param>
        /// <param name="suburb"></param>
        /// <param name="postCode"></param>
        /// <param name="state"></param>
        internal OrganisationAddressAttributes(bool? isMailing, bool? isCustomAddress, string formattedAddress, string streetAddress, string country, string suburb, string postCode, string state)
        {
            IsMailing = isMailing;
            IsCustomAddress = isCustomAddress;
            FormattedAddress = formattedAddress;
            StreetAddress = streetAddress;
            Country = country;
            Suburb = suburb;
            PostCode = postCode;
            State = state;
        }

        /// <summary> Gets or sets the is mailing. </summary>
        public bool? IsMailing { get; set; }
        /// <summary> Gets or sets the is custom address. </summary>
        public bool? IsCustomAddress { get; set; }
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
