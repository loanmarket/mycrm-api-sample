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
    internal partial class ContactExternalReferenceRestClient
    {
        private Uri endpoint;
        private ClientDiagnostics _clientDiagnostics;
        private HttpPipeline _pipeline;

        /// <summary> Initializes a new instance of ContactExternalReferenceRestClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        public ContactExternalReferenceRestClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
        {
            this.endpoint = endpoint ?? new Uri("");
            _clientDiagnostics = clientDiagnostics;
            _pipeline = pipeline;
        }

        internal HttpMessage CreatePostRequest(ContactExternalReferenceDocument body)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Post;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/jsonapi/contact-external-references", false);
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

        /// <param name="body"> The ContactExternalReferenceDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async Task<ResponseWithHeaders<ContactExternalReferenceDocument, ContactExternalReferencePostHeaders>> PostAsync(ContactExternalReferenceDocument body = null, CancellationToken cancellationToken = default)
        {
            using var message = CreatePostRequest(body);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            var headers = new ContactExternalReferencePostHeaders(message.Response);
            switch (message.Response.Status)
            {
                case 201:
                    {
                        ContactExternalReferenceDocument value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = ContactExternalReferenceDocument.DeserializeContactExternalReferenceDocument(document.RootElement);
                        return ResponseWithHeaders.FromValue(value, headers, message.Response);
                    }
                case 401:
                    return ResponseWithHeaders.FromValue((ContactExternalReferenceDocument)null, headers, message.Response);
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <param name="body"> The ContactExternalReferenceDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public ResponseWithHeaders<ContactExternalReferenceDocument, ContactExternalReferencePostHeaders> Post(ContactExternalReferenceDocument body = null, CancellationToken cancellationToken = default)
        {
            using var message = CreatePostRequest(body);
            _pipeline.Send(message, cancellationToken);
            var headers = new ContactExternalReferencePostHeaders(message.Response);
            switch (message.Response.Status)
            {
                case 201:
                    {
                        ContactExternalReferenceDocument value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = ContactExternalReferenceDocument.DeserializeContactExternalReferenceDocument(document.RootElement);
                        return ResponseWithHeaders.FromValue(value, headers, message.Response);
                    }
                case 401:
                    return ResponseWithHeaders.FromValue((ContactExternalReferenceDocument)null, headers, message.Response);
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal HttpMessage CreateGetRequest(int id)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/jsonapi/contact-external-references/", false);
            uri.AppendPath(id, true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async Task<Response<ContactExternalReferenceDocument>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            using var message = CreateGetRequest(id);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        ContactExternalReferenceDocument value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = ContactExternalReferenceDocument.DeserializeContactExternalReferenceDocument(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                case 401:
                    return Response.FromValue((ContactExternalReferenceDocument)null, message.Response);
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response<ContactExternalReferenceDocument> Get(int id, CancellationToken cancellationToken = default)
        {
            using var message = CreateGetRequest(id);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        ContactExternalReferenceDocument value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = ContactExternalReferenceDocument.DeserializeContactExternalReferenceDocument(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                case 401:
                    return Response.FromValue((ContactExternalReferenceDocument)null, message.Response);
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal HttpMessage CreatePatchRequest(int id, ContactExternalReferenceDocument body)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Patch;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/jsonapi/contact-external-references/", false);
            uri.AppendPath(id, true);
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
        /// <param name="body"> The ContactExternalReferenceDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async Task<Response<ContactExternalReferenceDocument>> PatchAsync(int id, ContactExternalReferenceDocument body = null, CancellationToken cancellationToken = default)
        {
            using var message = CreatePatchRequest(id, body);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        ContactExternalReferenceDocument value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = ContactExternalReferenceDocument.DeserializeContactExternalReferenceDocument(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                case 204:
                case 401:
                    return Response.FromValue((ContactExternalReferenceDocument)null, message.Response);
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="body"> The ContactExternalReferenceDocument to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response<ContactExternalReferenceDocument> Patch(int id, ContactExternalReferenceDocument body = null, CancellationToken cancellationToken = default)
        {
            using var message = CreatePatchRequest(id, body);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        ContactExternalReferenceDocument value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = ContactExternalReferenceDocument.DeserializeContactExternalReferenceDocument(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                case 204:
                case 401:
                    return Response.FromValue((ContactExternalReferenceDocument)null, message.Response);
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }

        internal HttpMessage CreateDeleteRequest(int id)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Delete;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/jsonapi/contact-external-references/", false);
            uri.AppendPath(id, true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        /// <param name="id"> The Integer to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async Task<Response> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            using var message = CreateDeleteRequest(id);
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
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response Delete(int id, CancellationToken cancellationToken = default)
        {
            using var message = CreateDeleteRequest(id);
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
