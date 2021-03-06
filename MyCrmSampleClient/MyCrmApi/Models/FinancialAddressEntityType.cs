// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The FinancialAddressEntityType. </summary>
    public readonly partial struct FinancialAddressEntityType : IEquatable<FinancialAddressEntityType>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="FinancialAddressEntityType"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public FinancialAddressEntityType(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string FinancialAddressValue = "financial-address";

        /// <summary> financial-address. </summary>
        public static FinancialAddressEntityType FinancialAddress { get; } = new FinancialAddressEntityType(FinancialAddressValue);
        /// <summary> Determines if two <see cref="FinancialAddressEntityType"/> values are the same. </summary>
        public static bool operator ==(FinancialAddressEntityType left, FinancialAddressEntityType right) => left.Equals(right);
        /// <summary> Determines if two <see cref="FinancialAddressEntityType"/> values are not the same. </summary>
        public static bool operator !=(FinancialAddressEntityType left, FinancialAddressEntityType right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="FinancialAddressEntityType"/>. </summary>
        public static implicit operator FinancialAddressEntityType(string value) => new FinancialAddressEntityType(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is FinancialAddressEntityType other && Equals(other);
        /// <inheritdoc />
        public bool Equals(FinancialAddressEntityType other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
