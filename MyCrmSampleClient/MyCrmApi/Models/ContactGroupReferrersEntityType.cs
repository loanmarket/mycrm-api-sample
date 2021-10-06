// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The ContactGroupReferrersEntityType. </summary>
    public readonly partial struct ContactGroupReferrersEntityType : IEquatable<ContactGroupReferrersEntityType>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="ContactGroupReferrersEntityType"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public ContactGroupReferrersEntityType(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string ContactGroupReferrersValue = "contact-group-referrers";

        /// <summary> contact-group-referrers. </summary>
        public static ContactGroupReferrersEntityType ContactGroupReferrers { get; } = new ContactGroupReferrersEntityType(ContactGroupReferrersValue);
        /// <summary> Determines if two <see cref="ContactGroupReferrersEntityType"/> values are the same. </summary>
        public static bool operator ==(ContactGroupReferrersEntityType left, ContactGroupReferrersEntityType right) => left.Equals(right);
        /// <summary> Determines if two <see cref="ContactGroupReferrersEntityType"/> values are not the same. </summary>
        public static bool operator !=(ContactGroupReferrersEntityType left, ContactGroupReferrersEntityType right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="ContactGroupReferrersEntityType"/>. </summary>
        public static implicit operator ContactGroupReferrersEntityType(string value) => new ContactGroupReferrersEntityType(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is ContactGroupReferrersEntityType other && Equals(other);
        /// <inheritdoc />
        public bool Equals(ContactGroupReferrersEntityType other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
