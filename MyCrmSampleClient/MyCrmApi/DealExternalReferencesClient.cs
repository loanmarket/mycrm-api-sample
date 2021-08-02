// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Core.Pipeline;
using MyCrmSampleClient.MyCrmApi.Models;

namespace MyCrmSampleClient.MyCrmApi
{
    /// <summary> The DealExternalReferences service client. </summary>
    public partial class DealExternalReferencesClient
    {
        private readonly ClientDiagnostics _clientDiagnostics;
        private readonly HttpPipeline _pipeline;
        internal DealExternalReferencesRestClient RestClient { get; }

        /// <summary> Initializes a new instance of DealExternalReferencesClient for mocking. </summary>
        protected DealExternalReferencesClient()
        {
        }

        /// <summary> Initializes a new instance of DealExternalReferencesClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        internal DealExternalReferencesClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
        {
            RestClient = new DealExternalReferencesRestClient(clientDiagnostics, pipeline, endpoint);
            _clientDiagnostics = clientDiagnostics;
            _pipeline = pipeline;
        }

        /// <param name="filter"> The ArrayOfString to use. </param>
        /// <param name="sort"> The ArrayOfString to use. </param>
        /// <param name="page"> The Pagination to use. </param>
        /// <param name="include"> The ArrayOfString to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<DealExternalReferencesDocument>> GetAsync(IEnumerable<string> filter = null, IEnumerable<string> sort = null, Pagination page = null, IEnumerable<string> include = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferencesClient.Get");
            scope.Start();
            try
            {
                return await RestClient.GetAsync(filter, sort, page, include, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="filter"> The ArrayOfString to use. </param>
        /// <param name="sort"> The ArrayOfString to use. </param>
        /// <param name="page"> The Pagination to use. </param>
        /// <param name="include"> The ArrayOfString to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<DealExternalReferencesDocument> Get(IEnumerable<string> filter = null, IEnumerable<string> sort = null, Pagination page = null, IEnumerable<string> include = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferencesClient.Get");
            scope.Start();
            try
            {
                return RestClient.Get(filter, sort, page, include, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> HeadAsync(CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferencesClient.Head");
            scope.Start();
            try
            {
                return await RestClient.HeadAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response Head(CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferencesClient.Head");
            scope.Start();
            try
            {
                return RestClient.Head(cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
    }
}