// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The DealStructureImportantDatesEntityType. </summary>
    public readonly partial struct DealStructureImportantDatesEntityType : IEquatable<DealStructureImportantDatesEntityType>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="DealStructureImportantDatesEntityType"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public DealStructureImportantDatesEntityType(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string DealStructureImportantDatesValue = "deal-structure-important-dates";

        /// <summary> deal-structure-important-dates. </summary>
        public static DealStructureImportantDatesEntityType DealStructureImportantDates { get; } = new DealStructureImportantDatesEntityType(DealStructureImportantDatesValue);
        /// <summary> Determines if two <see cref="DealStructureImportantDatesEntityType"/> values are the same. </summary>
        public static bool operator ==(DealStructureImportantDatesEntityType left, DealStructureImportantDatesEntityType right) => left.Equals(right);
        /// <summary> Determines if two <see cref="DealStructureImportantDatesEntityType"/> values are not the same. </summary>
        public static bool operator !=(DealStructureImportantDatesEntityType left, DealStructureImportantDatesEntityType right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="DealStructureImportantDatesEntityType"/>. </summary>
        public static implicit operator DealStructureImportantDatesEntityType(string value) => new DealStructureImportantDatesEntityType(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is DealStructureImportantDatesEntityType other && Equals(other);
        /// <inheritdoc />
        public bool Equals(DealStructureImportantDatesEntityType other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
