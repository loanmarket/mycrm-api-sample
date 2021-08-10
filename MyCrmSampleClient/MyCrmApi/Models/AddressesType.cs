// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The AddressesType. </summary>
    public readonly partial struct AddressesType : IEquatable<AddressesType>
    {
        private readonly string _value;

        /// <summary> Determines if two <see cref="AddressesType"/> values are the same. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public AddressesType(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string AddressValue = "address";

        /// <summary> address. </summary>
        public static AddressesType Address { get; } = new AddressesType(AddressValue);
        /// <summary> Determines if two <see cref="AddressesType"/> values are the same. </summary>
        public static bool operator ==(AddressesType left, AddressesType right) => left.Equals(right);
        /// <summary> Determines if two <see cref="AddressesType"/> values are not the same. </summary>
        public static bool operator !=(AddressesType left, AddressesType right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="AddressesType"/>. </summary>
        public static implicit operator AddressesType(string value) => new AddressesType(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is AddressesType other && Equals(other);
        /// <inheritdoc />
        public bool Equals(AddressesType other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
