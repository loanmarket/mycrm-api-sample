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
        /// <param name="email"></param>
        /// <param name="name"></param>
        /// <param name="tradingName"></param>
        /// <param name="website"></param>
        internal OrganisationAttributes(string email, string name, string tradingName, string website)
        {
            Email = email;
            Name = name;
            TradingName = tradingName;
            Website = website;
        }

        public string Email { get; }
        public string Name { get; }
        public string TradingName { get; }
        public string Website { get; }
    }
}
