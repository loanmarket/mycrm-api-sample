// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

namespace MyCrmSampleClient.MyCrmApi.Models
{
    /// <summary> The DealNoteAttributes. </summary>
    public partial class DealNoteAttributes
    {
        /// <summary> Initializes a new instance of DealNoteAttributes. </summary>
        public DealNoteAttributes()
        {
        }

        /// <summary> Initializes a new instance of DealNoteAttributes. </summary>
        /// <param name="title"></param>
        /// <param name="detail"></param>
        internal DealNoteAttributes(string title, string detail)
        {
            Title = title;
            Detail = detail;
        }

        /// <summary> Gets or sets the title. </summary>
        public string Title { get; set; }
        /// <summary> Gets or sets the detail. </summary>
        public string Detail { get; set; }
    }
}
