// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class ContactAddressLinks
    {
        internal static ContactAddressLinks DeserializeContactAddressLinks(JsonElement element)
        {
            Optional<string> self = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("self"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        self = null;
                        continue;
                    }
                    self = property.Value.GetString();
                    continue;
                }
            }
            return new ContactAddressLinks(self.Value);
        }
    }
}
