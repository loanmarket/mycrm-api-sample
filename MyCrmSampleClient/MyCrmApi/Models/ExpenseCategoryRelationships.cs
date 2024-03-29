// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The ExpenseCategoryRelationships. </summary>
    public partial class ExpenseCategoryRelationships
    {
        /// <summary> Initializes a new instance of ExpenseCategoryRelationships. </summary>
        public ExpenseCategoryRelationships()
        {
        }

        /// <summary> Initializes a new instance of ExpenseCategoryRelationships. </summary>
        /// <param name="expenseTypes"></param>
        internal ExpenseCategoryRelationships(ExpenseCategoryRelationshipsExpenseTypes expenseTypes)
        {
            ExpenseTypes = expenseTypes;
        }

        /// <summary> Gets or sets the expense types. </summary>
        public ExpenseCategoryRelationshipsExpenseTypes ExpenseTypes { get; set; }
    }
}
