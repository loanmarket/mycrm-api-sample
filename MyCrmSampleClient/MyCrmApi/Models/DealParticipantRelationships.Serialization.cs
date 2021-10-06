// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class DealParticipantRelationships : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(Contact))
            {
                writer.WritePropertyName("contact");
                writer.WriteObjectValue(Contact);
            }
            if (Optional.IsDefined(Deal))
            {
                writer.WritePropertyName("deal");
                writer.WriteObjectValue(Deal);
            }
            writer.WriteEndObject();
        }

        internal static DealParticipantRelationships DeserializeDealParticipantRelationships(JsonElement element)
        {
            Optional<RelationshipsSingleDocument> contact = default;
            Optional<RelationshipsSingleDocument> deal = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("contact"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    contact = RelationshipsSingleDocument.DeserializeRelationshipsSingleDocument(property.Value);
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
            return new DealParticipantRelationships(contact.Value, deal.Value);
        }
    }
}
