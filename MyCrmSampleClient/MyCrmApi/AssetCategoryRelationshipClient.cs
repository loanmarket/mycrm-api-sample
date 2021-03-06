// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Core.Pipeline;

namespace MyCrmSampleClient.MyCrmApi
{
    /// <summary> The AssetCategoryRelationship service client. </summary>
    public partial class AssetCategoryRelationshipClient
    {
        private readonly ClientDiagnostics _clientDiagnostics;
        private readonly HttpPipeline _pipeline;
        internal AssetCategoryRelationshipRestClient RestClient { get; }

        /// <summary> Initializes a new instance of AssetCategoryRelationshipClient for mocking. </summary>
        protected AssetCategoryRelationshipClient()
        {
        }

        /// <summary> Initializes a new instance of AssetCategoryRelationshipClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="clientDiagnostics"/> or <paramref name="pipeline"/> is null. </exception>
        internal AssetCategoryRelationshipClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
        {
            RestClient = new AssetCategoryRelationshipRestClient(clientDiagnostics, pipeline, endpoint);
            _clientDiagnostics = clientDiagnostics;
            _pipeline = pipeline;
        }

        /// <summary> Where `id` is the identifier of the asset category. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="relationshipName"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> GetAsync(int id, string relationshipName, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("AssetCategoryRelationshipClient.Get");
            scope.Start();
            try
            {
                return await RestClient.GetAsync(id, relationshipName, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the asset category. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="relationshipName"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response Get(int id, string relationshipName, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("AssetCategoryRelationshipClient.Get");
            scope.Start();
            try
            {
                return RestClient.Get(id, relationshipName, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
    }
}
