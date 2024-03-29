// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class DealStructureRelationships : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(DealImportantDates))
            {
                writer.WritePropertyName("dealImportantDates");
                writer.WriteObjectValue(DealImportantDates);
            }
            if (Optional.IsDefined(Deal))
            {
                writer.WritePropertyName("deal");
                writer.WriteObjectValue(Deal);
            }
            writer.WriteEndObject();
        }

        internal static DealStructureRelationships DeserializeDealStructureRelationships(JsonElement element)
        {
            Optional<RelationshipsMultipleDocument> dealImportantDates = default;
            Optional<DealStructureRelationshipsDeal> deal = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("dealImportantDates"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    dealImportantDates = RelationshipsMultipleDocument.DeserializeRelationshipsMultipleDocument(property.Value);
                    continue;
                }
                if (property.NameEquals("deal"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    deal = DealStructureRelationshipsDeal.DeserializeDealStructureRelationshipsDeal(property.Value);
                    continue;
                }
            }
            return new DealStructureRelationships(dealImportantDates.Value, deal.Value);
        }
    }
}
