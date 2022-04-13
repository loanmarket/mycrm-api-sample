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
        /// <param name="loanPurpose"></param>
        internal Opportunity(double? proposedLoanAmount, double? securityAmount, string summaryNotes, string loanPurpose)
        {
            ProposedLoanAmount = proposedLoanAmount;
            SecurityAmount = securityAmount;
            SummaryNotes = summaryNotes;
            LoanPurpose = loanPurpose;
        }

        /// <summary> Gets or sets the proposed loan amount. </summary>
        public double? ProposedLoanAmount { get; set; }
        /// <summary> Gets or sets the security amount. </summary>
        public double? SecurityAmount { get; set; }
        /// <summary> Gets or sets the summary notes. </summary>
        public string SummaryNotes { get; set; }
        /// <summary> Gets or sets the loan purpose. </summary>
        public string LoanPurpose { get; set; }
    }
}
