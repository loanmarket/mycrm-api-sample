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
    /// <summary> The DealRelationship service client. </summary>
    public partial class DealRelationshipClient
    {
        private readonly HttpPipeline _pipeline;
        private readonly Uri _endpoint;

        /// <summary> The ClientDiagnostics is used to provide tracing support for the client library. </summary>
        internal ClientDiagnostics ClientDiagnostics { get; }

        /// <summary> The HTTP pipeline for sending and receiving REST requests and responses. </summary>
        public virtual HttpPipeline Pipeline => _pipeline;

        /// <summary> Initializes a new instance of DealRelationshipClient for mocking. </summary>
        protected DealRelationshipClient()
        {
        }

        /// <summary> Initializes a new instance of DealRelationshipClient. </summary>
        /// <param name="endpoint"> server parameter. </param>
        /// <param name="options"> The options for configuring the client. </param>
        public DealRelationshipClient(Uri endpoint = null, MyCRMAPIClientOptions options = null)
        {
            endpoint ??= new Uri("");
            options ??= new MyCRMAPIClientOptions();

            ClientDiagnostics = new ClientDiagnostics(options);
            _pipeline = HttpPipelineBuilder.Build(options, Array.Empty<HttpPipelinePolicy>(), Array.Empty<HttpPipelinePolicy>(), new ResponseClassifier());
            _endpoint = endpoint;
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
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
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string
        ///       },
        ///       meta: {
        ///         data: Dictionary&lt;string, object&gt;
        ///       }
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual async Task<Response> GetDealScenariosAsync(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealRelationshipClient.GetDealScenarios");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetDealScenariosRequest(id, context);
                return await _pipeline.ProcessMessageAsync(message, context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
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
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string
        ///       },
        ///       meta: {
        ///         data: Dictionary&lt;string, object&gt;
        ///       }
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual Response GetDealScenarios(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealRelationshipClient.GetDealScenarios");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetDealScenariosRequest(id, context);
                return _pipeline.ProcessMessage(message, context);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
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
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string
        ///       },
        ///       meta: {
        ///         data: Dictionary&lt;string, object&gt;
        ///       }
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual async Task<Response> GetDealImportantDatesAsync(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealRelationshipClient.GetDealImportantDates");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetDealImportantDatesRequest(id, context);
                return await _pipeline.ProcessMessageAsync(message, context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
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
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string
        ///       },
        ///       meta: {
        ///         data: Dictionary&lt;string, object&gt;
        ///       }
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual Response GetDealImportantDates(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealRelationshipClient.GetDealImportantDates");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetDealImportantDatesRequest(id, context);
                return _pipeline.ProcessMessage(message, context);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
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
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string
        ///       },
        ///       meta: {
        ///         data: Dictionary&lt;string, object&gt;
        ///       }
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual async Task<Response> GetDealParticipantsAsync(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealRelationshipClient.GetDealParticipants");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetDealParticipantsRequest(id, context);
                return await _pipeline.ProcessMessageAsync(message, context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
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
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string
        ///       },
        ///       meta: {
        ///         data: Dictionary&lt;string, object&gt;
        ///       }
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual Response GetDealParticipants(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealRelationshipClient.GetDealParticipants");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetDealParticipantsRequest(id, context);
                return _pipeline.ProcessMessage(message, context);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
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
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string
        ///       },
        ///       meta: {
        ///         data: Dictionary&lt;string, object&gt;
        ///       }
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual async Task<Response> GetDealExternalReferencesAsync(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealRelationshipClient.GetDealExternalReferences");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetDealExternalReferencesRequest(id, context);
                return await _pipeline.ProcessMessageAsync(message, context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
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
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string
        ///       },
        ///       meta: {
        ///         data: Dictionary&lt;string, object&gt;
        ///       }
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual Response GetDealExternalReferences(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealRelationshipClient.GetDealExternalReferences");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetDealExternalReferencesRequest(id, context);
                return _pipeline.ProcessMessage(message, context);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
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
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string
        ///       },
        ///       meta: {
        ///         data: Dictionary&lt;string, object&gt;
        ///       }
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual async Task<Response> GetContactsAsync(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealRelationshipClient.GetContacts");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetContactsRequest(id, context);
                return await _pipeline.ProcessMessageAsync(message, context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
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
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string
        ///       },
        ///       meta: {
        ///         data: Dictionary&lt;string, object&gt;
        ///       }
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual Response GetContacts(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealRelationshipClient.GetContacts");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetContactsRequest(id, context);
                return _pipeline.ProcessMessage(message, context);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
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
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string
        ///       },
        ///       meta: {
        ///         data: Dictionary&lt;string, object&gt;
        ///       }
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual async Task<Response> GetDealStructuresAsync(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealRelationshipClient.GetDealStructures");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetDealStructuresRequest(id, context);
                return await _pipeline.ProcessMessageAsync(message, context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
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
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string
        ///       },
        ///       meta: {
        ///         data: Dictionary&lt;string, object&gt;
        ///       }
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual Response GetDealStructures(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealRelationshipClient.GetDealStructures");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetDealStructuresRequest(id, context);
                return _pipeline.ProcessMessage(message, context);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
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
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string
        ///       },
        ///       meta: {
        ///         data: Dictionary&lt;string, object&gt;
        ///       }
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual async Task<Response> GetAdvisersAsync(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealRelationshipClient.GetAdvisers");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetAdvisersRequest(id, context);
                return await _pipeline.ProcessMessageAsync(message, context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
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
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string
        ///       },
        ///       meta: {
        ///         data: Dictionary&lt;string, object&gt;
        ///       }
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual Response GetAdvisers(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealRelationshipClient.GetAdvisers");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetAdvisersRequest(id, context);
                return _pipeline.ProcessMessage(message, context);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
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
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string
        ///       },
        ///       meta: {
        ///         data: Dictionary&lt;string, object&gt;
        ///       }
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual async Task<Response> GetDealNotesAsync(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealRelationshipClient.GetDealNotes");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetDealNotesRequest(id, context);
                return await _pipeline.ProcessMessageAsync(message, context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal. </summary>
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
        ///   errors: [
        ///     {
        ///       id: string,
        ///       links: {
        ///         about: string
        ///       },
        ///       status: string,
        ///       code: string,
        ///       title: string,
        ///       detail: string,
        ///       source: {
        ///         pointer: string,
        ///         parameter: string
        ///       },
        ///       meta: {
        ///         data: Dictionary&lt;string, object&gt;
        ///       }
        ///     }
        ///   ]
        /// }
        /// </code>
        /// 
        /// </remarks>
        public virtual Response GetDealNotes(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealRelationshipClient.GetDealNotes");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetDealNotesRequest(id, context);
                return _pipeline.ProcessMessage(message, context);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        internal HttpMessage CreateGetDealScenariosRequest(int id, RequestContext context)
        {
            var message = _pipeline.CreateMessage(context, ResponseClassifier200401);
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/jsonapi/deals/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/relationships/dealScenario", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        internal HttpMessage CreateGetDealImportantDatesRequest(int id, RequestContext context)
        {
            var message = _pipeline.CreateMessage(context, ResponseClassifier200401);
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/jsonapi/deals/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/relationships/importantDates", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        internal HttpMessage CreateGetDealParticipantsRequest(int id, RequestContext context)
        {
            var message = _pipeline.CreateMessage(context, ResponseClassifier200401);
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/jsonapi/deals/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/relationships/participants", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        internal HttpMessage CreateGetDealExternalReferencesRequest(int id, RequestContext context)
        {
            var message = _pipeline.CreateMessage(context, ResponseClassifier200401);
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/jsonapi/deals/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/relationships/externalReferences", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        internal HttpMessage CreateGetContactsRequest(int id, RequestContext context)
        {
            var message = _pipeline.CreateMessage(context, ResponseClassifier200401);
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/jsonapi/deals/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/relationships/contacts", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        internal HttpMessage CreateGetDealStructuresRequest(int id, RequestContext context)
        {
            var message = _pipeline.CreateMessage(context, ResponseClassifier200401);
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/jsonapi/deals/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/relationships/dealStructures", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        internal HttpMessage CreateGetAdvisersRequest(int id, RequestContext context)
        {
            var message = _pipeline.CreateMessage(context, ResponseClassifier200401);
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/jsonapi/deals/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/relationships/adviser", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        internal HttpMessage CreateGetDealNotesRequest(int id, RequestContext context)
        {
            var message = _pipeline.CreateMessage(context, ResponseClassifier200401);
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/jsonapi/deals/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/relationships/dealNotes", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        private static ResponseClassifier _responseClassifier200401;
        private static ResponseClassifier ResponseClassifier200401 => _responseClassifier200401 ??= new StatusCodeClassifier(stackalloc ushort[] { 200, 401 });
    }
}
