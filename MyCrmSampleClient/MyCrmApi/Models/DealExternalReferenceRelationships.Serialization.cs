// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class DealExternalReferenceRelationships : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(Integration))
            {
                writer.WritePropertyName("integration");
                writer.WriteObjectValue(Integration);
            }
            if (Optional.IsDefined(Deal))
            {
                writer.WritePropertyName("deal");
                writer.WriteObjectValue(Deal);
            }
            writer.WriteEndObject();
        }

        internal static DealExternalReferenceRelationships DeserializeDealExternalReferenceRelationships(JsonElement element)
        {
            Optional<RelationshipsSingleDocument> integration = default;
            Optional<RelationshipsSingleDocument> deal = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("integration"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    integration = RelationshipsSingleDocument.DeserializeRelationshipsSingleDocument(property.Value);
                    continue;
                }
                if (property.NameEquals("deal"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    deal = RelationshipsSingleDocument.DeserializeRelationshipsSingleDocument(property.Value);
                    continue;
                }
            }
            return new DealExternalReferenceRelationships(integration.Value, deal.Value);
        }
    }
}
