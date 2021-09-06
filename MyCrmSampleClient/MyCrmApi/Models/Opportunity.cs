// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The Opportunity. </summary>
    public partial class Opportunity
    {
        /// <summary> Initializes a new instance of Opportunity. </summary>
        public Opportunity()
        {
        }

        /// <summary> Initializes a new instance of Opportunity. </summary>
        /// <param name="proposedLoanAmount"></param>
        /// <param name="securityAmount"></param>
        /// <param name="summaryNotes"></param>
        internal Opportunity(double? proposedLoanAmount, double? securityAmount, string summaryNotes)
        {
            ProposedLoanAmount = proposedLoanAmount;
            SecurityAmount = securityAmount;
            SummaryNotes = summaryNotes;
        }

        public double? ProposedLoanAmount { get; set; }
        public double? SecurityAmount { get; set; }
        public string SummaryNotes { get; set; }
    }
}
