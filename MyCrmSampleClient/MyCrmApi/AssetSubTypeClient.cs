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
    /// <summary> The AssetSubType service client. </summary>
    public partial class AssetSubTypeClient
    {
        private readonly ClientDiagnostics _clientDiagnostics;
        private readonly HttpPipeline _pipeline;
        internal AssetSubTypeRestClient RestClient { get; }

        /// <summary> Initializes a new instance of AssetSubTypeClient for mocking. </summary>
        protected AssetSubTypeClient()
        {
        }

        /// <summary> Initializes a new instance of AssetSubTypeClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        internal AssetSubTypeClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
        {
            RestClient = new AssetSubTypeRestClient(clientDiagnostics, pipeline, endpoint);
            _clientDiagnostics = clientDiagnostics;
            _pipeline = pipeline;
        }

        /// <summary> Where `id` is the identifier of the asset sub-type. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<AssetSubTypeDocument>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("AssetSubTypeClient.Get");
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

        /// <summary> Where `id` is the identifier of the asset sub-type. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<AssetSubTypeDocument> Get(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("AssetSubTypeClient.Get");
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