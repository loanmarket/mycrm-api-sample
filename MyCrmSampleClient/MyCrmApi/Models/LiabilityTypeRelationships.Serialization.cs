// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class LiabilityTypeRelationships : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(LiabilityCategory))
            {
                writer.WritePropertyName("liabilityCategory");
                writer.WriteObjectValue(LiabilityCategory);
            }
            writer.WriteEndObject();
        }

        internal static LiabilityTypeRelationships DeserializeLiabilityTypeRelationships(JsonElement element)
        {
            Optional<LiabilityTypeRelationshipsLiabilityCategory> liabilityCategory = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("liabilityCategory"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    liabilityCategory = LiabilityTypeRelationshipsLiabilityCategory.DeserializeLiabilityTypeRelationshipsLiabilityCategory(property.Value);
                    continue;
                }
            }
            return new LiabilityTypeRelationships(liabilityCategory.Value);
        }
    }
}
