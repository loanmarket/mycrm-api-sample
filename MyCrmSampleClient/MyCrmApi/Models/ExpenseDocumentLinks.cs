// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The ExpenseDocumentLinks. </summary>
    public partial class ExpenseDocumentLinks : TopLevelLinks
    {
        /// <summary> Initializes a new instance of ExpenseDocumentLinks. </summary>
        internal ExpenseDocumentLinks()
        {
        }

        /// <summary> Initializes a new instance of ExpenseDocumentLinks. </summary>
        /// <param name="self"></param>
        /// <param name="related"></param>
        /// <param name="describedby"></param>
        /// <param name="first"></param>
        /// <param name="last"></param>
        /// <param name="prev"></param>
        /// <param name="next"></param>
        internal ExpenseDocumentLinks(string self, string related, string describedby, string first, string last, string prev, string next) : base(self, related, describedby, first, last, prev, next)
        {
        }
    }
}
