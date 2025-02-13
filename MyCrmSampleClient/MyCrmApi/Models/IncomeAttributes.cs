// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The IncomeAttributes. </summary>
    public partial class IncomeAttributes
    {
        /// <summary> Initializes a new instance of IncomeAttributes. </summary>
        public IncomeAttributes()
        {
        }

        /// <summary> Initializes a new instance of IncomeAttributes. </summary>
        /// <param name="updated"></param>
        /// <param name="created"></param>
        /// <param name="description"></param>
        /// <param name="value"></param>
        /// <param name="incomeTypeId"></param>
        /// <param name="incomeVerification"></param>
        /// <param name="frequency"></param>
        /// <param name="incomeCategory"></param>
        /// <param name="incomeType"></param>
        /// <param name="incomeCategoryId"></param>
        internal IncomeAttributes(DateTimeOffset? updated, DateTimeOffset? created, string description, double? value, int? incomeTypeId, string incomeVerification, string frequency, string incomeCategory, string incomeType, int? incomeCategoryId)
        {
            Updated = updated;
            Created = created;
            Description = description;
            Value = value;
            IncomeTypeId = incomeTypeId;
            IncomeVerification = incomeVerification;
            Frequency = frequency;
            IncomeCategory = incomeCategory;
            IncomeType = incomeType;
            IncomeCategoryId = incomeCategoryId;
        }

        /// <summary> Gets the updated. </summary>
        public DateTimeOffset? Updated { get; }
        /// <summary> Gets the created. </summary>
        public DateTimeOffset? Created { get; }
        /// <summary> Gets or sets the description. </summary>
        public string Description { get; set; }
        /// <summary> Gets or sets the value. </summary>
        public double? Value { get; set; }
        /// <summary> Gets the income type id. </summary>
        public int? IncomeTypeId { get; }
        /// <summary> Gets the income verification. </summary>
        public string IncomeVerification { get; }
        /// <summary> Gets the frequency. </summary>
        public string Frequency { get; }
        /// <summary> Gets the income category. </summary>
        public string IncomeCategory { get; }
        /// <summary> Gets the income type. </summary>
        public string IncomeType { get; }
        /// <summary> Gets the income category id. </summary>
        public int? IncomeCategoryId { get; }
    }
}
