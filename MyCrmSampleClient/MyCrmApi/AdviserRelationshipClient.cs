// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Core.Pipeline;

namespace MyCrmSampleClient.MyCrmApi
{
    /// <summary> The AdviserRelationship service client. </summary>
    public partial class AdviserRelationshipClient
    {
        private readonly HttpPipeline _pipeline;
        private readonly Uri _endpoint;

        /// <summary> The ClientDiagnostics is used to provide tracing support for the client library. </summary>
        internal ClientDiagnostics ClientDiagnostics { get; }

        /// <summary> The HTTP pipeline for sending and receiving REST requests and responses. </summary>
        public virtual HttpPipeline Pipeline => _pipeline;

        /// <summary> Initializes a new instance of AdviserRelationshipClient for mocking. </summary>
        protected AdviserRelationshipClient()
        {
        }

        /// <summary> Initializes a new instance of AdviserRelationshipClient. </summary>
        /// <param name="endpoint"> server parameter. </param>
        /// <param name="options"> The options for configuring the client. </param>
        public AdviserRelationshipClient(Uri endpoint = null, MyCRMAPIClientOptions options = null)
        {
            endpoint ??= new Uri("");
            options ??= new MyCRMAPIClientOptions();

            ClientDiagnostics = new ClientDiagnostics(options);
            _pipeline = HttpPipelineBuilder.Build(options, Array.Empty<HttpPipelinePolicy>(), Array.Empty<HttpPipelinePolicy>(), new ResponseClassifier());
            _endpoint = endpoint;
        }

