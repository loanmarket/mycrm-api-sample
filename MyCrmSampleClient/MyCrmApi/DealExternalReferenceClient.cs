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
    /// <summary> The DealExternalReference service client. </summary>
    public partial class DealExternalReferenceClient
    {
        private readonly ClientDiagnostics _clientDiagnostics;
        private readonly HttpPipeline _pipeline;
        internal DealExternalReferenceRestClient RestClient { get; }

        /// <summary> Initializes a new instance of DealExternalReferenceClient for mocking. </summary>
        protected DealExternalReferenceClient()
        {
        }

        /// <summary> Initializes a new instance of DealExternalReferenceClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        internal DealExternalReferenceClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
        {
            RestClient = new DealExternalReferenceRestClient(clientDiagnostics, pipeline, endpoint);
            _clientDiagnostics = clientDiagnostics;
            _pipeline = pipeline;
        }

        /// <param name="body"> The DealExternalReferenceDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<DealExternalReferenceDocument>> PostAsync(DealExternalReferenceDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceClient.Post");
            scope.Start();
            try
            {
                return await RestClient.PostAsync(body, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="body"> The DealExternalReferenceDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<DealExternalReferenceDocument> Post(DealExternalReferenceDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceClient.Post");
            scope.Start();
            try
            {
                return RestClient.Post(body, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<DealExternalReferenceDocument>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceClient.Get");
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

        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<DealExternalReferenceDocument> Get(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceClient.Get");
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

        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> HeadAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceClient.Head");
            scope.Start();
            try
            {
                return await RestClient.HeadAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response Head(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceClient.Head");
            scope.Start();
            try
            {
                return RestClient.Head(id, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The DealExternalReferenceDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<DealExternalReferenceDocument>> PatchAsync(int id, DealExternalReferenceDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceClient.Patch");
            scope.Start();
            try
            {
                return await RestClient.PatchAsync(id, body, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The DealExternalReferenceDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<DealExternalReferenceDocument> Patch(int id, DealExternalReferenceDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceClient.Patch");
            scope.Start();
            try
            {
                return RestClient.Patch(id, body, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceClient.Delete");
            scope.Start();
            try
            {
                return await RestClient.DeleteAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response Delete(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceClient.Delete");
            scope.Start();
            try
            {
                return RestClient.Delete(id, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
    }
}
