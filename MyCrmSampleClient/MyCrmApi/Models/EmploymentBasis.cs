// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The EmploymentBasis. </summary>
    public readonly partial struct EmploymentBasis : IEquatable<EmploymentBasis>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="EmploymentBasis"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public EmploymentBasis(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string FullTimeValue = "FullTime";
        private const string PartTimeValue = "PartTime";
        private const string ContractValue = "Contract";
        private const string TemporaryValue = "Temporary";
        private const string CasualValue = "Casual";

        /// <summary> FullTime. </summary>
        public static EmploymentBasis FullTime { get; } = new EmploymentBasis(FullTimeValue);
        /// <summary> PartTime. </summary>
        public static EmploymentBasis PartTime { get; } = new EmploymentBasis(PartTimeValue);
        /// <summary> Contract. </summary>
        public static EmploymentBasis Contract { get; } = new EmploymentBasis(ContractValue);
        /// <summary> Temporary. </summary>
        public static EmploymentBasis Temporary { get; } = new EmploymentBasis(TemporaryValue);
        /// <summary> Casual. </summary>
        public static EmploymentBasis Casual { get; } = new EmploymentBasis(CasualValue);
        /// <summary> Determines if two <see cref="EmploymentBasis"/> values are the same. </summary>
        public static bool operator ==(EmploymentBasis left, EmploymentBasis right) => left.Equals(right);
        /// <summary> Determines if two <see cref="EmploymentBasis"/> values are not the same. </summary>
        public static bool operator !=(EmploymentBasis left, EmploymentBasis right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="EmploymentBasis"/>. </summary>
        public static implicit operator EmploymentBasis(string value) => new EmploymentBasis(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is EmploymentBasis other && Equals(other);
        /// <inheritdoc />
        public bool Equals(EmploymentBasis other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
