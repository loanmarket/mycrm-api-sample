// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class BusinessRelationships : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(ContactGroup))
            {
                writer.WritePropertyName("contactGroup");
                writer.WriteObjectValue(ContactGroup);
            }
            if (Optional.IsDefined(Ownership))
            {
                writer.WritePropertyName("ownership");
                writer.WriteObjectValue(Ownership);
            }
            if (Optional.IsDefined(Deals))
            {
                writer.WritePropertyName("deals");
                writer.WriteObjectValue(Deals);
            }
            if (Optional.IsDefined(Employments))
            {
                writer.WritePropertyName("employments");
                writer.WriteObjectValue(Employments);
            }
            writer.WriteEndObject();
        }

        internal static BusinessRelationships DeserializeBusinessRelationships(JsonElement element)
        {
            Optional<RelationshipsSingleDocument> contactGroup = default;
            Optional<RelationshipsMultipleDocument> ownership = default;
            Optional<RelationshipsMultipleDocument> deals = default;
            Optional<BusinessRelationshipsEmployments> employments = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("contactGroup"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    contactGroup = RelationshipsSingleDocument.DeserializeRelationshipsSingleDocument(property.Value);
                    continue;
                }
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
                if (property.NameEquals("deals"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    deals = RelationshipsMultipleDocument.DeserializeRelationshipsMultipleDocument(property.Value);
                    continue;
                }
                if (property.NameEquals("employments"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    employments = BusinessRelationshipsEmployments.DeserializeBusinessRelationshipsEmployments(property.Value);
                    continue;
                }
            }
            return new BusinessRelationships(contactGroup.Value, ownership.Value, deals.Value, employments.Value);
        }
    }
}
