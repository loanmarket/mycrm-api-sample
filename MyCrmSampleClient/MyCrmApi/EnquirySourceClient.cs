// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Core.Pipeline;
using MyCrmSampleClient.MyCrmApi.Models;

namespace MyCrmSampleClient.MyCrmApi
{
    /// <summary> The EnquirySource service client. </summary>
    public partial class EnquirySourceClient
    {
        private readonly ClientDiagnostics _clientDiagnostics;
        private readonly HttpPipeline _pipeline;
        internal EnquirySourceRestClient RestClient { get; }

        /// <summary> Initializes a new instance of EnquirySourceClient for mocking. </summary>
        protected EnquirySourceClient()
        {
        }

        /// <summary> Initializes a new instance of EnquirySourceClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="clientDiagnostics"/> or <paramref name="pipeline"/> is null. </exception>
        internal EnquirySourceClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
        {
            RestClient = new EnquirySourceRestClient(clientDiagnostics, pipeline, endpoint);
            _clientDiagnostics = clientDiagnostics;
            _pipeline = pipeline;
        }

        /// <summary> Where `id` is the identifier of the enquiry source. </summary>
        /// <param name="id"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<EnquirySourceDocument>> GetAsync(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("EnquirySourceClient.Get");
            scope.Start();
            try
            {
                return await RestClient.GetAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the enquiry source. </summary>
        /// <param name="id"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<EnquirySourceDocument> Get(string id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("EnquirySourceClient.Get");
            scope.Start();
            try
            {
                return RestClient.Get(id, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
    }
}
