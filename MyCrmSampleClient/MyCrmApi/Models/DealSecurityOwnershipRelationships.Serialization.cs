// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class DealSecurityOwnershipRelationships : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(Business))
            {
                writer.WritePropertyName("business");
                writer.WriteObjectValue(Business);
            }
            if (Optional.IsDefined(Contact))
            {
                writer.WritePropertyName("contact");
                writer.WriteObjectValue(Contact);
            }
            writer.WriteEndObject();
        }

        internal static DealSecurityOwnershipRelationships DeserializeDealSecurityOwnershipRelationships(JsonElement element)
        {
            Optional<RelationshipsSingleDocument> business = default;
            Optional<RelationshipsSingleDocument> contact = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("business"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    business = RelationshipsSingleDocument.DeserializeRelationshipsSingleDocument(property.Value);
                    continue;
                }
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
            }
            return new DealSecurityOwnershipRelationships(business.Value, contact.Value);
        }
    }
}
