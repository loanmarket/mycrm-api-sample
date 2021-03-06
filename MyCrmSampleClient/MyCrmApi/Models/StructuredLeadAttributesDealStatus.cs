// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The StructuredLeadAttributesDealStatus. </summary>
    public readonly partial struct StructuredLeadAttributesDealStatus : IEquatable<StructuredLeadAttributesDealStatus>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="StructuredLeadAttributesDealStatus"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public StructuredLeadAttributesDealStatus(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string NewLeadsValue = "NewLeads";
        private const string ResearchingValue = "Researching";
        private const string PreApprovedValue = "PreApproved";
        private const string ConditionallyApprovedValue = "ConditionallyApproved";
        private const string UnconditionallyApprovedValue = "UnconditionallyApproved";
        private const string LodgedValue = "Lodged";
        private const string InProgressValue = "InProgress";
        private const string PendingSettlementValue = "PendingSettlement";
        private const string VariedValue = "Varied";
        private const string RepaidValue = "Repaid";
        private const string WithdrawnValue = "Withdrawn";
        private const string CancelledValue = "Cancelled";
        private const string SettledValue = "Settled";

        /// <summary> NewLeads. </summary>
        public static StructuredLeadAttributesDealStatus NewLeads { get; } = new StructuredLeadAttributesDealStatus(NewLeadsValue);
        /// <summary> Researching. </summary>
        public static StructuredLeadAttributesDealStatus Researching { get; } = new StructuredLeadAttributesDealStatus(ResearchingValue);
        /// <summary> PreApproved. </summary>
        public static StructuredLeadAttributesDealStatus PreApproved { get; } = new StructuredLeadAttributesDealStatus(PreApprovedValue);
        /// <summary> ConditionallyApproved. </summary>
        public static StructuredLeadAttributesDealStatus ConditionallyApproved { get; } = new StructuredLeadAttributesDealStatus(ConditionallyApprovedValue);
        /// <summary> UnconditionallyApproved. </summary>
        public static StructuredLeadAttributesDealStatus UnconditionallyApproved { get; } = new StructuredLeadAttributesDealStatus(UnconditionallyApprovedValue);
        /// <summary> Lodged. </summary>
        public static StructuredLeadAttributesDealStatus Lodged { get; } = new StructuredLeadAttributesDealStatus(LodgedValue);
        /// <summary> InProgress. </summary>
        public static StructuredLeadAttributesDealStatus InProgress { get; } = new StructuredLeadAttributesDealStatus(InProgressValue);
        /// <summary> PendingSettlement. </summary>
        public static StructuredLeadAttributesDealStatus PendingSettlement { get; } = new StructuredLeadAttributesDealStatus(PendingSettlementValue);
        /// <summary> Varied. </summary>
        public static StructuredLeadAttributesDealStatus Varied { get; } = new StructuredLeadAttributesDealStatus(VariedValue);
        /// <summary> Repaid. </summary>
        public static StructuredLeadAttributesDealStatus Repaid { get; } = new StructuredLeadAttributesDealStatus(RepaidValue);
        /// <summary> Withdrawn. </summary>
        public static StructuredLeadAttributesDealStatus Withdrawn { get; } = new StructuredLeadAttributesDealStatus(WithdrawnValue);
        /// <summary> Cancelled. </summary>
        public static StructuredLeadAttributesDealStatus Cancelled { get; } = new StructuredLeadAttributesDealStatus(CancelledValue);
        /// <summary> Settled. </summary>
        public static StructuredLeadAttributesDealStatus Settled { get; } = new StructuredLeadAttributesDealStatus(SettledValue);
        /// <summary> Determines if two <see cref="StructuredLeadAttributesDealStatus"/> values are the same. </summary>
        public static bool operator ==(StructuredLeadAttributesDealStatus left, StructuredLeadAttributesDealStatus right) => left.Equals(right);
        /// <summary> Determines if two <see cref="StructuredLeadAttributesDealStatus"/> values are not the same. </summary>
        public static bool operator !=(StructuredLeadAttributesDealStatus left, StructuredLeadAttributesDealStatus right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="StructuredLeadAttributesDealStatus"/>. </summary>
        public static implicit operator StructuredLeadAttributesDealStatus(string value) => new StructuredLeadAttributesDealStatus(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is StructuredLeadAttributesDealStatus other && Equals(other);
        /// <inheritdoc />
        public bool Equals(StructuredLeadAttributesDealStatus other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
