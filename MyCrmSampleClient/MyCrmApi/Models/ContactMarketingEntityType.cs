// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The ContactMarketingEntityType. </summary>
    public readonly partial struct ContactMarketingEntityType : IEquatable<ContactMarketingEntityType>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="ContactMarketingEntityType"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public ContactMarketingEntityType(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string ContactMarketingValue = "contact-marketing";

        /// <summary> contact-marketing. </summary>
        public static ContactMarketingEntityType ContactMarketing { get; } = new ContactMarketingEntityType(ContactMarketingValue);
        /// <summary> Determines if two <see cref="ContactMarketingEntityType"/> values are the same. </summary>
        public static bool operator ==(ContactMarketingEntityType left, ContactMarketingEntityType right) => left.Equals(right);
        /// <summary> Determines if two <see cref="ContactMarketingEntityType"/> values are not the same. </summary>
        public static bool operator !=(ContactMarketingEntityType left, ContactMarketingEntityType right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="ContactMarketingEntityType"/>. </summary>
        public static implicit operator ContactMarketingEntityType(string value) => new ContactMarketingEntityType(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is ContactMarketingEntityType other && Equals(other);
        /// <inheritdoc />
        public bool Equals(ContactMarketingEntityType other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
