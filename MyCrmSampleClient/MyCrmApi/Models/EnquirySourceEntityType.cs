// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The EnquirySourceEntityType. </summary>
    public readonly partial struct EnquirySourceEntityType : IEquatable<EnquirySourceEntityType>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="EnquirySourceEntityType"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public EnquirySourceEntityType(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string EnquirySourceValue = "enquiry-source";

        /// <summary> enquiry-source. </summary>
        public static EnquirySourceEntityType EnquirySource { get; } = new EnquirySourceEntityType(EnquirySourceValue);
        /// <summary> Determines if two <see cref="EnquirySourceEntityType"/> values are the same. </summary>
        public static bool operator ==(EnquirySourceEntityType left, EnquirySourceEntityType right) => left.Equals(right);
        /// <summary> Determines if two <see cref="EnquirySourceEntityType"/> values are not the same. </summary>
        public static bool operator !=(EnquirySourceEntityType left, EnquirySourceEntityType right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="EnquirySourceEntityType"/>. </summary>
        public static implicit operator EnquirySourceEntityType(string value) => new EnquirySourceEntityType(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is EnquirySourceEntityType other && Equals(other);
        /// <inheritdoc />
        public bool Equals(EnquirySourceEntityType other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
