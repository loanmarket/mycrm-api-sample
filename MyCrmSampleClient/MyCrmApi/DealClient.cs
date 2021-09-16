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
    /// <summary> The Deal service client. </summary>
    public partial class DealClient
    {
        private readonly ClientDiagnostics _clientDiagnostics;
        private readonly HttpPipeline _pipeline;
        internal DealRestClient RestClient { get; }

        /// <summary> Initializes a new instance of DealClient for mocking. </summary>
        protected DealClient()
        {
        }

        /// <summary> Initializes a new instance of DealClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        internal DealClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
        {
            RestClient = new DealRestClient(clientDiagnostics, pipeline, endpoint);
            _clientDiagnostics = clientDiagnostics;
            _pipeline = pipeline;
        }

        /// <summary> Creates a new deal with attributes, relationships or both. </summary>
        /// <param name="body"> The DealDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<DealDocument>> PostAsync(DealDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealClient.Post");
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

        /// <summary> Creates a new deal with attributes, relationships or both. </summary>
        /// <param name="body"> The DealDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<DealDocument> Post(DealDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealClient.Post");
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

        /// <summary> &gt; ```id```: The identifier of the deal. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<DealDocument>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealClient.Get");
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

        /// <summary> &gt; ```id```: The identifier of the deal. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<DealDocument> Get(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealClient.Get");
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

        /// <summary> &gt; ```id```: The identifier of the deal. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The DealDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<DealDocument>> PatchAsync(int id, DealDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealClient.Patch");
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

        /// <summary> &gt; ```id```: The identifier of the deal. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The DealDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<DealDocument> Patch(int id, DealDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealClient.Patch");
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

        /// <summary> &gt; ```id```: The identifier of the deal. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealClient.Delete");
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

        /// <summary> &gt; ```id```: The identifier of the deal. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response Delete(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealClient.Delete");
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
