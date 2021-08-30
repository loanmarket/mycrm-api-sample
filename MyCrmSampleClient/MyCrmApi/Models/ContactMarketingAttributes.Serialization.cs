// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class ContactMarketingAttributes : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(HasMarketingConsent))
            {
                if (HasMarketingConsent != null)
                {
                    writer.WritePropertyName("hasMarketingConsent");
                    writer.WriteBooleanValue(HasMarketingConsent.Value);
                }
                else
                {
                    writer.WriteNull("hasMarketingConsent");
                }
            }
            writer.WriteEndObject();
        }

        internal static ContactMarketingAttributes DeserializeContactMarketingAttributes(JsonElement element)
        {
            Optional<DateTimeOffset?> updated = default;
            Optional<bool?> hasMarketingConsent = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("updated"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        updated = null;
                        continue;
                    }
                    updated = property.Value.GetDateTimeOffset("O");
                    continue;
                }
                if (property.NameEquals("hasMarketingConsent"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        hasMarketingConsent = null;
                        continue;
                    }
                    hasMarketingConsent = property.Value.GetBoolean();
                    continue;
                }
            }
            return new ContactMarketingAttributes(Optional.ToNullable(updated), Optional.ToNullable(hasMarketingConsent));
        }
    }
}
