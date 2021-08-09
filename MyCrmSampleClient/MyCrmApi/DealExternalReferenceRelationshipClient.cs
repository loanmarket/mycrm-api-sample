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
    /// <summary> The DealExternalReferenceRelationship service client. </summary>
    public partial class DealExternalReferenceRelationshipClient
    {
        private readonly ClientDiagnostics _clientDiagnostics;
        private readonly HttpPipeline _pipeline;
        internal DealExternalReferenceRelationshipRestClient RestClient { get; }

        /// <summary> Initializes a new instance of DealExternalReferenceRelationshipClient for mocking. </summary>
        protected DealExternalReferenceRelationshipClient()
        {
        }

        /// <summary> Initializes a new instance of DealExternalReferenceRelationshipClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        internal DealExternalReferenceRelationshipClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
        {
            RestClient = new DealExternalReferenceRelationshipRestClient(clientDiagnostics, pipeline, endpoint);
            _clientDiagnostics = clientDiagnostics;
            _pipeline = pipeline;
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<ResourceIdentifier>> GetDealsAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceRelationshipClient.GetDeals");
            scope.Start();
            try
            {
                return await RestClient.GetDealsAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<ResourceIdentifier> GetDeals(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceRelationshipClient.GetDeals");
            scope.Start();
            try
            {
                return RestClient.GetDeals(id, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The ResourceIdentifier to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> PostDealsAsync(int id, ResourceIdentifier body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceRelationshipClient.PostDeals");
            scope.Start();
            try
            {
                return await RestClient.PostDealsAsync(id, body, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The ResourceIdentifier to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response PostDeals(int id, ResourceIdentifier body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceRelationshipClient.PostDeals");
            scope.Start();
            try
            {
                return RestClient.PostDeals(id, body, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The ResourceIdentifier to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> PatchDealsAsync(int id, ResourceIdentifier body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceRelationshipClient.PatchDeals");
            scope.Start();
            try
            {
                return await RestClient.PatchDealsAsync(id, body, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The ResourceIdentifier to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response PatchDeals(int id, ResourceIdentifier body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceRelationshipClient.PatchDeals");
            scope.Start();
            try
            {
                return RestClient.PatchDeals(id, body, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> DeleteDealsAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceRelationshipClient.DeleteDeals");
            scope.Start();
            try
            {
                return await RestClient.DeleteDealsAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response DeleteDeals(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceRelationshipClient.DeleteDeals");
            scope.Start();
            try
            {
                return RestClient.DeleteDeals(id, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<ResourceIdentifier>> GetIntegrationsAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceRelationshipClient.GetIntegrations");
            scope.Start();
            try
            {
                return await RestClient.GetIntegrationsAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<ResourceIdentifier> GetIntegrations(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceRelationshipClient.GetIntegrations");
            scope.Start();
            try
            {
                return RestClient.GetIntegrations(id, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The ResourceIdentifier to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> PostIntegrationsAsync(int id, ResourceIdentifier body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceRelationshipClient.PostIntegrations");
            scope.Start();
            try
            {
                return await RestClient.PostIntegrationsAsync(id, body, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The ResourceIdentifier to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response PostIntegrations(int id, ResourceIdentifier body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceRelationshipClient.PostIntegrations");
            scope.Start();
            try
            {
                return RestClient.PostIntegrations(id, body, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The ResourceIdentifier to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> PatchIntegrationsAsync(int id, ResourceIdentifier body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceRelationshipClient.PatchIntegrations");
            scope.Start();
            try
            {
                return await RestClient.PatchIntegrationsAsync(id, body, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The ResourceIdentifier to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response PatchIntegrations(int id, ResourceIdentifier body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceRelationshipClient.PatchIntegrations");
            scope.Start();
            try
            {
                return RestClient.PatchIntegrations(id, body, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> DeleteIntegrationsAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceRelationshipClient.DeleteIntegrations");
            scope.Start();
            try
            {
                return await RestClient.DeleteIntegrationsAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response DeleteIntegrations(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealExternalReferenceRelationshipClient.DeleteIntegrations");
            scope.Start();
            try
            {
                return RestClient.DeleteIntegrations(id, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
    }
}
