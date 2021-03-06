// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The OwnersEntityType. </summary>
    public readonly partial struct OwnersEntityType : IEquatable<OwnersEntityType>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="OwnersEntityType"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public OwnersEntityType(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string OwnersValue = "owners";

        /// <summary> owners. </summary>
        public static OwnersEntityType Owners { get; } = new OwnersEntityType(OwnersValue);
        /// <summary> Determines if two <see cref="OwnersEntityType"/> values are the same. </summary>
        public static bool operator ==(OwnersEntityType left, OwnersEntityType right) => left.Equals(right);
        /// <summary> Determines if two <see cref="OwnersEntityType"/> values are not the same. </summary>
        public static bool operator !=(OwnersEntityType left, OwnersEntityType right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="OwnersEntityType"/>. </summary>
        public static implicit operator OwnersEntityType(string value) => new OwnersEntityType(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is OwnersEntityType other && Equals(other);
        /// <inheritdoc />
        public bool Equals(OwnersEntityType other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
