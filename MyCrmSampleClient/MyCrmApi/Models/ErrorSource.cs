// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The ErrorSource. </summary>
    internal partial class ErrorSource
    {
        /// <summary> Initializes a new instance of ErrorSource. </summary>
        internal ErrorSource()
        {
        }

        /// <summary> Initializes a new instance of ErrorSource. </summary>
        /// <param name="pointer"></param>
        /// <param name="parameter"></param>
        /// <param name="header"></param>
        internal ErrorSource(string pointer, string parameter, string header)
        {
            Pointer = pointer;
            Parameter = parameter;
            Header = header;
        }

        /// <summary> Gets the pointer. </summary>
        public string Pointer { get; }
        /// <summary> Gets the parameter. </summary>
        public string Parameter { get; }
        /// <summary> Gets the header. </summary>
        public string Header { get; }
    }
}
