// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Core.Pipeline;
using MyCrmSampleClient.MyCrmApi.Models;

namespace MyCrmSampleClient.MyCrmApi
{
    internal partial class DealExternalReferenceRelatedRestClient
    {
        private Uri endpoint;
        private ClientDiagnostics _clientDiagnostics;
        private HttpPipeline _pipeline;

        /// <summary> Initializes a new instance of DealExternalReferenceRelatedRestClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        public DealExternalReferenceRelatedRestClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
        {
            this.endpoint = endpoint ?? new Uri("");
            _clientDiagnostics = clientDiagnostics;
            _pipeline = pipeline;
        }

        internal HttpMessage CreateGetDealsRequest(int id)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/jsonapi/deal-external-references/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/deal", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        /// <summary> Where `id` is the identifier of the deal external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async Task<Response<DealsDocument>> GetDealsAsync(int id, CancellationToken cancellationToken = default)
        {
            using var message = CreateGetDealsRequest(id);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        DealsDocument value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = DealsDocument.DeserializeDealsDocument(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                case 401:
                    return Response.FromValue((DealsDocument)null, message.Response);
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <summary> Where `id` is the identifier of the deal external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response<DealsDocument> GetDeals(int id, CancellationToken cancellationToken = default)
        {
            using var message = CreateGetDealsRequest(id);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        DealsDocument value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = DealsDocument.DeserializeDealsDocument(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                case 401:
                    return Response.FromValue((DealsDocument)null, message.Response);
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal HttpMessage CreateGetIntegrationsRequest(int id)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/jsonapi/deal-external-references/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/integration", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        /// <summary> Where `id` is the identifier of the deal external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async Task<Response<IntegrationsDocument>> GetIntegrationsAsync(int id, CancellationToken cancellationToken = default)
        {
            using var message = CreateGetIntegrationsRequest(id);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        IntegrationsDocument value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = IntegrationsDocument.DeserializeIntegrationsDocument(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                case 401:
                    return Response.FromValue((IntegrationsDocument)null, message.Response);
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <summary> Where `id` is the identifier of the deal external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response<IntegrationsDocument> GetIntegrations(int id, CancellationToken cancellationToken = default)
        {
            using var message = CreateGetIntegrationsRequest(id);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        IntegrationsDocument value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = IntegrationsDocument.DeserializeIntegrationsDocument(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                case 401:
                    return Response.FromValue((IntegrationsDocument)null, message.Response);
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }
    }
}
