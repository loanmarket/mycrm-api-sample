// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The HighLevelSummaryEntityType. </summary>
    public readonly partial struct HighLevelSummaryEntityType : IEquatable<HighLevelSummaryEntityType>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="HighLevelSummaryEntityType"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public HighLevelSummaryEntityType(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string HighLevelSummaryValue = "high-level-summary";

        /// <summary> high-level-summary. </summary>
        public static HighLevelSummaryEntityType HighLevelSummary { get; } = new HighLevelSummaryEntityType(HighLevelSummaryValue);
        /// <summary> Determines if two <see cref="HighLevelSummaryEntityType"/> values are the same. </summary>
        public static bool operator ==(HighLevelSummaryEntityType left, HighLevelSummaryEntityType right) => left.Equals(right);
        /// <summary> Determines if two <see cref="HighLevelSummaryEntityType"/> values are not the same. </summary>
        public static bool operator !=(HighLevelSummaryEntityType left, HighLevelSummaryEntityType right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="HighLevelSummaryEntityType"/>. </summary>
        public static implicit operator HighLevelSummaryEntityType(string value) => new HighLevelSummaryEntityType(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is HighLevelSummaryEntityType other && Equals(other);
        /// <inheritdoc />
        public bool Equals(HighLevelSummaryEntityType other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
