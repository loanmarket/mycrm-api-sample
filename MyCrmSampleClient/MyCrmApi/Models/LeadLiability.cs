// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The LeadLiability. </summary>
    public partial class LeadLiability
    {
        /// <summary> Initializes a new instance of LeadLiability. </summary>
        public LeadLiability()
        {
            Ownership = new ChangeTrackingList<LeadContactReference>();
        }

        /// <summary> Initializes a new instance of LeadLiability. </summary>
        /// <param name="lid"></param>
        /// <param name="liabilityTypeId"></param>
        /// <param name="liabilitySubTypeId"></param>
        /// <param name="description"></param>
        /// <param name="accountName"></param>
        /// <param name="bsb"></param>
        /// <param name="accountNumber"></param>
        /// <param name="value"></param>
        /// <param name="limit"></param>
        /// <param name="repayments"></param>
        /// <param name="repaymentFrequency"></param>
        /// <param name="interestRate"></param>
        /// <param name="interestTaxDeductible"></param>
        /// <param name="creditorName"></param>
        /// <param name="loanTerm"></param>
        /// <param name="loanRepaymentType"></param>
        /// <param name="mortgagePriority"></param>
        /// <param name="asset"></param>
        /// <param name="ownership"></param>
        internal LeadLiability(string lid, int? liabilityTypeId, int? liabilitySubTypeId, string description, string accountName, string bsb, string accountNumber, double? value, double? limit, double? repayments, Frequency? repaymentFrequency, double? interestRate, bool? interestTaxDeductible, string creditorName, int? loanTerm, LiabilityRepaymentType? loanRepaymentType, MortgagePriority? mortgagePriority, LeadAssetReference asset, IList<LeadContactReference> ownership)
        {
            Lid = lid;
            LiabilityTypeId = liabilityTypeId;
            LiabilitySubTypeId = liabilitySubTypeId;
            Description = description;
            AccountName = accountName;
            Bsb = bsb;
            AccountNumber = accountNumber;
            Value = value;
            Limit = limit;
            Repayments = repayments;
            RepaymentFrequency = repaymentFrequency;
            InterestRate = interestRate;
            InterestTaxDeductible = interestTaxDeductible;
            CreditorName = creditorName;
            LoanTerm = loanTerm;
            LoanRepaymentType = loanRepaymentType;
            MortgagePriority = mortgagePriority;
            Asset = asset;
            Ownership = ownership;
        }

        /// <summary> Gets or sets the lid. </summary>
        public string Lid { get; set; }
        /// <summary> Gets or sets the liability type id. </summary>
        public int? LiabilityTypeId { get; set; }
        /// <summary> Gets or sets the liability sub type id. </summary>
        public int? LiabilitySubTypeId { get; set; }
        /// <summary> Gets or sets the description. </summary>
        public string Description { get; set; }
        /// <summary> Gets or sets the account name. </summary>
        public string AccountName { get; set; }
        /// <summary> Gets or sets the bsb. </summary>
        public string Bsb { get; set; }
        /// <summary> Gets or sets the account number. </summary>
        public string AccountNumber { get; set; }
        /// <summary> Gets or sets the value. </summary>
        public double? Value { get; set; }
        /// <summary> Gets or sets the limit. </summary>
        public double? Limit { get; set; }
        /// <summary> Gets or sets the repayments. </summary>
        public double? Repayments { get; set; }
        /// <summary> Gets or sets the repayment frequency. </summary>
        public Frequency? RepaymentFrequency { get; set; }
        /// <summary> Gets or sets the interest rate. </summary>
        public double? InterestRate { get; set; }
        /// <summary> Gets or sets the interest tax deductible. </summary>
        public bool? InterestTaxDeductible { get; set; }
        /// <summary> Gets or sets the creditor name. </summary>
        public string CreditorName { get; set; }
        /// <summary> Gets or sets the loan term. </summary>
        public int? LoanTerm { get; set; }
        /// <summary> Gets or sets the loan repayment type. </summary>
        public LiabilityRepaymentType? LoanRepaymentType { get; set; }
        /// <summary> Gets or sets the mortgage priority. </summary>
        public MortgagePriority? MortgagePriority { get; set; }
        /// <summary> Gets or sets the asset. </summary>
        public LeadAssetReference Asset { get; set; }
        /// <summary> Gets or sets the ownership. </summary>
        public IList<LeadContactReference> Ownership { get; set; }
    }
}
