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
    /// <summary> The DealRelated service client. </summary>
    public partial class DealRelatedClient
    {
        private readonly ClientDiagnostics _clientDiagnostics;
        private readonly HttpPipeline _pipeline;
        internal DealRelatedRestClient RestClient { get; }

        /// <summary> Initializes a new instance of DealRelatedClient for mocking. </summary>
        protected DealRelatedClient()
        {
        }

        /// <summary> Initializes a new instance of DealRelatedClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        internal DealRelatedClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
        {
            RestClient = new DealRelatedRestClient(clientDiagnostics, pipeline, endpoint);
            _clientDiagnostics = clientDiagnostics;
            _pipeline = pipeline;
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<DealScenariosDocument>> GetDealScenariosAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealRelatedClient.GetDealScenarios");
            scope.Start();
            try
            {
                return await RestClient.GetDealScenariosAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<DealScenariosDocument> GetDealScenarios(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealRelatedClient.GetDealScenarios");
            scope.Start();
            try
            {
                return RestClient.GetDealScenarios(id, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<DealParticipantsDocument>> GetDealParticipantsAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealRelatedClient.GetDealParticipants");
            scope.Start();
            try
            {
                return await RestClient.GetDealParticipantsAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<DealParticipantsDocument> GetDealParticipants(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealRelatedClient.GetDealParticipants");
            scope.Start();
            try
            {
                return RestClient.GetDealParticipants(id, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<DealExternalReferencesDocument>> GetDealExternalReferencesAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealRelatedClient.GetDealExternalReferences");
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

        /// <summary> Where `id` is the identifier of the deal. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<DealExternalReferencesDocument> GetDealExternalReferences(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealRelatedClient.GetDealExternalReferences");
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

        /// <summary> Where `id` is the identifier of the deal. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<ContactsDocument>> GetContactsAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealRelatedClient.GetContacts");
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

        /// <summary> Where `id` is the identifier of the deal. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<ContactsDocument> GetContacts(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealRelatedClient.GetContacts");
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

        /// <summary> Where `id` is the identifier of the deal. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<DealImportantDatesDocument>> GetDealImportantDatesAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealRelatedClient.GetDealImportantDates");
            scope.Start();
            try
            {
                return await RestClient.GetDealImportantDatesAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<DealImportantDatesDocument> GetDealImportantDates(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealRelatedClient.GetDealImportantDates");
            scope.Start();
            try
            {
                return RestClient.GetDealImportantDates(id, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<DealStructuresDocument>> GetDealStructuresAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealRelatedClient.GetDealStructures");
            scope.Start();
            try
            {
                return await RestClient.GetDealStructuresAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<DealStructuresDocument> GetDealStructures(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealRelatedClient.GetDealStructures");
            scope.Start();
            try
            {
                return RestClient.GetDealStructures(id, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<AdvisersDocument>> GetAdvisersAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealRelatedClient.GetAdvisers");
            scope.Start();
            try
            {
                return await RestClient.GetAdvisersAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<AdvisersDocument> GetAdvisers(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealRelatedClient.GetAdvisers");
            scope.Start();
            try
            {
                return RestClient.GetAdvisers(id, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual async Task<Response<DealNotesDocument>> GetDealNotesAsync(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealRelatedClient.GetDealNotes");
            scope.Start();
            try
            {
                return await RestClient.GetDealNotesAsync(id, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public virtual Response<DealNotesDocument> GetDealNotes(int id, CancellationToken cancellationToken = default)
        {
            using var scope = _clientDiagnostics.CreateScope("DealRelatedClient.GetDealNotes");
            scope.Start();
            try
            {
                return RestClient.GetDealNotes(id, cancellationToken);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }
    }
}
