// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The Dimension. </summary>
    internal readonly partial struct Dimension : IEquatable<Dimension>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="Dimension"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public Dimension(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string PointValue = "Point";
        private const string CurveValue = "Curve";
        private const string SurfaceValue = "Surface";
        private const string CollapseValue = "Collapse";
        private const string DontcareValue = "Dontcare";
        private const string TrueValue = "True";
        private const string FalseValue = "False";

        /// <summary> Point. </summary>
        public static Dimension Point { get; } = new Dimension(PointValue);
        /// <summary> Curve. </summary>
        public static Dimension Curve { get; } = new Dimension(CurveValue);
        /// <summary> Surface. </summary>
        public static Dimension Surface { get; } = new Dimension(SurfaceValue);
        /// <summary> Collapse. </summary>
        public static Dimension Collapse { get; } = new Dimension(CollapseValue);
        /// <summary> Dontcare. </summary>
        public static Dimension Dontcare { get; } = new Dimension(DontcareValue);
        /// <summary> True. </summary>
        public static Dimension True { get; } = new Dimension(TrueValue);
        /// <summary> False. </summary>
        public static Dimension False { get; } = new Dimension(FalseValue);
        /// <summary> Determines if two <see cref="Dimension"/> values are the same. </summary>
        public static bool operator ==(Dimension left, Dimension right) => left.Equals(right);
        /// <summary> Determines if two <see cref="Dimension"/> values are not the same. </summary>
        public static bool operator !=(Dimension left, Dimension right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="Dimension"/>. </summary>
        public static implicit operator Dimension(string value) => new Dimension(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is Dimension other && Equals(other);
        /// <inheritdoc />
        public bool Equals(Dimension other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value?.GetHashCode() ?? 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}