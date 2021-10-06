// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The DealImportantDatesEntityType. </summary>
    public readonly partial struct DealImportantDatesEntityType : IEquatable<DealImportantDatesEntityType>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="DealImportantDatesEntityType"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public DealImportantDatesEntityType(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string DealImportantDatesValue = "deal-important-dates";

        /// <summary> deal-important-dates. </summary>
        public static DealImportantDatesEntityType DealImportantDates { get; } = new DealImportantDatesEntityType(DealImportantDatesValue);
        /// <summary> Determines if two <see cref="DealImportantDatesEntityType"/> values are the same. </summary>
        public static bool operator ==(DealImportantDatesEntityType left, DealImportantDatesEntityType right) => left.Equals(right);
        /// <summary> Determines if two <see cref="DealImportantDatesEntityType"/> values are not the same. </summary>
        public static bool operator !=(DealImportantDatesEntityType left, DealImportantDatesEntityType right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="DealImportantDatesEntityType"/>. </summary>
        public static implicit operator DealImportantDatesEntityType(string value) => new DealImportantDatesEntityType(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is DealImportantDatesEntityType other && Equals(other);
        /// <inheritdoc />
        public bool Equals(DealImportantDatesEntityType other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
