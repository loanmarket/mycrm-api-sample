// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The LoanStructureType. </summary>
    public readonly partial struct LoanStructureType : IEquatable<LoanStructureType>
    {
        private readonly string _value;

        /// <summary> Determines if two <see cref="LoanStructureType"/> values are the same. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public LoanStructureType(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string PrincipalInterestValue = "PrincipalInterest";
        private const string InterestOnlyValue = "InterestOnly";
        private const string RevolvingCreditValue = "RevolvingCredit";
        private const string OffsetValue = "Offset";

        /// <summary> PrincipalInterest. </summary>
        public static LoanStructureType PrincipalInterest { get; } = new LoanStructureType(PrincipalInterestValue);
        /// <summary> InterestOnly. </summary>
        public static LoanStructureType InterestOnly { get; } = new LoanStructureType(InterestOnlyValue);
        /// <summary> RevolvingCredit. </summary>
        public static LoanStructureType RevolvingCredit { get; } = new LoanStructureType(RevolvingCreditValue);
        /// <summary> Offset. </summary>
        public static LoanStructureType Offset { get; } = new LoanStructureType(OffsetValue);
        /// <summary> Determines if two <see cref="LoanStructureType"/> values are the same. </summary>
        public static bool operator ==(LoanStructureType left, LoanStructureType right) => left.Equals(right);
        /// <summary> Determines if two <see cref="LoanStructureType"/> values are not the same. </summary>
        public static bool operator !=(LoanStructureType left, LoanStructureType right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="LoanStructureType"/>. </summary>
        public static implicit operator LoanStructureType(string value) => new LoanStructureType(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is LoanStructureType other && Equals(other);
        /// <inheritdoc />
        public bool Equals(LoanStructureType other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
