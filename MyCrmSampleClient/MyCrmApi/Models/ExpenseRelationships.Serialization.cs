// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class ExpenseRelationships : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(Ownership))
            {
                writer.WritePropertyName("ownership");
                writer.WriteObjectValue(Ownership);
            }
            if (Optional.IsDefined(Financial))
            {
                writer.WritePropertyName("financial");
                writer.WriteObjectValue(Financial);
            }
            writer.WriteEndObject();
        }

        internal static ExpenseRelationships DeserializeExpenseRelationships(JsonElement element)
        {
            Optional<RelationshipsMultipleDocument> ownership = default;
            Optional<RelationshipsSingleDocument> financial = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("ownership"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    ownership = RelationshipsMultipleDocument.DeserializeRelationshipsMultipleDocument(property.Value);
                    continue;
                }
                if (property.NameEquals("financial"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    financial = RelationshipsSingleDocument.DeserializeRelationshipsSingleDocument(property.Value);
                    continue;
                }
            }
            return new ExpenseRelationships(ownership.Value, financial.Value);
        }
    }
}