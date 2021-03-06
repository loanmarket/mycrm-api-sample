// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi
{
    /// <summary> Client options for MyCRMAPIClient. </summary>
    public partial class MyCRMAPIClientOptions : ClientOptions
    {
        private const ServiceVersion LatestVersion = ServiceVersion.Vv1;

        /// <summary> The version of the service to use. </summary>
        public enum ServiceVersion
        {
            /// <summary> Service version "v1". </summary>
            Vv1 = 1,
        }

        internal string Version { get; }

        /// <summary> Initializes new instance of MyCRMAPIClientOptions. </summary>
        public MyCRMAPIClientOptions(ServiceVersion version = LatestVersion)
        {
            Version = version switch
            {
                ServiceVersion.Vv1 => "v1",
                _ => throw new NotSupportedException()
            };
        }
    }
}
