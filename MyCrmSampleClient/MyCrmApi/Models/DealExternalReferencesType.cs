// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The DealExternalReferencesType. </summary>
    public readonly partial struct DealExternalReferencesType : IEquatable<DealExternalReferencesType>
    {
        private readonly string _value;

        /// <summary> Determines if two <see cref="DealExternalReferencesType"/> values are the same. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public DealExternalReferencesType(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string DealExternalReferencesValue = "deal-external-references";

        /// <summary> deal-external-references. </summary>
        public static DealExternalReferencesType DealExternalReferences { get; } = new DealExternalReferencesType(DealExternalReferencesValue);
        /// <summary> Determines if two <see cref="DealExternalReferencesType"/> values are the same. </summary>
        public static bool operator ==(DealExternalReferencesType left, DealExternalReferencesType right) => left.Equals(right);
        /// <summary> Determines if two <see cref="DealExternalReferencesType"/> values are not the same. </summary>
        public static bool operator !=(DealExternalReferencesType left, DealExternalReferencesType right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="DealExternalReferencesType"/>. </summary>
        public static implicit operator DealExternalReferencesType(string value) => new DealExternalReferencesType(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is DealExternalReferencesType other && Equals(other);
        /// <inheritdoc />
        public bool Equals(DealExternalReferencesType other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}