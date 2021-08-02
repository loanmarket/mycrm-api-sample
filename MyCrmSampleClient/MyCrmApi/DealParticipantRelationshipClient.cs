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
    /// <summary> The DealParticipantRelationship service client. </summary>
    public partial class DealParticipantRelationshipClient
    {
        private readonly ClientDiagnostics _clientDiagnostics;
        private readonly HttpPipeline _pipeline;
        internal DealParticipantRelationshipRestClient RestClient { get; }

        /// <summary> Initializes a new instance of DealParticipantRelationshipClient for mocking. </summary>
        protected DealParticipantRelationshipClient()
        {
        }

        /// <summary> Initializes a new instance of DealParticipantRelationshipClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        internal DealParticipantRelationshipClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
        {
            RestClient = new DealParticipantRelationshipRestClient(clientDiagnostics, pipeline, endpoint);
            _clientDiagnostics = clientDiagnostics;
            _pipeline = pipeline;
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="relationshipName"> The DealParticipantRelationship to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<RelationshipsMultipleDocument>> GetAsync(int id, DealParticipantRelationship relationshipName, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealParticipantRelationshipClient.Get");
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

        /// <param name="id"> The Integer to use. </param>
        /// <param name="relationshipName"> The DealParticipantRelationship to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<RelationshipsMultipleDocument> Get(int id, DealParticipantRelationship relationshipName, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealParticipantRelationshipClient.Get");
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

        /// <param name="id"> The Integer to use. </param>
        /// <param name="relationshipName"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> GetHeadAsync(int id, string relationshipName, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealParticipantRelationshipClient.GetHead");
            scope.Start();
            try
            {
                return await RestClient.GetHeadAsync(id, relationshipName, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="relationshipName"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response GetHead(int id, string relationshipName, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealParticipantRelationshipClient.GetHead");
            scope.Start();
            try
            {
                return RestClient.GetHead(id, relationshipName, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="relationshipName"> The DealParticipantRelationship to use. </param>
        /// <param name="body"> The RelationshipsMultipleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> PostAsync(int id, DealParticipantRelationship relationshipName, RelationshipsMultipleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealParticipantRelationshipClient.Post");
            scope.Start();
            try
            {
                return await RestClient.PostAsync(id, relationshipName, body, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="relationshipName"> The DealParticipantRelationship to use. </param>
        /// <param name="body"> The RelationshipsMultipleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response Post(int id, DealParticipantRelationship relationshipName, RelationshipsMultipleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealParticipantRelationshipClient.Post");
            scope.Start();
            try
            {
                return RestClient.Post(id, relationshipName, body, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="relationshipName"> The DealParticipantRelationship to use. </param>
        /// <param name="body"> The RelationshipsMultipleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> PatchAsync(int id, DealParticipantRelationship relationshipName, RelationshipsMultipleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealParticipantRelationshipClient.Patch");
            scope.Start();
            try
            {
                return await RestClient.PatchAsync(id, relationshipName, body, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="relationshipName"> The DealParticipantRelationship to use. </param>
        /// <param name="body"> The RelationshipsMultipleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response Patch(int id, DealParticipantRelationship relationshipName, RelationshipsMultipleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealParticipantRelationshipClient.Patch");
            scope.Start();
            try
            {
                return RestClient.Patch(id, relationshipName, body, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="relationshipName"> The DealParticipantRelationship to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> DeleteAsync(int id, DealParticipantRelationship relationshipName, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealParticipantRelationshipClient.Delete");
            scope.Start();
            try
            {
                return await RestClient.DeleteAsync(id, relationshipName, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="relationshipName"> The DealParticipantRelationship to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response Delete(int id, DealParticipantRelationship relationshipName, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealParticipantRelationshipClient.Delete");
            scope.Start();
            try
            {
                return RestClient.Delete(id, relationshipName, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
    }
}
