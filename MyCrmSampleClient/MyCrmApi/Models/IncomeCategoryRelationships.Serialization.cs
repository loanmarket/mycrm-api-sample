// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class IncomeCategoryRelationships : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(IncomeTypes))
            {
                writer.WritePropertyName("incomeTypes");
                writer.WriteObjectValue(IncomeTypes);
            }
            writer.WriteEndObject();
        }

        internal static IncomeCategoryRelationships DeserializeIncomeCategoryRelationships(JsonElement element)
        {
            Optional<RelationshipsMultipleDocument> incomeTypes = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("incomeTypes"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    incomeTypes = RelationshipsMultipleDocument.DeserializeRelationshipsMultipleDocument(property.Value);
                    continue;
                }
            }
            return new IncomeCategoryRelationships(incomeTypes.Value);
        }
    }
}