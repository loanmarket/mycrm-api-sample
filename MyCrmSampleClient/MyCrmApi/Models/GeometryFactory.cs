// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The GeometryFactory. </summary>
    internal partial class GeometryFactory
    {
        /// <summary> Initializes a new instance of GeometryFactory. </summary>
        internal GeometryFactory()
        {
        }

        /// <summary> Gets the precision model. </summary>
        public PrecisionModel PrecisionModel { get; }
        /// <summary> Gets the coordinate sequence factory. </summary>
        public CoordinateSequenceFactory CoordinateSequenceFactory { get; }
        /// <summary> Gets the srid. </summary>
        public int? Srid { get; }
        /// <summary> Gets the geometry services. </summary>
        public NtsGeometryServices GeometryServices { get; }
    }
}
