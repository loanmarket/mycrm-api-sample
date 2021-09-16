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
    /// <summary> The ContactExternalReferenceRelationship service client. </summary>
    public partial class ContactExternalReferenceRelationshipClient
    {
        private readonly ClientDiagnostics _clientDiagnostics;
        private readonly HttpPipeline _pipeline;
        internal ContactExternalReferenceRelationshipRestClient RestClient { get; }

        /// <summary> Initializes a new instance of ContactExternalReferenceRelationshipClient for mocking. </summary>
        protected ContactExternalReferenceRelationshipClient()
        {
        }

        /// <summary> Initializes a new instance of ContactExternalReferenceRelationshipClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        internal ContactExternalReferenceRelationshipClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
        {
            RestClient = new ContactExternalReferenceRelationshipRestClient(clientDiagnostics, pipeline, endpoint);
            _clientDiagnostics = clientDiagnostics;
            _pipeline = pipeline;
        }

        /// <summary> &gt; ```id```: The identifier of the contact external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<RelationshipsSingleDocument>> GetContactsAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ContactExternalReferenceRelationshipClient.GetContacts");
            scope.Start();
            try
            {
                return await RestClient.GetContactsAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> &gt; ```id```: The identifier of the contact external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<RelationshipsSingleDocument> GetContacts(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ContactExternalReferenceRelationshipClient.GetContacts");
            scope.Start();
            try
            {
                return RestClient.GetContacts(id, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> &gt; ```id```: The identifier of the primary contact external reference resource. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The RelationshipsSingleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> PostContactsAsync(int id, RelationshipsSingleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ContactExternalReferenceRelationshipClient.PostContacts");
            scope.Start();
            try
            {
                return await RestClient.PostContactsAsync(id, body, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> &gt; ```id```: The identifier of the primary contact external reference resource. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The RelationshipsSingleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response PostContacts(int id, RelationshipsSingleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ContactExternalReferenceRelationshipClient.PostContacts");
            scope.Start();
            try
            {
                return RestClient.PostContacts(id, body, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> &gt; ```id```: The identifier of the contact external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The RelationshipsSingleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> PatchContactsAsync(int id, RelationshipsSingleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ContactExternalReferenceRelationshipClient.PatchContacts");
            scope.Start();
            try
            {
                return await RestClient.PatchContactsAsync(id, body, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> &gt; ```id```: The identifier of the contact external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The RelationshipsSingleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response PatchContacts(int id, RelationshipsSingleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ContactExternalReferenceRelationshipClient.PatchContacts");
            scope.Start();
            try
            {
                return RestClient.PatchContacts(id, body, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> &gt; ```id```: The identifier of the contact external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> DeleteContactsAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ContactExternalReferenceRelationshipClient.DeleteContacts");
            scope.Start();
            try
            {
                return await RestClient.DeleteContactsAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> &gt; ```id```: The identifier of the contact external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response DeleteContacts(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ContactExternalReferenceRelationshipClient.DeleteContacts");
            scope.Start();
            try
            {
                return RestClient.DeleteContacts(id, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> &gt; ```id```: The identifier of the contact external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<RelationshipsSingleDocument>> GetIntegrationsAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ContactExternalReferenceRelationshipClient.GetIntegrations");
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

        /// <summary> &gt; ```id```: The identifier of the contact external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<RelationshipsSingleDocument> GetIntegrations(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ContactExternalReferenceRelationshipClient.GetIntegrations");
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

        /// <summary> &gt; ```id```: The identifier of the primary contact external reference resource. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The RelationshipsSingleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> PostIntegrationsAsync(int id, RelationshipsSingleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ContactExternalReferenceRelationshipClient.PostIntegrations");
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

        /// <summary> &gt; ```id```: The identifier of the primary contact external reference resource. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The RelationshipsSingleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response PostIntegrations(int id, RelationshipsSingleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ContactExternalReferenceRelationshipClient.PostIntegrations");
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

        /// <summary> &gt; ```id```: The identifier of the contact external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The RelationshipsSingleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> PatchIntegrationsAsync(int id, RelationshipsSingleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ContactExternalReferenceRelationshipClient.PatchIntegrations");
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

        /// <summary> &gt; ```id```: The identifier of the contact external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The RelationshipsSingleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response PatchIntegrations(int id, RelationshipsSingleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ContactExternalReferenceRelationshipClient.PatchIntegrations");
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

        /// <summary> &gt; ```id```: The identifier of the contact external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response> DeleteIntegrationsAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ContactExternalReferenceRelationshipClient.DeleteIntegrations");
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

        /// <summary> &gt; ```id```: The identifier of the contact external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response DeleteIntegrations(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ContactExternalReferenceRelationshipClient.DeleteIntegrations");
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
