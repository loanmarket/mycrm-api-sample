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
    /// <summary> The DealNote service client. </summary>
    public partial class DealNoteClient
    {
        private readonly HttpPipeline _pipeline;
        private readonly Uri _endpoint;

        /// <summary> The ClientDiagnostics is used to provide tracing support for the client library. </summary>
        internal ClientDiagnostics ClientDiagnostics { get; }

        /// <summary> The HTTP pipeline for sending and receiving REST requests and responses. </summary>
        public virtual HttpPipeline Pipeline => _pipeline;

        /// <summary> Initializes a new instance of DealNoteClient for mocking. </summary>
        protected DealNoteClient()
        {
        }

        /// <summary> Initializes a new instance of DealNoteClient. </summary>
        /// <param name="endpoint"> server parameter. </param>
        /// <param name="options"> The options for configuring the client. </param>
        public DealNoteClient(Uri endpoint = null, MyCRMAPIClientOptions options = null)
        {
            endpoint ??= new Uri("");
            options ??= new MyCRMAPIClientOptions();

            ClientDiagnostics = new ClientDiagnostics(options);
            _pipeline = HttpPipelineBuilder.Build(options, Array.Empty<HttpPipelinePolicy>(), Array.Empty<HttpPipelinePolicy>(), new ResponseClassifier());
            _endpoint = endpoint;
        }

        /// <summary> Creates a new deal note with attributes, relationships or both. </summary>
        /// <param name="content"> The content to send as the body of the request. </param>
        /// <param name="context"> The request context, which can override default behaviors on the request on a per-call basis. </param>
        /// <remarks>
        /// Schema for <c>Request Body</c>:
        /// <code>{
        ///   meta: Dictionary&lt;string, object&gt;,
        ///   jsonApi: Dictionary&lt;string, object&gt;,
        ///   links: {
        ///     self: string,
        ///     related: string,
        ///     describedBy: string,
        ///     first: string,
        ///     last: string,
        ///     prev: string,
        ///     next: string
        ///   },
        ///   data: {
        ///     type: string (required),
        ///     id: string,
        ///     type: &quot;deal-notes&quot; (required),
        ///     id: string (required),
        ///     attributes: {
        ///       title: string,
        ///       detail: string
        ///     },
        ///     relationships: {
        ///       deal: {
        ///         links: {
        ///           self: string,
        ///           related: string
        ///         },
        ///         meta: Dictionary&lt;string, object&gt;,
        ///         data: {
        ///           type: string (required),
        ///           id: string
        ///         } (required)
        ///       }
        ///     },
        ///     links: {
        ///       self: string
        ///     },
        ///     meta: Dictionary&lt;string, object&gt;
        ///   } (required),
        ///   included: [
        ///     {
        ///       type: string (required),
        ///       id: string
        ///     }
        ///   ]
        /// }
        /// </code>
        /// Schema for <c>Response Body</c>:
        /// <code>{
        ///   meta: Dictionary&lt;string, object&gt;,
        ///   jsonApi: Dictionary&lt;string, object&gt;,
        ///   links: {
        ///     self: string,
        ///     related: string,
        ///     describedBy: string,
        ///     first: string,
        ///     last: string,
        ///     prev: string,
        ///     next: string
        ///   },
        ///   data: {
        ///     type: string,
        ///     id: string,
        ///     type: &quot;deal-notes&quot;,
        ///     id: string,
        ///     attributes: {
        ///       title: string,
        ///       detail: string
        ///     },
        ///     relationships: {
        ///       deal: {
        ///         links: {
        ///           self: string,
        ///           related: string
        ///         },
        ///         meta: Dictionary&lt;string, object&gt;,
        ///         data: {
        ///           type: string,
        ///           id: string
        ///         }
        ///       }
        ///     },
        ///     links: {
        ///       self: string
        ///     },
        ///     meta: Dictionary&lt;string, object&gt;
        ///   },
        ///   included: [
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
        public virtual async Task<Response> PostAsync(RequestContent content, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealNoteClient.Post");
            scope.Start();
            try
            {
                using HttpMessage message = CreatePostRequest(content, context);
                return await _pipeline.ProcessMessageAsync(message, context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Creates a new deal note with attributes, relationships or both. </summary>
        /// <param name="content"> The content to send as the body of the request. </param>
        /// <param name="context"> The request context, which can override default behaviors on the request on a per-call basis. </param>
        /// <remarks>
        /// Schema for <c>Request Body</c>:
        /// <code>{
        ///   meta: Dictionary&lt;string, object&gt;,
        ///   jsonApi: Dictionary&lt;string, object&gt;,
        ///   links: {
        ///     self: string,
        ///     related: string,
        ///     describedBy: string,
        ///     first: string,
        ///     last: string,
        ///     prev: string,
        ///     next: string
        ///   },
        ///   data: {
        ///     type: string (required),
        ///     id: string,
        ///     type: &quot;deal-notes&quot; (required),
        ///     id: string (required),
        ///     attributes: {
        ///       title: string,
        ///       detail: string
        ///     },
        ///     relationships: {
        ///       deal: {
        ///         links: {
        ///           self: string,
        ///           related: string
        ///         },
        ///         meta: Dictionary&lt;string, object&gt;,
        ///         data: {
        ///           type: string (required),
        ///           id: string
        ///         } (required)
        ///       }
        ///     },
        ///     links: {
        ///       self: string
        ///     },
        ///     meta: Dictionary&lt;string, object&gt;
        ///   } (required),
        ///   included: [
        ///     {
        ///       type: string (required),
        ///       id: string
        ///     }
        ///   ]
        /// }
        /// </code>
        /// Schema for <c>Response Body</c>:
        /// <code>{
        ///   meta: Dictionary&lt;string, object&gt;,
        ///   jsonApi: Dictionary&lt;string, object&gt;,
        ///   links: {
        ///     self: string,
        ///     related: string,
        ///     describedBy: string,
        ///     first: string,
        ///     last: string,
        ///     prev: string,
        ///     next: string
        ///   },
        ///   data: {
        ///     type: string,
        ///     id: string,
        ///     type: &quot;deal-notes&quot;,
        ///     id: string,
        ///     attributes: {
        ///       title: string,
        ///       detail: string
        ///     },
        ///     relationships: {
        ///       deal: {
        ///         links: {
        ///           self: string,
        ///           related: string
        ///         },
        ///         meta: Dictionary&lt;string, object&gt;,
        ///         data: {
        ///           type: string,
        ///           id: string
        ///         }
        ///       }
        ///     },
        ///     links: {
        ///       self: string
        ///     },
        ///     meta: Dictionary&lt;string, object&gt;
        ///   },
        ///   included: [
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
        public virtual Response Post(RequestContent content, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealNoteClient.Post");
            scope.Start();
            try
            {
                using HttpMessage message = CreatePostRequest(content, context);
                return _pipeline.ProcessMessage(message, context);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal note. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="context"> The request context, which can override default behaviors on the request on a per-call basis. </param>
        /// <remarks>
        /// Schema for <c>Response Body</c>:
        /// <code>{
        ///   meta: Dictionary&lt;string, object&gt;,
        ///   jsonApi: Dictionary&lt;string, object&gt;,
        ///   links: {
        ///     self: string,
        ///     related: string,
        ///     describedBy: string,
        ///     first: string,
        ///     last: string,
        ///     prev: string,
        ///     next: string
        ///   },
        ///   data: {
        ///     type: string,
        ///     id: string,
        ///     type: &quot;deal-notes&quot;,
        ///     id: string,
        ///     attributes: {
        ///       title: string,
        ///       detail: string
        ///     },
        ///     relationships: {
        ///       deal: {
        ///         links: {
        ///           self: string,
        ///           related: string
        ///         },
        ///         meta: Dictionary&lt;string, object&gt;,
        ///         data: {
        ///           type: string,
        ///           id: string
        ///         }
        ///       }
        ///     },
        ///     links: {
        ///       self: string
        ///     },
        ///     meta: Dictionary&lt;string, object&gt;
        ///   },
        ///   included: [
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
        public virtual async Task<Response> GetDealNoteAsync(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealNoteClient.GetDealNote");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetDealNoteRequest(id, context);
                return await _pipeline.ProcessMessageAsync(message, context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal note. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="context"> The request context, which can override default behaviors on the request on a per-call basis. </param>
        /// <remarks>
        /// Schema for <c>Response Body</c>:
        /// <code>{
        ///   meta: Dictionary&lt;string, object&gt;,
        ///   jsonApi: Dictionary&lt;string, object&gt;,
        ///   links: {
        ///     self: string,
        ///     related: string,
        ///     describedBy: string,
        ///     first: string,
        ///     last: string,
        ///     prev: string,
        ///     next: string
        ///   },
        ///   data: {
        ///     type: string,
        ///     id: string,
        ///     type: &quot;deal-notes&quot;,
        ///     id: string,
        ///     attributes: {
        ///       title: string,
        ///       detail: string
        ///     },
        ///     relationships: {
        ///       deal: {
        ///         links: {
        ///           self: string,
        ///           related: string
        ///         },
        ///         meta: Dictionary&lt;string, object&gt;,
        ///         data: {
        ///           type: string,
        ///           id: string
        ///         }
        ///       }
        ///     },
        ///     links: {
        ///       self: string
        ///     },
        ///     meta: Dictionary&lt;string, object&gt;
        ///   },
        ///   included: [
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
        public virtual Response GetDealNote(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealNoteClient.GetDealNote");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetDealNoteRequest(id, context);
                return _pipeline.ProcessMessage(message, context);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal note. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="content"> The content to send as the body of the request. </param>
        /// <param name="context"> The request context, which can override default behaviors on the request on a per-call basis. </param>
        /// <remarks>
        /// Schema for <c>Request Body</c>:
        /// <code>{
        ///   meta: Dictionary&lt;string, object&gt;,
        ///   jsonApi: Dictionary&lt;string, object&gt;,
        ///   links: {
        ///     self: string,
        ///     related: string,
        ///     describedBy: string,
        ///     first: string,
        ///     last: string,
        ///     prev: string,
        ///     next: string
        ///   },
        ///   data: {
        ///     type: string (required),
        ///     id: string,
        ///     type: &quot;deal-notes&quot; (required),
        ///     id: string (required),
        ///     attributes: {
        ///       title: string,
        ///       detail: string
        ///     },
        ///     relationships: {
        ///       deal: {
        ///         links: {
        ///           self: string,
        ///           related: string
        ///         },
        ///         meta: Dictionary&lt;string, object&gt;,
        ///         data: {
        ///           type: string (required),
        ///           id: string
        ///         } (required)
        ///       }
        ///     },
        ///     links: {
        ///       self: string
        ///     },
        ///     meta: Dictionary&lt;string, object&gt;
        ///   } (required),
        ///   included: [
        ///     {
        ///       type: string (required),
        ///       id: string
        ///     }
        ///   ]
        /// }
        /// </code>
        /// Schema for <c>Response Body</c>:
        /// <code>{
        ///   meta: Dictionary&lt;string, object&gt;,
        ///   jsonApi: Dictionary&lt;string, object&gt;,
        ///   links: {
        ///     self: string,
        ///     related: string,
        ///     describedBy: string,
        ///     first: string,
        ///     last: string,
        ///     prev: string,
        ///     next: string
        ///   },
        ///   data: {
        ///     type: string,
        ///     id: string,
        ///     type: &quot;deal-notes&quot;,
        ///     id: string,
        ///     attributes: {
        ///       title: string,
        ///       detail: string
        ///     },
        ///     relationships: {
        ///       deal: {
        ///         links: {
        ///           self: string,
        ///           related: string
        ///         },
        ///         meta: Dictionary&lt;string, object&gt;,
        ///         data: {
        ///           type: string,
        ///           id: string
        ///         }
        ///       }
        ///     },
        ///     links: {
        ///       self: string
        ///     },
        ///     meta: Dictionary&lt;string, object&gt;
        ///   },
        ///   included: [
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
        public virtual async Task<Response> PatchAsync(int id, RequestContent content, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealNoteClient.Patch");
            scope.Start();
            try
            {
                using HttpMessage message = CreatePatchRequest(id, content, context);
                return await _pipeline.ProcessMessageAsync(message, context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal note. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="content"> The content to send as the body of the request. </param>
        /// <param name="context"> The request context, which can override default behaviors on the request on a per-call basis. </param>
        /// <remarks>
        /// Schema for <c>Request Body</c>:
        /// <code>{
        ///   meta: Dictionary&lt;string, object&gt;,
        ///   jsonApi: Dictionary&lt;string, object&gt;,
        ///   links: {
        ///     self: string,
        ///     related: string,
        ///     describedBy: string,
        ///     first: string,
        ///     last: string,
        ///     prev: string,
        ///     next: string
        ///   },
        ///   data: {
        ///     type: string (required),
        ///     id: string,
        ///     type: &quot;deal-notes&quot; (required),
        ///     id: string (required),
        ///     attributes: {
        ///       title: string,
        ///       detail: string
        ///     },
        ///     relationships: {
        ///       deal: {
        ///         links: {
        ///           self: string,
        ///           related: string
        ///         },
        ///         meta: Dictionary&lt;string, object&gt;,
        ///         data: {
        ///           type: string (required),
        ///           id: string
        ///         } (required)
        ///       }
        ///     },
        ///     links: {
        ///       self: string
        ///     },
        ///     meta: Dictionary&lt;string, object&gt;
        ///   } (required),
        ///   included: [
        ///     {
        ///       type: string (required),
        ///       id: string
        ///     }
        ///   ]
        /// }
        /// </code>
        /// Schema for <c>Response Body</c>:
        /// <code>{
        ///   meta: Dictionary&lt;string, object&gt;,
        ///   jsonApi: Dictionary&lt;string, object&gt;,
        ///   links: {
        ///     self: string,
        ///     related: string,
        ///     describedBy: string,
        ///     first: string,
        ///     last: string,
        ///     prev: string,
        ///     next: string
        ///   },
        ///   data: {
        ///     type: string,
        ///     id: string,
        ///     type: &quot;deal-notes&quot;,
        ///     id: string,
        ///     attributes: {
        ///       title: string,
        ///       detail: string
        ///     },
        ///     relationships: {
        ///       deal: {
        ///         links: {
        ///           self: string,
        ///           related: string
        ///         },
        ///         meta: Dictionary&lt;string, object&gt;,
        ///         data: {
        ///           type: string,
        ///           id: string
        ///         }
        ///       }
        ///     },
        ///     links: {
        ///       self: string
        ///     },
        ///     meta: Dictionary&lt;string, object&gt;
        ///   },
        ///   included: [
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
        public virtual Response Patch(int id, RequestContent content, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealNoteClient.Patch");
            scope.Start();
            try
            {
                using HttpMessage message = CreatePatchRequest(id, content, context);
                return _pipeline.ProcessMessage(message, context);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal note. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="context"> The request context, which can override default behaviors on the request on a per-call basis. </param>
        /// <remarks>
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
        public virtual async Task<Response> DeleteAsync(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealNoteClient.Delete");
            scope.Start();
            try
            {
                using HttpMessage message = CreateDeleteRequest(id, context);
                return await _pipeline.ProcessMessageAsync(message, context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the deal note. </summary>
        /// <param name="id"> The Integer to use. </param>
        /// <param name="context"> The request context, which can override default behaviors on the request on a per-call basis. </param>
        /// <remarks>
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
        public virtual Response Delete(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealNoteClient.Delete");
            scope.Start();
            try
            {
                using HttpMessage message = CreateDeleteRequest(id, context);
                return _pipeline.ProcessMessage(message, context);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        internal HttpMessage CreatePostRequest(RequestContent content, RequestContext context)
        {
            var message = _pipeline.CreateMessage(context, ResponseClassifier201401);
            var request = message.Request;
            request.Method = RequestMethod.Post;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/jsonapi/deal-notes", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            request.Headers.Add("Content-Type", "application/vnd.api+json");
            request.Content = content;
            return message;
        }

        internal HttpMessage CreateGetDealNoteRequest(int id, RequestContext context)
        {
            var message = _pipeline.CreateMessage(context, ResponseClassifier200401);
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/jsonapi/deal-notes/", false);
            uri.AppendPath(id, true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        internal HttpMessage CreatePatchRequest(int id, RequestContent content, RequestContext context)
        {
            var message = _pipeline.CreateMessage(context, ResponseClassifier200204401);
            var request = message.Request;
            request.Method = RequestMethod.Patch;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/jsonapi/deal-notes/", false);
            uri.AppendPath(id, true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            request.Headers.Add("Content-Type", "application/vnd.api+json");
            request.Content = content;
            return message;
        }

        internal HttpMessage CreateDeleteRequest(int id, RequestContext context)
        {
            var message = _pipeline.CreateMessage(context, ResponseClassifier204401);
            var request = message.Request;
            request.Method = RequestMethod.Delete;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/jsonapi/deal-notes/", false);
            uri.AppendPath(id, true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        private static ResponseClassifier _responseClassifier201401;
        private static ResponseClassifier ResponseClassifier201401 => _responseClassifier201401 ??= new StatusCodeClassifier(stackalloc ushort[] { 201, 401 });
        private static ResponseClassifier _responseClassifier200401;
        private static ResponseClassifier ResponseClassifier200401 => _responseClassifier200401 ??= new StatusCodeClassifier(stackalloc ushort[] { 200, 401 });
        private static ResponseClassifier _responseClassifier200204401;
        private static ResponseClassifier ResponseClassifier200204401 => _responseClassifier200204401 ??= new StatusCodeClassifier(stackalloc ushort[] { 200, 204, 401 });
        private static ResponseClassifier _responseClassifier204401;
        private static ResponseClassifier ResponseClassifier204401 => _responseClassifier204401 ??= new StatusCodeClassifier(stackalloc ushort[] { 204, 401 });
    }
}
