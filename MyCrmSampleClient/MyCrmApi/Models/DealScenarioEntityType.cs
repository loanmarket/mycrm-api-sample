// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The DealScenarioEntityType. </summary>
    public readonly partial struct DealScenarioEntityType : IEquatable<DealScenarioEntityType>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="DealScenarioEntityType"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public DealScenarioEntityType(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string DealScenarioValue = "deal-scenario";

        /// <summary> deal-scenario. </summary>
        public static DealScenarioEntityType DealScenario { get; } = new DealScenarioEntityType(DealScenarioValue);
        /// <summary> Determines if two <see cref="DealScenarioEntityType"/> values are the same. </summary>
        public static bool operator ==(DealScenarioEntityType left, DealScenarioEntityType right) => left.Equals(right);
        /// <summary> Determines if two <see cref="DealScenarioEntityType"/> values are not the same. </summary>
        public static bool operator !=(DealScenarioEntityType left, DealScenarioEntityType right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="DealScenarioEntityType"/>. </summary>
        public static implicit operator DealScenarioEntityType(string value) => new DealScenarioEntityType(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is DealScenarioEntityType other && Equals(other);
        /// <inheritdoc />
        public bool Equals(DealScenarioEntityType other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
