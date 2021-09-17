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
    internal partial class DealExternalReferenceRelationshipRestClient
    {
        private Uri endpoint;
        private ClientDiagnostics _clientDiagnostics;
        private HttpPipeline _pipeline;

        /// <summary> Initializes a new instance of DealExternalReferenceRelationshipRestClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        public DealExternalReferenceRelationshipRestClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
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
            uri.AppendPath("/relationships/deal", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        /// <summary> Where `id` is the identifier of the deal external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async Task<Response<RelationshipsSingleDocument>> GetDealsAsync(int id, CancellationToken cancellationToken = default)
        {
            using var message = CreateGetDealsRequest(id);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        RelationshipsSingleDocument value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = RelationshipsSingleDocument.DeserializeRelationshipsSingleDocument(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                case 401:
                    return Response.FromValue((RelationshipsSingleDocument)null, message.Response);
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <summary> Where `id` is the identifier of the deal external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response<RelationshipsSingleDocument> GetDeals(int id, CancellationToken cancellationToken = default)
        {
            using var message = CreateGetDealsRequest(id);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        RelationshipsSingleDocument value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = RelationshipsSingleDocument.DeserializeRelationshipsSingleDocument(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                case 401:
                    return Response.FromValue((RelationshipsSingleDocument)null, message.Response);
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal HttpMessage CreatePostDealsRequest(int id, RelationshipsSingleDocument body)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Post;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/jsonapi/deal-external-references/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/relationships/deal", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            if (body != null)
            {
                request.Headers.Add("Content-Type", "application/vnd.api+json");
                var content = new Utf8JsonRequestContent();
                content.JsonWriter.WriteObjectValue(body);
                request.Content = content;
            }
            return message;
        }

        /// <summary> Where `id` is the identifier of the primary deal external reference resource. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The RelationshipsSingleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async Task<Response> PostDealsAsync(int id, RelationshipsSingleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var message = CreatePostDealsRequest(id, body);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                case 204:
                case 401:
                    return message.Response;
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <summary> Where `id` is the identifier of the primary deal external reference resource. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The RelationshipsSingleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response PostDeals(int id, RelationshipsSingleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var message = CreatePostDealsRequest(id, body);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                case 204:
                case 401:
                    return message.Response;
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal HttpMessage CreatePatchDealsRequest(int id, RelationshipsSingleDocument body)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Patch;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/jsonapi/deal-external-references/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/relationships/deal", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            if (body != null)
            {
                request.Headers.Add("Content-Type", "application/vnd.api+json");
                var content = new Utf8JsonRequestContent();
                content.JsonWriter.WriteObjectValue(body);
                request.Content = content;
            }
            return message;
        }

        /// <summary> Where `id` is the identifier of the deal external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The RelationshipsSingleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async Task<Response> PatchDealsAsync(int id, RelationshipsSingleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var message = CreatePatchDealsRequest(id, body);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                case 204:
                case 401:
                    return message.Response;
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <summary> Where `id` is the identifier of the deal external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The RelationshipsSingleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response PatchDeals(int id, RelationshipsSingleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var message = CreatePatchDealsRequest(id, body);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                case 204:
                case 401:
                    return message.Response;
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal HttpMessage CreateDeleteDealsRequest(int id)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Delete;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/jsonapi/deal-external-references/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/relationships/deal", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        /// <summary> Where `id` is the identifier of the deal external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async Task<Response> DeleteDealsAsync(int id, CancellationToken cancellationToken = default)
        {
            using var message = CreateDeleteDealsRequest(id);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 204:
                case 401:
                    return message.Response;
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <summary> Where `id` is the identifier of the deal external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response DeleteDeals(int id, CancellationToken cancellationToken = default)
        {
            using var message = CreateDeleteDealsRequest(id);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 204:
                case 401:
                    return message.Response;
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
            uri.AppendPath("/relationships/integration", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        /// <summary> Where `id` is the identifier of the deal external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async Task<Response<RelationshipsSingleDocument>> GetIntegrationsAsync(int id, CancellationToken cancellationToken = default)
        {
            using var message = CreateGetIntegrationsRequest(id);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        RelationshipsSingleDocument value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = RelationshipsSingleDocument.DeserializeRelationshipsSingleDocument(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                case 401:
                    return Response.FromValue((RelationshipsSingleDocument)null, message.Response);
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <summary> Where `id` is the identifier of the deal external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response<RelationshipsSingleDocument> GetIntegrations(int id, CancellationToken cancellationToken = default)
        {
            using var message = CreateGetIntegrationsRequest(id);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        RelationshipsSingleDocument value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = RelationshipsSingleDocument.DeserializeRelationshipsSingleDocument(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                case 401:
                    return Response.FromValue((RelationshipsSingleDocument)null, message.Response);
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal HttpMessage CreatePostIntegrationsRequest(int id, RelationshipsSingleDocument body)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Post;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/jsonapi/deal-external-references/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/relationships/integration", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            if (body != null)
            {
                request.Headers.Add("Content-Type", "application/vnd.api+json");
                var content = new Utf8JsonRequestContent();
                content.JsonWriter.WriteObjectValue(body);
                request.Content = content;
            }
            return message;
        }

        /// <summary> Where `id` is the identifier of the primary deal external reference resource. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The RelationshipsSingleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async Task<Response> PostIntegrationsAsync(int id, RelationshipsSingleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var message = CreatePostIntegrationsRequest(id, body);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                case 204:
                case 401:
                    return message.Response;
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <summary> Where `id` is the identifier of the primary deal external reference resource. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The RelationshipsSingleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response PostIntegrations(int id, RelationshipsSingleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var message = CreatePostIntegrationsRequest(id, body);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                case 204:
                case 401:
                    return message.Response;
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal HttpMessage CreatePatchIntegrationsRequest(int id, RelationshipsSingleDocument body)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Patch;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/jsonapi/deal-external-references/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/relationships/integration", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            if (body != null)
            {
                request.Headers.Add("Content-Type", "application/vnd.api+json");
                var content = new Utf8JsonRequestContent();
                content.JsonWriter.WriteObjectValue(body);
                request.Content = content;
            }
            return message;
        }

        /// <summary> Where `id` is the identifier of the deal external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The RelationshipsSingleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async Task<Response> PatchIntegrationsAsync(int id, RelationshipsSingleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var message = CreatePatchIntegrationsRequest(id, body);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                case 204:
                case 401:
                    return message.Response;
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <summary> Where `id` is the identifier of the deal external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The RelationshipsSingleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response PatchIntegrations(int id, RelationshipsSingleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var message = CreatePatchIntegrationsRequest(id, body);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                case 204:
                case 401:
                    return message.Response;
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal HttpMessage CreateDeleteIntegrationsRequest(int id)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Delete;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/jsonapi/deal-external-references/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/relationships/integration", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        /// <summary> Where `id` is the identifier of the deal external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async Task<Response> DeleteIntegrationsAsync(int id, CancellationToken cancellationToken = default)
        {
            using var message = CreateDeleteIntegrationsRequest(id);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 204:
                case 401:
                    return message.Response;
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <summary> Where `id` is the identifier of the deal external reference. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response DeleteIntegrations(int id, CancellationToken cancellationToken = default)
        {
            using var message = CreateDeleteIntegrationsRequest(id);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 204:
                case 401:
                    return message.Response;
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }
    }
}
