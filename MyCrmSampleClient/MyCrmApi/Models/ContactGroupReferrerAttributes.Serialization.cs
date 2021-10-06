// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class ContactGroupReferrerAttributes : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteEndObject();
        }

        internal static ContactGroupReferrerAttributes DeserializeContactGroupReferrerAttributes(JsonElement element)
        {
            Optional<string> referrerOrganisationName = default;
            Optional<string> referrerName = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("referrerOrganisationName"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        referrerOrganisationName = null;
                        continue;
                    }
                    referrerOrganisationName = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("referrerName"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        referrerName = null;
                        continue;
                    }
                    referrerName = property.Value.GetString();
                    continue;
                }
            }
            return new ContactGroupReferrerAttributes(referrerOrganisationName.Value, referrerName.Value);
        }
    }
}
