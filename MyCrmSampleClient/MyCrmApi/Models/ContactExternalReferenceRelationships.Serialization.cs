// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class ContactExternalReferenceRelationships : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(Integration))
            {
                writer.WritePropertyName("integration");
                writer.WriteObjectValue(Integration);
            }
            if (Optional.IsDefined(Contact))
            {
                writer.WritePropertyName("contact");
                writer.WriteObjectValue(Contact);
            }
            writer.WriteEndObject();
        }

        internal static ContactExternalReferenceRelationships DeserializeContactExternalReferenceRelationships(JsonElement element)
        {
            Optional<RelationshipsSingleDocument> integration = default;
            Optional<RelationshipsSingleDocument> contact = default;
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
            return new ContactExternalReferenceRelationships(integration.Value, contact.Value);
        }
    }
}
