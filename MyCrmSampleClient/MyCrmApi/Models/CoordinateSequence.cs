// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The CoordinateSequence. </summary>
    internal partial class CoordinateSequence
    {
        /// <summary> Initializes a new instance of CoordinateSequence. </summary>
        internal CoordinateSequence()
        {
        }

        /// <summary> Gets the dimension. </summary>
        public int? Dimension { get; }
        /// <summary> Gets the measures. </summary>
        public int? Measures { get; }
        /// <summary> Gets the spatial. </summary>
        public int? Spatial { get; }
        /// <summary> Gets the ordinates. </summary>
        public Ordinates? Ordinates { get; }
        /// <summary> Gets the has z. </summary>
        public bool? HasZ { get; }
        /// <summary> Gets the has m. </summary>
        public bool? HasM { get; }
        /// <summary> Gets the z ordinate index. </summary>
        public int? ZOrdinateIndex { get; }
        /// <summary> Gets the m ordinate index. </summary>
        public int? MOrdinateIndex { get; }
        /// <summary> Gets the first. </summary>
        public Coordinate First { get; }
        /// <summary> Gets the last. </summary>
        public Coordinate Last { get; }
        /// <summary> Gets the count. </summary>
        public int? Count { get; }
    }
}
