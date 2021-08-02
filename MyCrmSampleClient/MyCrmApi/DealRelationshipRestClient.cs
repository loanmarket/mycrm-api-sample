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
    internal partial class DealRelationshipRestClient
    {
        private Uri endpoint;
        private ClientDiagnostics _clientDiagnostics;
        private HttpPipeline _pipeline;

        /// <summary> Initializes a new instance of DealRelationshipRestClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        public DealRelationshipRestClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
        {
            this.endpoint = endpoint ?? new Uri("");
            _clientDiagnostics = clientDiagnostics;
            _pipeline = pipeline;
        }

        internal HttpMessage CreateGetRequest(int id, DealRelationship relationshipName)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/jsonapi/deals/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/relationships/", false);
            uri.AppendPath(relationshipName.ToString(), true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="relationshipName"> The DealRelationship to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async Task<Response<RelationshipsMultipleDocument>> GetAsync(int id, DealRelationship relationshipName, CancellationToken cancellationToken = default)
        {
            using var message = CreateGetRequest(id, relationshipName);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        RelationshipsMultipleDocument value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = RelationshipsMultipleDocument.DeserializeRelationshipsMultipleDocument(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                case 401:
                    return Response.FromValue((RelationshipsMultipleDocument)null, message.Response);
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="relationshipName"> The DealRelationship to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response<RelationshipsMultipleDocument> Get(int id, DealRelationship relationshipName, CancellationToken cancellationToken = default)
        {
            using var message = CreateGetRequest(id, relationshipName);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        RelationshipsMultipleDocument value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = RelationshipsMultipleDocument.DeserializeRelationshipsMultipleDocument(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                case 401:
                    return Response.FromValue((RelationshipsMultipleDocument)null, message.Response);
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal HttpMessage CreateGetHeadRequest(int id, string relationshipName)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Head;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/jsonapi/deals/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/relationships/", false);
            uri.AppendPath(relationshipName, true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="relationshipName"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="relationshipName"/> is null. </exception>
        public async Task<Response> GetHeadAsync(int id, string relationshipName, CancellationToken cancellationToken = default)
        {
            if (relationshipName == null)
            {
                throw new ArgumentNullException(nameof(relationshipName));
            }

            using var message = CreateGetHeadRequest(id, relationshipName);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                case 401:
                    return message.Response;
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="relationshipName"> The String to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        /// <exception cref="ArgumentNullException"> <paramref name="relationshipName"/> is null. </exception>
        public Response GetHead(int id, string relationshipName, CancellationToken cancellationToken = default)
        {
            if (relationshipName == null)
            {
                throw new ArgumentNullException(nameof(relationshipName));
            }

            using var message = CreateGetHeadRequest(id, relationshipName);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                case 401:
                    return message.Response;
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal HttpMessage CreatePostRequest(int id, DealRelationship relationshipName, RelationshipsMultipleDocument body)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Post;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/jsonapi/deals/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/relationships/", false);
            uri.AppendPath(relationshipName.ToString(), true);
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

        /// <param name="id"> The Integer to use. </param>
        /// <param name="relationshipName"> The DealRelationship to use. </param>
        /// <param name="body"> The RelationshipsMultipleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async Task<Response> PostAsync(int id, DealRelationship relationshipName, RelationshipsMultipleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var message = CreatePostRequest(id, relationshipName, body);
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

        /// <param name="id"> The Integer to use. </param>
        /// <param name="relationshipName"> The DealRelationship to use. </param>
        /// <param name="body"> The RelationshipsMultipleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response Post(int id, DealRelationship relationshipName, RelationshipsMultipleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var message = CreatePostRequest(id, relationshipName, body);
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

        internal HttpMessage CreatePatchRequest(int id, DealRelationship relationshipName, RelationshipsMultipleDocument body)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Patch;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/jsonapi/deals/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/relationships/", false);
            uri.AppendPath(relationshipName.ToString(), true);
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

        /// <param name="id"> The Integer to use. </param>
        /// <param name="relationshipName"> The DealRelationship to use. </param>
        /// <param name="body"> The RelationshipsMultipleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async Task<Response> PatchAsync(int id, DealRelationship relationshipName, RelationshipsMultipleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var message = CreatePatchRequest(id, relationshipName, body);
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

        /// <param name="id"> The Integer to use. </param>
        /// <param name="relationshipName"> The DealRelationship to use. </param>
        /// <param name="body"> The RelationshipsMultipleDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response Patch(int id, DealRelationship relationshipName, RelationshipsMultipleDocument body = null, CancellationToken cancellationToken = default)
        {
            using var message = CreatePatchRequest(id, relationshipName, body);
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

        internal HttpMessage CreateDeleteRequest(int id, DealRelationship relationshipName)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Delete;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/jsonapi/deals/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/relationships/", false);
            uri.AppendPath(relationshipName.ToString(), true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="relationshipName"> The DealRelationship to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async Task<Response> DeleteAsync(int id, DealRelationship relationshipName, CancellationToken cancellationToken = default)
        {
            using var message = CreateDeleteRequest(id, relationshipName);
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

        /// <param name="id"> The Integer to use. </param>
        /// <param name="relationshipName"> The DealRelationship to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response Delete(int id, DealRelationship relationshipName, CancellationToken cancellationToken = default)
        {
            using var message = CreateDeleteRequest(id, relationshipName);
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
