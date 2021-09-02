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
    /// <summary> The ContactGroupReferrer service client. </summary>
    public partial class ContactGroupReferrerClient
    {
        private readonly ClientDiagnostics _clientDiagnostics;
        private readonly HttpPipeline _pipeline;
        internal ContactGroupReferrerRestClient RestClient { get; }

        /// <summary> Initializes a new instance of ContactGroupReferrerClient for mocking. </summary>
        protected ContactGroupReferrerClient()
        {
        }

        /// <summary> Initializes a new instance of ContactGroupReferrerClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        internal ContactGroupReferrerClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
        {
            RestClient = new ContactGroupReferrerRestClient(clientDiagnostics, pipeline, endpoint);
            _clientDiagnostics = clientDiagnostics;
            _pipeline = pipeline;
        }

        /// <param name="body"> The ContactGroupReferrerDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<ContactGroupReferrerDocument>> PostAsync(ContactGroupReferrerDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ContactGroupReferrerClient.Post");
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

        /// <param name="body"> The ContactGroupReferrerDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<ContactGroupReferrerDocument> Post(ContactGroupReferrerDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ContactGroupReferrerClient.Post");
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
        public virtual async Task<Response<ContactGroupReferrerDocument>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ContactGroupReferrerClient.Get");
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
        public virtual Response<ContactGroupReferrerDocument> Get(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ContactGroupReferrerClient.Get");
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
        /// <param name="body"> The ContactGroupReferrerDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<ContactGroupReferrerDocument>> PatchAsync(int id, ContactGroupReferrerDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ContactGroupReferrerClient.Patch");
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
        /// <param name="body"> The ContactGroupReferrerDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<ContactGroupReferrerDocument> Patch(int id, ContactGroupReferrerDocument body = null, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("ContactGroupReferrerClient.Patch");
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
            using var scope = _clientDiagnostics.CreateScope("ContactGroupReferrerClient.Delete");
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
            using var scope = _clientDiagnostics.CreateScope("ContactGroupReferrerClient.Delete");
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