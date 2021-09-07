// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Core.Pipeline;
using MyCrmSampleClient.MyCrmApi.Models;

namespace MyCrmSampleClient.MyCrmApi
{
    internal partial class AdviserDetailsRestClient
    {
        private Uri endpoint;
        private ClientDiagnostics _clientDiagnostics;
        private HttpPipeline _pipeline;

        /// <summary> Initializes a new instance of AdviserDetailsRestClient. </summary>
        /// <param name="clientDiagnostics"> The handler for diagnostic messaging in the client. </param>
        /// <param name="pipeline"> The HTTP pipeline for sending and receiving REST requests and responses. </param>
        /// <param name="endpoint"> server parameter. </param>
        public AdviserDetailsRestClient(ClientDiagnostics clientDiagnostics, HttpPipeline pipeline, Uri endpoint = null)
        {
            this.endpoint = endpoint ?? new Uri("");
            _clientDiagnostics = clientDiagnostics;
            _pipeline = pipeline;
        }

        internal HttpMessage CreateGetRequest(IEnumerable<string> filter, IEnumerable<string> sort, Pagination page, IEnumerable<string> include)
        {
            var message = _pipeline.CreateMessage();
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(endpoint);
            uri.AppendPath("/jsonapi/adviser-details", false);
            if (filter != null)
            {
                uri.AppendQueryDelimited("filter", filter, ",", false);
            }
            if (sort != null)
            {
                uri.AppendQueryDelimited("sort", sort, ",", false);
            }
            if (page != null)
            {
                uri.AppendQuery("page", page, true);
            }
            if (include != null)
            {
                uri.AppendQueryDelimited("include", include, ",", true);
            }
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        /// <param name="filter"> The ArrayOfString to use. </param>
        /// <param name="sort"> The ArrayOfString to use. </param>
        /// <param name="page"> The Pagination to use. </param>
        /// <param name="include"> The ArrayOfString to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public async Task<Response<AdviserDetailsDocument>> GetAsync(IEnumerable<string> filter = null, IEnumerable<string> sort = null, Pagination page = null, IEnumerable<string> include = null, CancellationToken cancellationToken = default)
        {
            using var message = CreateGetRequest(filter, sort, page, include);
            await _pipeline.SendAsync(message, cancellationToken).ConfigureAwait(false);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        AdviserDetailsDocument value = default;
                        using var document = await JsonDocument.ParseAsync(message.Response.ContentStream, default, cancellationToken).ConfigureAwait(false);
                        value = AdviserDetailsDocument.DeserializeAdviserDetailsDocument(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                case 401:
                    return Response.FromValue((AdviserDetailsDocument)null, message.Response);
                default:
                    throw await _clientDiagnostics.CreateRequestFailedExceptionAsync(message.Response).ConfigureAwait(false);
            }
        }

        /// <param name="filter"> The ArrayOfString to use. </param>
        /// <param name="sort"> The ArrayOfString to use. </param>
        /// <param name="page"> The Pagination to use. </param>
        /// <param name="include"> The ArrayOfString to use. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        public Response<AdviserDetailsDocument> Get(IEnumerable<string> filter = null, IEnumerable<string> sort = null, Pagination page = null, IEnumerable<string> include = null, CancellationToken cancellationToken = default)
        {
            using var message = CreateGetRequest(filter, sort, page, include);
            _pipeline.Send(message, cancellationToken);
            switch (message.Response.Status)
            {
                case 200:
                    {
                        AdviserDetailsDocument value = default;
                        using var document = JsonDocument.Parse(message.Response.ContentStream);
                        value = AdviserDetailsDocument.DeserializeAdviserDetailsDocument(document.RootElement);
                        return Response.FromValue(value, message.Response);
                    }
                case 401:
                    return Response.FromValue((AdviserDetailsDocument)null, message.Response);
                default:
                    throw _clientDiagnostics.CreateRequestFailedException(message.Response);
            }
        }
    }
}