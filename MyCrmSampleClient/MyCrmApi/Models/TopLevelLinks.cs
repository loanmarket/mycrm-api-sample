// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The TopLevelLinks. </summary>
    public partial class TopLevelLinks
    {
        /// <summary> Initializes a new instance of TopLevelLinks. </summary>
        internal TopLevelLinks()
        {
        }

        /// <summary> Initializes a new instance of TopLevelLinks. </summary>
        /// <param name="self"></param>
        /// <param name="related"></param>
        /// <param name="describedBy"></param>
        /// <param name="first"></param>
        /// <param name="last"></param>
        /// <param name="prev"></param>
        /// <param name="next"></param>
        internal TopLevelLinks(string self, string related, string describedBy, string first, string last, string prev, string next)
        {
            Self = self;
            Related = related;
            DescribedBy = describedBy;
            First = first;
            Last = last;
            Prev = prev;
            Next = next;
        }

        /// <summary> Gets the self. </summary>
        public string Self { get; }
        /// <summary> Gets the related. </summary>
        public string Related { get; }
        /// <summary> Gets the described by. </summary>
        public string DescribedBy { get; }
        /// <summary> Gets the first. </summary>
        public string First { get; }
        /// <summary> Gets the last. </summary>
        public string Last { get; }
        /// <summary> Gets the prev. </summary>
        public string Prev { get; }
        /// <summary> Gets the next. </summary>
        public string Next { get; }
    }
}
