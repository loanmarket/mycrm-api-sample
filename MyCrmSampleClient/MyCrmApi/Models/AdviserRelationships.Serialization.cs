// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class AdviserRelationships : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(AdviserDetails))
            {
                writer.WritePropertyName("adviserDetails");
                writer.WriteObjectValue(AdviserDetails);
            }
            if (Optional.IsDefined(ContactGroups))
            {
                writer.WritePropertyName("contactGroups");
                writer.WriteObjectValue(ContactGroups);
            }
            writer.WriteEndObject();
        }

        internal static AdviserRelationships DeserializeAdviserRelationships(JsonElement element)
        {
            Optional<RelationshipsSingleDocument> adviserDetails = default;
            Optional<RelationshipsMultipleDocument> contactGroups = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("adviserDetails"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    adviserDetails = RelationshipsSingleDocument.DeserializeRelationshipsSingleDocument(property.Value);
                    continue;
                }
                if (property.NameEquals("contactGroups"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    contactGroups = RelationshipsMultipleDocument.DeserializeRelationshipsMultipleDocument(property.Value);
                    continue;
                }
            }
            return new AdviserRelationships(adviserDetails.Value, contactGroups.Value);
        }
    }
}
