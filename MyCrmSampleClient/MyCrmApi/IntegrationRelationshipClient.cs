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
    /// <summary> The IntegrationRelationship service client. </summary>
    public partial class IntegrationRelationshipClient
    {
        private readonly ClientDiagnostics _clientDiagnostics;
        private readonly HttpPipeline _pipeline;
        internal IntegrationRelationshipRestClient RestClient { get; }

        /// <summary> Initializes a new instance of IntegrationRelationshipClient for mocking. </summary>
        protected IntegrationRelationshipClient()
        {
        }

        /// <summary> Initializes a new instance of IntegrationRelationshipClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        internal IntegrationRelationshipClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
        {
            RestClient = new IntegrationRelationshipRestClient(clientDiagnostics, pipeline, endpoint);
            _clientDiagnostics = clientDiagnostics;
            _pipeline = pipeline;
        }

        /// <summary> &gt; ```id```: The identifier of the integration. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<RelationshipsMultipleDocument>> GetContactExternalReferencesAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("IntegrationRelationshipClient.GetContactExternalReferences");
            scope.Start();
            try
            {
                return await RestClient.GetContactExternalReferencesAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> &gt; ```id```: The identifier of the integration. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<RelationshipsMultipleDocument> GetContactExternalReferences(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("IntegrationRelationshipClient.GetContactExternalReferences");
            scope.Start();
            try
            {
                return RestClient.GetContactExternalReferences(id, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> &gt; ```id```: The identifier of the primary integration resource. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The RelationshipsMultipleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> PostContactExternalReferencesAsync(int id, RelationshipsMultipleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("IntegrationRelationshipClient.PostContactExternalReferences");
            scope.Start();
            try
            {
                return await RestClient.PostContactExternalReferencesAsync(id, body, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> &gt; ```id```: The identifier of the primary integration resource. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The RelationshipsMultipleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response PostContactExternalReferences(int id, RelationshipsMultipleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("IntegrationRelationshipClient.PostContactExternalReferences");
            scope.Start();
            try
            {
                return RestClient.PostContactExternalReferences(id, body, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> &gt; ```id```: The identifier of the integration. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The RelationshipsMultipleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> PatchContactExternalReferencesAsync(int id, RelationshipsMultipleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("IntegrationRelationshipClient.PatchContactExternalReferences");
            scope.Start();
            try
            {
                return await RestClient.PatchContactExternalReferencesAsync(id, body, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> &gt; ```id```: The identifier of the integration. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The RelationshipsMultipleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response PatchContactExternalReferences(int id, RelationshipsMultipleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("IntegrationRelationshipClient.PatchContactExternalReferences");
            scope.Start();
            try
            {
                return RestClient.PatchContactExternalReferences(id, body, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> &gt; ```id```: The identifier of the integration. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> DeleteContactExternalReferencesAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("IntegrationRelationshipClient.DeleteContactExternalReferences");
            scope.Start();
            try
            {
                return await RestClient.DeleteContactExternalReferencesAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> &gt; ```id```: The identifier of the integration. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response DeleteContactExternalReferences(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("IntegrationRelationshipClient.DeleteContactExternalReferences");
            scope.Start();
            try
            {
                return RestClient.DeleteContactExternalReferences(id, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> &gt; ```id```: The identifier of the integration. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<RelationshipsMultipleDocument>> GetDealExternalReferencesAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("IntegrationRelationshipClient.GetDealExternalReferences");
            scope.Start();
            try
            {
                return await RestClient.GetDealExternalReferencesAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> &gt; ```id```: The identifier of the integration. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<RelationshipsMultipleDocument> GetDealExternalReferences(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("IntegrationRelationshipClient.GetDealExternalReferences");
            scope.Start();
            try
            {
                return RestClient.GetDealExternalReferences(id, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> &gt; ```id```: The identifier of the primary integration resource. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The RelationshipsMultipleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> PostDealExternalReferencesAsync(int id, RelationshipsMultipleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("IntegrationRelationshipClient.PostDealExternalReferences");
            scope.Start();
            try
            {
                return await RestClient.PostDealExternalReferencesAsync(id, body, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> &gt; ```id```: The identifier of the primary integration resource. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The RelationshipsMultipleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response PostDealExternalReferences(int id, RelationshipsMultipleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("IntegrationRelationshipClient.PostDealExternalReferences");
            scope.Start();
            try
            {
                return RestClient.PostDealExternalReferences(id, body, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> &gt; ```id```: The identifier of the integration. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The RelationshipsMultipleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> PatchDealExternalReferencesAsync(int id, RelationshipsMultipleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("IntegrationRelationshipClient.PatchDealExternalReferences");
            scope.Start();
            try
            {
                return await RestClient.PatchDealExternalReferencesAsync(id, body, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> &gt; ```id```: The identifier of the integration. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The RelationshipsMultipleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response PatchDealExternalReferences(int id, RelationshipsMultipleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("IntegrationRelationshipClient.PatchDealExternalReferences");
            scope.Start();
            try
            {
                return RestClient.PatchDealExternalReferences(id, body, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> &gt; ```id```: The identifier of the integration. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> DeleteDealExternalReferencesAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("IntegrationRelationshipClient.DeleteDealExternalReferences");
            scope.Start();
            try
            {
                return await RestClient.DeleteDealExternalReferencesAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> &gt; ```id```: The identifier of the integration. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response DeleteDealExternalReferences(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("IntegrationRelationshipClient.DeleteDealExternalReferences");
            scope.Start();
            try
            {
                return RestClient.DeleteDealExternalReferences(id, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
    }
}
