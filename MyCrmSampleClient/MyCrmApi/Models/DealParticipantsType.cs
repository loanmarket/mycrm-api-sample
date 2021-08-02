// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The DealParticipantsType. </summary>
    public readonly partial struct DealParticipantsType : IEquatable<DealParticipantsType>
    {
        private readonly string _value;

        /// <summary> Determines if two <see cref="DealParticipantsType"/> values are the same. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public DealParticipantsType(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string DealParticipantsValue = "deal-participants";

        /// <summary> deal-participants. </summary>
        public static DealParticipantsType DealParticipants { get; } = new DealParticipantsType(DealParticipantsValue);
        /// <summary> Determines if two <see cref="DealParticipantsType"/> values are the same. </summary>
        public static bool operator ==(DealParticipantsType left, DealParticipantsType right) => left.Equals(right);
        /// <summary> Determines if two <see cref="DealParticipantsType"/> values are not the same. </summary>
        public static bool operator !=(DealParticipantsType left, DealParticipantsType right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="DealParticipantsType"/>. </summary>
        public static implicit operator DealParticipantsType(string value) => new DealParticipantsType(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is DealParticipantsType other && Equals(other);
        /// <inheritdoc />
        public bool Equals(DealParticipantsType other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
