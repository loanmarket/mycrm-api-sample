// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class ContactAddressDetailAttributes : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteEndObject();
        }

        internal static ContactAddressDetailAttributes DeserializeContactAddressDetailAttributes(JsonElement element)
        {
            Optional<DateTimeOffset?> created = default;
            Optional<DateTimeOffset?> updated = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("created"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        created = null;
                        continue;
                    }
                    created = property.Value.GetDateTimeOffset("O");
                    continue;
                }
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
            }
            return new ContactAddressDetailAttributes(Optional.ToNullable(created), Optional.ToNullable(updated));
        }
    }
}