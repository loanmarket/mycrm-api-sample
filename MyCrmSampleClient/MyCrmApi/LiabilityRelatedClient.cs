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
    /// <summary> The LiabilityRelated service client. </summary>
    public partial class LiabilityRelatedClient
    {
        private readonly HttpPipeline _pipeline;
        private readonly Uri _endpoint;

        /// <summary> The ClientDiagnostics is used to provide tracing support for the client library. </summary>
        internal ClientDiagnostics ClientDiagnostics { get; }

        /// <summary> The HTTP pipeline for sending and receiving REST requests and responses. </summary>
        public virtual HttpPipeline Pipeline => _pipeline;

        /// <summary> Initializes a new instance of LiabilityRelatedClient for mocking. </summary>
        protected LiabilityRelatedClient()
        {
        }

        /// <summary> Initializes a new instance of LiabilityRelatedClient. </summary>
        /// <param name="endpoint"> server parameter. </param>
        /// <param name="options"> The options for configuring the client. </param>
        public LiabilityRelatedClient(Uri endpoint = null, MyCRMAPIClientOptions options = null)
        {
            endpoint ??= new Uri("");
            options ??= new MyCRMAPIClientOptions();

            ClientDiagnostics = new ClientDiagnostics(options);
            _pipeline = HttpPipelineBuilder.Build(options, Array.Empty<HttpPipelinePolicy>(), Array.Empty<HttpPipelinePolicy>(), new ResponseClassifier());
            _endpoint = endpoint;
        }

        /// <summary> Where `id` is the identifier of the liabilities. </summary>
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
        ///   data: [
        ///     {
        ///       type: string,
        ///       id: string,
        ///       type: &quot;assets&quot;,
        ///       id: string,
        ///       attributes: {
        ///         updated: string (ISO 8601 Format),
        ///         created: string (ISO 8601 Format),
        ///         description: string,
        ///         value: number,
        ///         accountName: string,
        ///         accountNumber: string,
        ///         bsb: string,
        ///         vehicleMake: string,
        ///         vehicleYear: number,
        ///         propertyType: string,
        ///         isRentalIncome: boolean,
        ///         isExistingMortgages: boolean,
        ///         assetTypeId: number,
        ///         isAccountDetailsKnown: boolean,
        ///         institution: string,
        ///         assetSubType: string,
        ///         assetSubTypeId: number,
        ///         assetType: string,
        ///         assetCategory: string,
        ///         currencyCode: string,
        ///         valuationBasis: string,
        ///         propertyZoningType: string,
        ///         addressPropertyType: string,
        ///         assetCategoryId: number
        ///       },
        ///       relationships: {
        ///         ownership: {
        ///           links: {
        ///             self: string,
        ///             related: string
        ///           },
        ///           meta: Dictionary&lt;string, object&gt;,
        ///           data: [
        ///             {
        ///               type: string,
        ///               id: string
        ///             }
        ///           ]
        ///         },
        ///         addresses: RelationshipsMultipleDocument
        ///       },
        ///       links: {
        ///         self: string
        ///       },
        ///       meta: Dictionary&lt;string, object&gt;
        ///     }
        ///   ],
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
        public virtual async Task<Response> GetAssetsAsync(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("LiabilityRelatedClient.GetAssets");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetAssetsRequest(id, context);
                return await _pipeline.ProcessMessageAsync(message, context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the liabilities. </summary>
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
        ///   data: [
        ///     {
        ///       type: string,
        ///       id: string,
        ///       type: &quot;assets&quot;,
        ///       id: string,
        ///       attributes: {
        ///         updated: string (ISO 8601 Format),
        ///         created: string (ISO 8601 Format),
        ///         description: string,
        ///         value: number,
        ///         accountName: string,
        ///         accountNumber: string,
        ///         bsb: string,
        ///         vehicleMake: string,
        ///         vehicleYear: number,
        ///         propertyType: string,
        ///         isRentalIncome: boolean,
        ///         isExistingMortgages: boolean,
        ///         assetTypeId: number,
        ///         isAccountDetailsKnown: boolean,
        ///         institution: string,
        ///         assetSubType: string,
        ///         assetSubTypeId: number,
        ///         assetType: string,
        ///         assetCategory: string,
        ///         currencyCode: string,
        ///         valuationBasis: string,
        ///         propertyZoningType: string,
        ///         addressPropertyType: string,
        ///         assetCategoryId: number
        ///       },
        ///       relationships: {
        ///         ownership: {
        ///           links: {
        ///             self: string,
        ///             related: string
        ///           },
        ///           meta: Dictionary&lt;string, object&gt;,
        ///           data: [
        ///             {
        ///               type: string,
        ///               id: string
        ///             }
        ///           ]
        ///         },
        ///         addresses: RelationshipsMultipleDocument
        ///       },
        ///       links: {
        ///         self: string
        ///       },
        ///       meta: Dictionary&lt;string, object&gt;
        ///     }
        ///   ],
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
        public virtual Response GetAssets(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("LiabilityRelatedClient.GetAssets");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetAssetsRequest(id, context);
                return _pipeline.ProcessMessage(message, context);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the liabilities. </summary>
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
        ///   data: [
        ///     {
        ///       type: string,
        ///       id: string,
        ///       type: &quot;owners&quot;,
        ///       id: string,
        ///       attributes: {
        ///         updated: string (ISO 8601 Format),
        ///         created: string (ISO 8601 Format),
        ///         ownershipPercentage: number
        ///       },
        ///       relationships: {
        ///         contact: {
        ///           links: {
        ///             self: string,
        ///             related: string
        ///           },
        ///           meta: Dictionary&lt;string, object&gt;,
        ///           data: {
        ///             type: string,
        ///             id: string
        ///           }
        ///         },
        ///         business: RelationshipsSingleDocument,
        ///         asset: RelationshipsSingleDocument,
        ///         expense: RelationshipsSingleDocument,
        ///         liability: RelationshipsSingleDocument,
        ///         income: RelationshipsSingleDocument
        ///       },
        ///       links: {
        ///         self: string
        ///       },
        ///       meta: Dictionary&lt;string, object&gt;
        ///     }
        ///   ],
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
        public virtual async Task<Response> GetOwnersAsync(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("LiabilityRelatedClient.GetOwners");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetOwnersRequest(id, context);
                return await _pipeline.ProcessMessageAsync(message, context).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        /// <summary> Where `id` is the identifier of the liabilities. </summary>
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
        ///   data: [
        ///     {
        ///       type: string,
        ///       id: string,
        ///       type: &quot;owners&quot;,
        ///       id: string,
        ///       attributes: {
        ///         updated: string (ISO 8601 Format),
        ///         created: string (ISO 8601 Format),
        ///         ownershipPercentage: number
        ///       },
        ///       relationships: {
        ///         contact: {
        ///           links: {
        ///             self: string,
        ///             related: string
        ///           },
        ///           meta: Dictionary&lt;string, object&gt;,
        ///           data: {
        ///             type: string,
        ///             id: string
        ///           }
        ///         },
        ///         business: RelationshipsSingleDocument,
        ///         asset: RelationshipsSingleDocument,
        ///         expense: RelationshipsSingleDocument,
        ///         liability: RelationshipsSingleDocument,
        ///         income: RelationshipsSingleDocument
        ///       },
        ///       links: {
        ///         self: string
        ///       },
        ///       meta: Dictionary&lt;string, object&gt;
        ///     }
        ///   ],
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
        public virtual Response GetOwners(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("LiabilityRelatedClient.GetOwners");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetOwnersRequest(id, context);
                return _pipeline.ProcessMessage(message, context);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        internal HttpMessage CreateGetAssetsRequest(int id, RequestContext context)
        {
            var message = _pipeline.CreateMessage(context, ResponseClassifier200401);
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/jsonapi/liabilities/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/linkedAsset", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        internal HttpMessage CreateGetOwnersRequest(int id, RequestContext context)
        {
            var message = _pipeline.CreateMessage(context, ResponseClassifier200401);
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/jsonapi/liabilities/", false);
            uri.AppendPath(id, true);
            uri.AppendPath("/ownership", false);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        private static ResponseClassifier _responseClassifier200401;
        private static ResponseClassifier ResponseClassifier200401 => _responseClassifier200401 ??= new StatusCodeClassifier(stackalloc ushort[] { 200, 401 });
    }
}
