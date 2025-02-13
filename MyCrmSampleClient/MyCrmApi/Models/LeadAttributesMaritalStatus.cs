// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The LeadAttributesMaritalStatus. </summary>
    public readonly partial struct LeadAttributesMaritalStatus : IEquatable<LeadAttributesMaritalStatus>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="LeadAttributesMaritalStatus"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public LeadAttributesMaritalStatus(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string SingleValue = "Single";
        private const string MarriedValue = "Married";
        private const string DeFactoValue = "DeFacto";
        private const string SeparatedValue = "Separated";
        private const string DivorcedValue = "Divorced";
        private const string WidowedValue = "Widowed";

        /// <summary> Single. </summary>
        public static LeadAttributesMaritalStatus Single { get; } = new LeadAttributesMaritalStatus(SingleValue);
        /// <summary> Married. </summary>
        public static LeadAttributesMaritalStatus Married { get; } = new LeadAttributesMaritalStatus(MarriedValue);
        /// <summary> DeFacto. </summary>
        public static LeadAttributesMaritalStatus DeFacto { get; } = new LeadAttributesMaritalStatus(DeFactoValue);
        /// <summary> Separated. </summary>
        public static LeadAttributesMaritalStatus Separated { get; } = new LeadAttributesMaritalStatus(SeparatedValue);
        /// <summary> Divorced. </summary>
        public static LeadAttributesMaritalStatus Divorced { get; } = new LeadAttributesMaritalStatus(DivorcedValue);
        /// <summary> Widowed. </summary>
        public static LeadAttributesMaritalStatus Widowed { get; } = new LeadAttributesMaritalStatus(WidowedValue);
        /// <summary> Determines if two <see cref="LeadAttributesMaritalStatus"/> values are the same. </summary>
        public static bool operator ==(LeadAttributesMaritalStatus left, LeadAttributesMaritalStatus right) => left.Equals(right);
        /// <summary> Determines if two <see cref="LeadAttributesMaritalStatus"/> values are not the same. </summary>
        public static bool operator !=(LeadAttributesMaritalStatus left, LeadAttributesMaritalStatus right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="LeadAttributesMaritalStatus"/>. </summary>
        public static implicit operator LeadAttributesMaritalStatus(string value) => new LeadAttributesMaritalStatus(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is LeadAttributesMaritalStatus other && Equals(other);
        /// <inheritdoc />
        public bool Equals(LeadAttributesMaritalStatus other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
