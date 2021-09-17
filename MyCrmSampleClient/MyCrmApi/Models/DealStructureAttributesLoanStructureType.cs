// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The DealStructureAttributesLoanStructureType. </summary>
    public readonly partial struct DealStructureAttributesLoanStructureType : IEquatable<DealStructureAttributesLoanStructureType>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="DealStructureAttributesLoanStructureType"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public DealStructureAttributesLoanStructureType(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string PrincipalInterestValue = "PrincipalInterest";
        private const string InterestOnlyValue = "InterestOnly";
        private const string RevolvingCreditValue = "RevolvingCredit";
        private const string OffsetValue = "Offset";

        /// <summary> PrincipalInterest. </summary>
        public static DealStructureAttributesLoanStructureType PrincipalInterest { get; } = new DealStructureAttributesLoanStructureType(PrincipalInterestValue);
        /// <summary> InterestOnly. </summary>
        public static DealStructureAttributesLoanStructureType InterestOnly { get; } = new DealStructureAttributesLoanStructureType(InterestOnlyValue);
        /// <summary> RevolvingCredit. </summary>
        public static DealStructureAttributesLoanStructureType RevolvingCredit { get; } = new DealStructureAttributesLoanStructureType(RevolvingCreditValue);
        /// <summary> Offset. </summary>
        public static DealStructureAttributesLoanStructureType Offset { get; } = new DealStructureAttributesLoanStructureType(OffsetValue);
        /// <summary> Determines if two <see cref="DealStructureAttributesLoanStructureType"/> values are the same. </summary>
        public static bool operator ==(DealStructureAttributesLoanStructureType left, DealStructureAttributesLoanStructureType right) => left.Equals(right);
        /// <summary> Determines if two <see cref="DealStructureAttributesLoanStructureType"/> values are not the same. </summary>
        public static bool operator !=(DealStructureAttributesLoanStructureType left, DealStructureAttributesLoanStructureType right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="DealStructureAttributesLoanStructureType"/>. </summary>
        public static implicit operator DealStructureAttributesLoanStructureType(string value) => new DealStructureAttributesLoanStructureType(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is DealStructureAttributesLoanStructureType other && Equals(other);
        /// <inheritdoc />
        public bool Equals(DealStructureAttributesLoanStructureType other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
