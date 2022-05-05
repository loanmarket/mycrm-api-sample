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
    /// <summary> The Liability service client. </summary>
    public partial class LiabilityClient
    {
        private readonly HttpPipeline _pipeline;
        private readonly Uri _endpoint;

        /// <summary> The ClientDiagnostics is used to provide tracing support for the client library. </summary>
        internal ClientDiagnostics ClientDiagnostics { get; }

        /// <summary> The HTTP pipeline for sending and receiving REST requests and responses. </summary>
        public virtual HttpPipeline Pipeline => _pipeline;

        /// <summary> Initializes a new instance of LiabilityClient for mocking. </summary>
        protected LiabilityClient()
        {
        }

        /// <summary> Initializes a new instance of LiabilityClient. </summary>
        /// <param name="endpoint"> server parameter. </param>
        /// <param name="options"> The options for configuring the client. </param>
        public LiabilityClient(Uri endpoint = null, MyCRMAPIClientOptions options = null)
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
        ///     describedby: string,
        ///     first: string,
        ///     last: string,
        ///     prev: string,
        ///     next: string
        ///   },
        ///   data: {
        ///     type: string,
        ///     id: string,
        ///     type: &quot;liabilities&quot;,
        ///     id: string,
        ///     attributes: {
        ///       created: string (ISO 8601 Format),
        ///       repayment: number,
        ///       value: number,
        ///       limit: number,
        ///       accountName: string,
        ///       accountNumber: string,
        ///       bsb: string,
        ///       interestTaxDeductible: boolean,
        ///       interestRate: number,
        ///       isAccountDetailsKnown: boolean,
        ///       liabilityTypeId: number,
        ///       loanTerm: number,
        ///       repaymentFrequency: string,
        ///       liabilityCategory: string,
        ///       liabilityCategoryId: number,
        ///       liabilityType: string,
        ///       mortgagePriority: string,
        ///       updated: string (ISO 8601 Format),
        ///       creditorName: string,
        ///       loanRepaymentType: &quot;InterestOnly&quot; | &quot;PrincipalInterest&quot;
        ///     },
        ///     relationships: {
        ///       linkedAsset: {
        ///         links: {
        ///           self: string,
        ///           related: string
        ///         },
        ///         meta: Dictionary&lt;string, object&gt;,
        ///         data: {
        ///           type: string,
        ///           id: string
        ///         }
        ///       },
        ///       ownership: {
        ///         links: {
        ///           self: string,
        ///           related: string
        ///         },
        ///         meta: Dictionary&lt;string, object&gt;,
        ///         data: [ResourceIdentifier]
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
        public virtual async Task<Response> GetLiabilityAsync(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("LiabilityClient.GetLiability");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetLiabilityRequest(id, context);
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
        ///     describedby: string,
        ///     first: string,
        ///     last: string,
        ///     prev: string,
        ///     next: string
        ///   },
        ///   data: {
        ///     type: string,
        ///     id: string,
        ///     type: &quot;liabilities&quot;,
        ///     id: string,
        ///     attributes: {
        ///       created: string (ISO 8601 Format),
        ///       repayment: number,
        ///       value: number,
        ///       limit: number,
        ///       accountName: string,
        ///       accountNumber: string,
        ///       bsb: string,
        ///       interestTaxDeductible: boolean,
        ///       interestRate: number,
        ///       isAccountDetailsKnown: boolean,
        ///       liabilityTypeId: number,
        ///       loanTerm: number,
        ///       repaymentFrequency: string,
        ///       liabilityCategory: string,
        ///       liabilityCategoryId: number,
        ///       liabilityType: string,
        ///       mortgagePriority: string,
        ///       updated: string (ISO 8601 Format),
        ///       creditorName: string,
        ///       loanRepaymentType: &quot;InterestOnly&quot; | &quot;PrincipalInterest&quot;
        ///     },
        ///     relationships: {
        ///       linkedAsset: {
        ///         links: {
        ///           self: string,
        ///           related: string
        ///         },
        ///         meta: Dictionary&lt;string, object&gt;,
        ///         data: {
        ///           type: string,
        ///           id: string
        ///         }
        ///       },
        ///       ownership: {
        ///         links: {
        ///           self: string,
        ///           related: string
        ///         },
        ///         meta: Dictionary&lt;string, object&gt;,
        ///         data: [ResourceIdentifier]
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
        public virtual Response GetLiability(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("LiabilityClient.GetLiability");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetLiabilityRequest(id, context);
                return _pipeline.ProcessMessage(message, context);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        internal HttpMessage CreateGetLiabilityRequest(int id, RequestContext context)
        {
            var message = _pipeline.CreateMessage(context, ResponseClassifier200401);
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/jsonapi/liabilities/", false);
            uri.AppendPath(id, true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        private static ResponseClassifier _responseClassifier200401;
        private static ResponseClassifier ResponseClassifier200401 => _responseClassifier200401 ??= new StatusCodeClassifier(stackalloc ushort[] { 200, 401 });
    }
}
