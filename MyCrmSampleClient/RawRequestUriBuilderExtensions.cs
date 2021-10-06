using Azure.Core;
using MyCrmSampleClient.MyCrmApi.Models;

namespace MyCrmSampleClient
{
    namespace MyCrmApi
    {
        public static class RawRequestUriBuilderExtensions
        {
            internal static void AppendQuery(this RawRequestUriBuilder uri, string name, Pagination? value,
                bool escapeValue)
            {
                if (value == null) return;

                if (value.Size != null) uri.AppendQuery("page[size]", value.Size.Value);
                if (value.Number != null) uri.AppendQuery("page[number]", value.Number.Value);
            }
        }
    }
}