        /// <summary> Where `id` is the identifier of the adviser. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="context"> The request context, which can override default behaviors on the request on a per-call basis. </param>
        /// <remarks>
        /// Schema for <c>Response Body</c>:
        /// <code>{
        ///   links: {
        ///     self: string,
        ///     related: string
        ///   },
        ///   meta: Dictionary&lt;string, object&gt;,
        ///   data: [
        ///     {
        ///       type: string,
        ///       id: string
        ///     }
        ///   ]
        /// }
        /// </code>
        /// Schema for <c>Response Error</c>:
        /// <code>{
        ///   links: {
        ///     self: string,
        ///     related: string,
        ///     describedby: string,
        ///     first: string,
        ///     last: string,
        ///     prev: string,
        ///     next: string
        ///   },
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string,
        ///         type: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string,
        ///         header: string
        ///       },
        ///       meta: Dictionary&lt;string, object&gt;
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual async Task<Response> GetContactGroupsAsync(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("AdviserRelationshipClient.GetContactGroups");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetContactGroupsRequest(id, context);
                return await _pipeline.ProcessMessageAsync(message, context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the adviser. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="context"> The request context, which can override default behaviors on the request on a per-call basis. </param>
        /// <remarks>
        /// Schema for <c>Response Body</c>:
        /// <code>{
        ///   links: {
        ///     self: string,
        ///     related: string
        ///   },
        ///   meta: Dictionary&lt;string, object&gt;,
        ///   data: [
        ///     {
        ///       type: string,
        ///       id: string
        ///     }
        ///   ]
        /// }
        /// </code>
        /// Schema for <c>Response Error</c>:
        /// <code>{
        ///   links: {
        ///     self: string,
        ///     related: string,
        ///     describedby: string,
        ///     first: string,
        ///     last: string,
        ///     prev: string,
        ///     next: string
        ///   },
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string,
        ///         type: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string,
        ///         header: string
        ///       },
        ///       meta: Dictionary&lt;string, object&gt;
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual Response GetContactGroups(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("AdviserRelationshipClient.GetContactGroups");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetContactGroupsRequest(id, context);
                return _pipeline.ProcessMessage(message, context);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the adviser. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="context"> The request context, which can override default behaviors on the request on a per-call basis. </param>
        /// <remarks>
        /// Schema for <c>Response Body</c>:
        /// <code>{
        ///   links: {
        ///     self: string,
        ///     related: string
        ///   },
        ///   meta: Dictionary&lt;string, object&gt;,
        ///   data: {
        ///     type: string,
        ///     id: string
        ///   }
        /// }
        /// </code>
        /// Schema for <c>Response Error</c>:
        /// <code>{
        ///   links: {
        ///     self: string,
        ///     related: string,
        ///     describedby: string,
        ///     first: string,
        ///     last: string,
        ///     prev: string,
        ///     next: string
        ///   },
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string,
        ///         type: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string,
        ///         header: string
        ///       },
        ///       meta: Dictionary&lt;string, object&gt;
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual async Task<Response> GetAdviserDetailsAsync(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("AdviserRelationshipClient.GetAdviserDetails");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetAdviserDetailsRequest(id, context);
                return await _pipeline.ProcessMessageAsync(message, context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the adviser. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="context"> The request context, which can override default behaviors on the request on a per-call basis. </param>
        /// <remarks>
        /// Schema for <c>Response Body</c>:
        /// <code>{
        ///   links: {
        ///     self: string,
        ///     related: string
        ///   },
        ///   meta: Dictionary&lt;string, object&gt;,
        ///   data: {
        ///     type: string,
        ///     id: string
        ///   }
        /// }
        /// </code>
        /// Schema for <c>Response Error</c>:
        /// <code>{
        ///   links: {
        ///     self: string,
        ///     related: string,
        ///     describedby: string,
        ///     first: string,
        ///     last: string,
        ///     prev: string,
        ///     next: string
        ///   },
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string,
        ///         type: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string,
        ///         header: string
        ///       },
        ///       meta: Dictionary&lt;string, object&gt;
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual Response GetAdviserDetails(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("AdviserRelationshipClient.GetAdviserDetails");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetAdviserDetailsRequest(id, context);
                return _pipeline.ProcessMessage(message, context);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the adviser. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="context"> The request context, which can override default behaviors on the request on a per-call basis. </param>
        /// <remarks>
        /// Schema for <c>Response Body</c>:
        /// <code>{
        ///   links: {
        ///     self: string,
        ///     related: string
        ///   },
        ///   meta: Dictionary&lt;string, object&gt;,
        ///   data: {
        ///     type: string,
        ///     id: string
        ///   }
        /// }
        /// </code>
        /// Schema for <c>Response Error</c>:
        /// <code>{
        ///   links: {
        ///     self: string,
        ///     related: string,
        ///     describedby: string,
        ///     first: string,
        ///     last: string,
        ///     prev: string,
        ///     next: string
        ///   },
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string,
        ///         type: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string,
        ///         header: string
        ///       },
        ///       meta: Dictionary&lt;string, object&gt;
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual async Task<Response> GetOrganisationsAsync(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("AdviserRelationshipClient.GetOrganisations");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetOrganisationsRequest(id, context);
                return await _pipeline.ProcessMessageAsync(message, context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the adviser. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="context"> The request context, which can override default behaviors on the request on a per-call basis. </param>
        /// <remarks>
        /// Schema for <c>Response Body</c>:
        /// <code>{
        ///   links: {
        ///     self: string,
        ///     related: string
        ///   },
        ///   meta: Dictionary&lt;string, object&gt;,
        ///   data: {
        ///     type: string,
        ///     id: string
        ///   }
        /// }
        /// </code>
        /// Schema for <c>Response Error</c>:
        /// <code>{
        ///   links: {
        ///     self: string,
        ///     related: string,
        ///     describedby: string,
        ///     first: string,
        ///     last: string,
        ///     prev: string,
        ///     next: string
        ///   },
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string,
        ///         type: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string,
        ///         header: string
        ///       },
        ///       meta: Dictionary&lt;string, object&gt;
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual Response GetOrganisations(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("AdviserRelationshipClient.GetOrganisations");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetOrganisationsRequest(id, context);
                return _pipeline.ProcessMessage(message, context);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the adviser. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="context"> The request context, which can override default behaviors on the request on a per-call basis. </param>
        /// <remarks>
        /// Schema for <c>Response Body</c>:
        /// <code>{
        ///   links: {
        ///     self: string,
        ///     related: string
        ///   },
        ///   meta: Dictionary&lt;string, object&gt;,
        ///   data: [
        ///     {
        ///       type: string,
        ///       id: string
        ///     }
        ///   ]
        /// }
        /// </code>
        /// Schema for <c>Response Error</c>:
        /// <code>{
        ///   links: {
        ///     self: string,
        ///     related: string,
        ///     describedby: string,
        ///     first: string,
        ///     last: string,
        ///     prev: string,
        ///     next: string
        ///   },
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string,
        ///         type: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string,
        ///         header: string
        ///       },
        ///       meta: Dictionary&lt;string, object&gt;
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual async Task<Response> GetAgreementHoldersAsync(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("AdviserRelationshipClient.GetAgreementHolders");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetAgreementHoldersRequest(id, context);
                return await _pipeline.ProcessMessageAsync(message, context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the adviser. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="context"> The request context, which can override default behaviors on the request on a per-call basis. </param>
        /// <remarks>
        /// Schema for <c>Response Body</c>:
        /// <code>{
        ///   links: {
        ///     self: string,
        ///     related: string
        ///   },
        ///   meta: Dictionary&lt;string, object&gt;,
        ///   data: [
        ///     {
        ///       type: string,
        ///       id: string
        ///     }
        ///   ]
        /// }
        /// </code>
        /// Schema for <c>Response Error</c>:
        /// <code>{
        ///   links: {
        ///     self: string,
        ///     related: string,
        ///     describedby: string,
        ///     first: string,
        ///     last: string,
        ///     prev: string,
        ///     next: string
        ///   },
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string,
        ///         type: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string,
        ///         header: string
        ///       },
        ///       meta: Dictionary&lt;string, object&gt;
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual Response GetAgreementHolders(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("AdviserRelationshipClient.GetAgreementHolders");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetAgreementHoldersRequest(id, context);
                return _pipeline.ProcessMessage(message, context);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the adviser. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="context"> The request context, which can override default behaviors on the request on a per-call basis. </param>
        /// <remarks>
        /// Schema for <c>Response Body</c>:
        /// <code>{
        ///   links: {
        ///     self: string,
        ///     related: string
        ///   },
        ///   meta: Dictionary&lt;string, object&gt;,
        ///   data: [
        ///     {
        ///       type: string,
        ///       id: string
        ///     }
        ///   ]
        /// }
        /// </code>
        /// Schema for <c>Response Error</c>:
        /// <code>{
        ///   links: {
        ///     self: string,
        ///     related: string,
        ///     describedby: string,
        ///     first: string,
        ///     last: string,
        ///     prev: string,
        ///     next: string
        ///   },
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string,
        ///         type: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string,
        ///         header: string
        ///       },
        ///       meta: Dictionary&lt;string, object&gt;
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual async Task<Response> GetAdviserAddressesAsync(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("AdviserRelationshipClient.GetAdviserAddresses");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetAdviserAddressesRequest(id, context);
                return await _pipeline.ProcessMessageAsync(message, context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the adviser. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="context"> The request context, which can override default behaviors on the request on a per-call basis. </param>
        /// <remarks>
        /// Schema for <c>Response Body</c>:
        /// <code>{
        ///   links: {
        ///     self: string,
        ///     related: string
        ///   },
        ///   meta: Dictionary&lt;string, object&gt;,
        ///   data: [
        ///     {
        ///       type: string,
        ///       id: string
        ///     }
        ///   ]
        /// }
        /// </code>
        /// Schema for <c>Response Error</c>:
        /// <code>{
        ///   links: {
        ///     self: string,
        ///     related: string,
        ///     describedby: string,
        ///     first: string,
        ///     last: string,
        ///     prev: string,
        ///     next: string
        ///   },
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string,
        ///         type: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string,
        ///         header: string
        ///       },
        ///       meta: Dictionary&lt;string, object&gt;
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual Response GetAdviserAddresses(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("AdviserRelationshipClient.GetAdviserAddresses");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetAdviserAddressesRequest(id, context);
                return _pipeline.ProcessMessage(message, context);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        internal HttpMessage CreateGetContactGroupsRequest(int id, RequestContext context)
        {
            var message = _pipeline.CreateMessage(context, ResponseClassifier200401);
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/jsonapi/advisers/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/relationships/contactGroups", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        internal HttpMessage CreateGetAdviserDetailsRequest(int id, RequestContext context)
        {
            var message = _pipeline.CreateMessage(context, ResponseClassifier200401);
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/jsonapi/advisers/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/relationships/adviserDetails", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        internal HttpMessage CreateGetOrganisationsRequest(int id, RequestContext context)
        {
            var message = _pipeline.CreateMessage(context, ResponseClassifier200401);
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/jsonapi/advisers/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/relationships/organisation", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        internal HttpMessage CreateGetAgreementHoldersRequest(int id, RequestContext context)
        {
            var message = _pipeline.CreateMessage(context, ResponseClassifier200401);
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/jsonapi/advisers/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/relationships/agreementHolders", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        internal HttpMessage CreateGetAdviserAddressesRequest(int id, RequestContext context)
        {
            var message = _pipeline.CreateMessage(context, ResponseClassifier200401);
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/jsonapi/advisers/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/relationships/addresses", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        private static ResponseClassifier _responseClassifier200401;
        private static ResponseClassifier ResponseClassifier200401 => _responseClassifier200401 ??= new StatusCodeClassifier(stackalloc ushort[] { 200, 401 });
    }
}
