// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The Gender. </summary>
    public readonly partial struct Gender : IEquatable<Gender>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="Gender"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public Gender(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string MaleValue = "Male";
        private const string FemaleValue = "Female";
        private const string UndisclosedValue = "Undisclosed";

        /// <summary> Male. </summary>
        public static Gender Male { get; } = new Gender(MaleValue);
        /// <summary> Female. </summary>
        public static Gender Female { get; } = new Gender(FemaleValue);
        /// <summary> Undisclosed. </summary>
        public static Gender Undisclosed { get; } = new Gender(UndisclosedValue);
        /// <summary> Determines if two <see cref="Gender"/> values are the same. </summary>
        public static bool operator ==(Gender left, Gender right) => left.Equals(right);
        /// <summary> Determines if two <see cref="Gender"/> values are not the same. </summary>
        public static bool operator !=(Gender left, Gender right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="Gender"/>. </summary>
        public static implicit operator Gender(string value) => new Gender(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is Gender other && Equals(other);
        /// <inheritdoc />
        public bool Equals(Gender other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
