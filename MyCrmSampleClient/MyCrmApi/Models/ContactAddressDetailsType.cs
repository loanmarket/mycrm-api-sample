// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The ContactAddressDetailsType. </summary>
    public readonly partial struct ContactAddressDetailsType : IEquatable<ContactAddressDetailsType>
    {
        private readonly string _value;

        /// <summary> Determines if two <see cref="ContactAddressDetailsType"/> values are the same. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public ContactAddressDetailsType(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string ContactAddressDetailsValue = "contact-address-details";

        /// <summary> contact-address-details. </summary>
        public static ContactAddressDetailsType ContactAddressDetails { get; } = new ContactAddressDetailsType(ContactAddressDetailsValue);
        /// <summary> Determines if two <see cref="ContactAddressDetailsType"/> values are the same. </summary>
        public static bool operator ==(ContactAddressDetailsType left, ContactAddressDetailsType right) => left.Equals(right);
        /// <summary> Determines if two <see cref="ContactAddressDetailsType"/> values are not the same. </summary>
        public static bool operator !=(ContactAddressDetailsType left, ContactAddressDetailsType right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="ContactAddressDetailsType"/>. </summary>
        public static implicit operator ContactAddressDetailsType(string value) => new ContactAddressDetailsType(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is ContactAddressDetailsType other && Equals(other);
        /// <inheritdoc />
        public bool Equals(ContactAddressDetailsType other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
