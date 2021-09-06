// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class DealParticipantAttributes : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(IsApplicant))
            {
                if (IsApplicant != null)
                {
                    writer.WritePropertyName("isApplicant");
                    writer.WriteBooleanValue(IsApplicant.Value);
                }
                else
                {
                    writer.WriteNull("isApplicant");
                }
            }
            if (Optional.IsDefined(IsDependent))
            {
                if (IsDependent != null)
                {
                    writer.WritePropertyName("isDependent");
                    writer.WriteBooleanValue(IsDependent.Value);
                }
                else
                {
                    writer.WriteNull("isDependent");
                }
            }
            writer.WriteEndObject();
        }

        internal static DealParticipantAttributes DeserializeDealParticipantAttributes(JsonElement element)
        {
            Optional<DateTimeOffset?> updated = default;
            Optional<DateTimeOffset?> created = default;
            Optional<bool?> isApplicant = default;
            Optional<bool?> isDependent = default;
            Optional<bool?> isGuarantor = default;
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
                if (property.NameEquals("isApplicant"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        isApplicant = null;
                        continue;
                    }
                    isApplicant = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("isDependent"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        isDependent = null;
                        continue;
                    }
                    isDependent = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("isGuarantor"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        isGuarantor = null;
                        continue;
                    }
                    isGuarantor = property.Value.GetBoolean();
                    continue;
                }
            }
            return new DealParticipantAttributes(Optional.ToNullable(updated), Optional.ToNullable(created), Optional.ToNullable(isApplicant), Optional.ToNullable(isDependent), Optional.ToNullable(isGuarantor));
        }
    }
}
