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
    /// <summary> The Deal service client. </summary>
    public partial class DealClient
    {
        private readonly HttpPipeline _pipeline;
        private readonly Uri _endpoint;

        /// <summary> The ClientDiagnostics is used to provide tracing support for the client library. </summary>
        internal ClientDiagnostics ClientDiagnostics { get; }

        /// <summary> The HTTP pipeline for sending and receiving REST requests and responses. </summary>
        public virtual HttpPipeline Pipeline => _pipeline;

        /// <summary> Initializes a new instance of DealClient for mocking. </summary>
        protected DealClient()
        {
        }

        /// <summary> Initializes a new instance of DealClient. </summary>
        /// <param name="endpoint"> server parameter. </param>
        /// <param name="options"> The options for configuring the client. </param>
        public DealClient(Uri endpoint = null, MyCRMAPIClientOptions options = null)
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
        ///     type: &quot;deals&quot;,
        ///     id: string,
        ///     attributes: {
        ///       updated: string (ISO 8601 Format),
        ///       created: string (ISO 8601 Format),
        ///       dates: {
        ///         settled: ImportantDatesSetSettled,
        ///         lodged: ImportantDatesSetLodged,
        ///         financeClause: ImportantDatesSetFinanceClause,
        ///         notProceeding: ImportantDatesSetNotProceeding,
        ///         approved: ImportantDatesSetApproved,
        ///         preApproved: ImportantDatesSetPreApproved,
        ///         conditionallyApproved: ImportantDatesSetConditionallyApproved,
        ///         preApprovedExpiry: ImportantDatesSetPreApprovedExpiry,
        ///         estimatedSettled: ImportantDatesSetEstimatedSettled
        ///       },
        ///       totalLoanAmount: number,
        ///       customStatusName: string,
        ///       dealType: &quot;Residential&quot; | &quot;Commercial&quot; | &quot;SmallBusiness&quot; | &quot;AssetFinance&quot; | &quot;PersonalLoan&quot; | &quot;DepositBond&quot;,
        ///       name: string,
        ///       dealStatus: &quot;NewLeads&quot; | &quot;Researching&quot; | &quot;PreApproved&quot; | &quot;ConditionallyApproved&quot; | &quot;UnconditionallyApproved&quot; | &quot;Lodged&quot; | &quot;InProgress&quot; | &quot;PendingSettlement&quot; | &quot;Varied&quot; | &quot;Repaid&quot; | &quot;Withdrawn&quot; | &quot;Cancelled&quot; | &quot;Settled&quot;,
        ///       opportunity: {
        ///         proposedLoanAmount: number,
        ///         securityAmount: number,
        ///         summaryNotes: string,
        ///         loanPurpose: string
        ///       },
        ///       splits: [
        ///         {
        ///           loanTermYears: number,
        ///           rateType: string,
        ///           loanStructureType: &quot;PrincipalInterest&quot; | &quot;InterestOnly&quot; | &quot;RevolvingCredit&quot; | &quot;Offset&quot;,
        ///           amount: number,
        ///           interestRate: number,
        ///           rateTypePeriodMonths: number,
        ///           paymentAmount: number,
        ///           fixedRateStartDate: SplitFixedRateStartDate,
        ///           fixedRateEndDate: SplitFixedRateEndDate,
        ///           interestOnlyStartDate: SplitInterestOnlyStartDate,
        ///           interestOnlyEndDate: SplitInterestOnlyEndDate,
        ///           repaymentHolidayStartDate: SplitRepaymentHolidayStartDate,
        ///           repaymentHolidayEndDate: SplitRepaymentHolidayEndDate
        ///         }
        ///       ],
        ///       lenderName: string
        ///     },
        ///     relationships: {
        ///       dealScenario: {
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
        ///       importantDates: {
        ///         links: {
        ///           self: string,
        ///           related: string
        ///         },
        ///         meta: Dictionary&lt;string, object&gt;,
        ///         data: [ResourceIdentifier]
        ///       },
        ///       participants: RelationshipsMultipleDocument,
        ///       externalReferences: RelationshipsMultipleDocument,
        ///       dealStructures: RelationshipsMultipleDocument,
        ///       contacts: {
        ///         links: RelationshipsMultipleDocumentLinks,
        ///         meta: Dictionary&lt;string, object&gt;,
        ///         data: [ResourceIdentifier]
        ///       },
        ///       adviser: RelationshipsSingleDocument,
        ///       dealNotes: RelationshipsMultipleDocument
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
        public virtual async Task<Response> GetDealAsync(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealClient.GetDeal");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetDealRequest(id, context);
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
        ///     type: &quot;deals&quot;,
        ///     id: string,
        ///     attributes: {
        ///       updated: string (ISO 8601 Format),
        ///       created: string (ISO 8601 Format),
        ///       dates: {
        ///         settled: ImportantDatesSetSettled,
        ///         lodged: ImportantDatesSetLodged,
        ///         financeClause: ImportantDatesSetFinanceClause,
        ///         notProceeding: ImportantDatesSetNotProceeding,
        ///         approved: ImportantDatesSetApproved,
        ///         preApproved: ImportantDatesSetPreApproved,
        ///         conditionallyApproved: ImportantDatesSetConditionallyApproved,
        ///         preApprovedExpiry: ImportantDatesSetPreApprovedExpiry,
        ///         estimatedSettled: ImportantDatesSetEstimatedSettled
        ///       },
        ///       totalLoanAmount: number,
        ///       customStatusName: string,
        ///       dealType: &quot;Residential&quot; | &quot;Commercial&quot; | &quot;SmallBusiness&quot; | &quot;AssetFinance&quot; | &quot;PersonalLoan&quot; | &quot;DepositBond&quot;,
        ///       name: string,
        ///       dealStatus: &quot;NewLeads&quot; | &quot;Researching&quot; | &quot;PreApproved&quot; | &quot;ConditionallyApproved&quot; | &quot;UnconditionallyApproved&quot; | &quot;Lodged&quot; | &quot;InProgress&quot; | &quot;PendingSettlement&quot; | &quot;Varied&quot; | &quot;Repaid&quot; | &quot;Withdrawn&quot; | &quot;Cancelled&quot; | &quot;Settled&quot;,
        ///       opportunity: {
        ///         proposedLoanAmount: number,
        ///         securityAmount: number,
        ///         summaryNotes: string,
        ///         loanPurpose: string
        ///       },
        ///       splits: [
        ///         {
        ///           loanTermYears: number,
        ///           rateType: string,
        ///           loanStructureType: &quot;PrincipalInterest&quot; | &quot;InterestOnly&quot; | &quot;RevolvingCredit&quot; | &quot;Offset&quot;,
        ///           amount: number,
        ///           interestRate: number,
        ///           rateTypePeriodMonths: number,
        ///           paymentAmount: number,
        ///           fixedRateStartDate: SplitFixedRateStartDate,
        ///           fixedRateEndDate: SplitFixedRateEndDate,
        ///           interestOnlyStartDate: SplitInterestOnlyStartDate,
        ///           interestOnlyEndDate: SplitInterestOnlyEndDate,
        ///           repaymentHolidayStartDate: SplitRepaymentHolidayStartDate,
        ///           repaymentHolidayEndDate: SplitRepaymentHolidayEndDate
        ///         }
        ///       ],
        ///       lenderName: string
        ///     },
        ///     relationships: {
        ///       dealScenario: {
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
        ///       importantDates: {
        ///         links: {
        ///           self: string,
        ///           related: string
        ///         },
        ///         meta: Dictionary&lt;string, object&gt;,
        ///         data: [ResourceIdentifier]
        ///       },
        ///       participants: RelationshipsMultipleDocument,
        ///       externalReferences: RelationshipsMultipleDocument,
        ///       dealStructures: RelationshipsMultipleDocument,
        ///       contacts: {
        ///         links: RelationshipsMultipleDocumentLinks,
        ///         meta: Dictionary&lt;string, object&gt;,
        ///         data: [ResourceIdentifier]
        ///       },
        ///       adviser: RelationshipsSingleDocument,
        ///       dealNotes: RelationshipsMultipleDocument
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
        public virtual Response GetDeal(int id, RequestContext context = null)
        {
            using var scope = ClientDiagnostics.CreateScope("DealClient.GetDeal");
            scope.Start();
            try
            {
                using HttpMessage message = CreateGetDealRequest(id, context);
                return _pipeline.ProcessMessage(message, context);
            }
            catch (Exception e)
            {
                scope.Failed(e);
                throw;
            }
        }

        internal HttpMessage CreateGetDealRequest(int id, RequestContext context)
        {
            var message = _pipeline.CreateMessage(context, ResponseClassifier200401);
            var request = message.Request;
            request.Method = RequestMethod.Get;
            var uri = new RawRequestUriBuilder();
            uri.Reset(_endpoint);
            uri.AppendPath("/jsonapi/deals/", false);
            uri.AppendPath(id, true);
            request.Uri = uri;
            request.Headers.Add("Accept", "application/vnd.api+json");
            return message;
        }

        private static ResponseClassifier _responseClassifier200401;
        private static ResponseClassifier ResponseClassifier200401 => _responseClassifier200401 ??= new StatusCodeClassifier(stackalloc ushort[] { 200, 401 });
    }
}
