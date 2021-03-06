// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using Azure;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi
{
    internal partial class ContactExternalReferencePostHeaders
    {
        private readonly Response _response;
        public ContactExternalReferencePostHeaders(Response response)
        {
            _response = response;
        }
        /// <summary> Location of newly created resource. </summary>
        public string Location => _response.Headers.TryGetValue("Location", out string value) ? value : null;
    }
}
