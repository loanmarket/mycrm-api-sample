// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The DealImportantDatesType. </summary>
    internal readonly partial struct DealImportantDatesType : IEquatable<DealImportantDatesType>
    {
        private readonly string _value;

        /// <summary> Determines if two <see cref="DealImportantDatesType"/> values are the same. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public DealImportantDatesType(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string DealImportantDatesValue = "deal-important-dates";

        /// <summary> deal-important-dates. </summary>
        public static DealImportantDatesType DealImportantDates { get; } = new DealImportantDatesType(DealImportantDatesValue);
        /// <summary> Determines if two <see cref="DealImportantDatesType"/> values are the same. </summary>
        public static bool operator ==(DealImportantDatesType left, DealImportantDatesType right) => left.Equals(right);
        /// <summary> Determines if two <see cref="DealImportantDatesType"/> values are not the same. </summary>
        public static bool operator !=(DealImportantDatesType left, DealImportantDatesType right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="DealImportantDatesType"/>. </summary>
        public static implicit operator DealImportantDatesType(string value) => new DealImportantDatesType(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is DealImportantDatesType other && Equals(other);
        /// <inheritdoc />
        public bool Equals(DealImportantDatesType other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
