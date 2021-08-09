// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class DealNote : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(Attributes))
            {
                writer.WritePropertyName("attributes");
                writer.WriteObjectValue(Attributes);
            }
            if (Optional.IsDefined(Relationships))
            {
                writer.WritePropertyName("relationships");
                writer.WriteObjectValue(Relationships);
            }
            writer.WritePropertyName("type");
            writer.WriteStringValue(Type);
            writer.WritePropertyName("id");
            writer.WriteStringValue(Id);
            writer.WriteEndObject();
        }

        internal static DealNote DeserializeDealNote(JsonElement element)
        {
            Optional<DealNoteAttributes> attributes = default;
            Optional<DealNoteRelationships> relationships = default;
            Optional<DealNoteLinks> links = default;
            Optional<IReadOnlyDictionary<string, object>> meta = default;
            string type = default;
            string id = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("attributes"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    attributes = DealNoteAttributes.DeserializeDealNoteAttributes(property.Value);
                    continue;
                }
                if (property.NameEquals("relationships"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    relationships = DealNoteRelationships.DeserializeDealNoteRelationships(property.Value);
                    continue;
                }
                if (property.NameEquals("links"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    links = DealNoteLinks.DeserializeDealNoteLinks(property.Value);
                    continue;
                }
                if (property.NameEquals("meta"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    Dictionary<string, object> dictionary = new Dictionary<string, object>();
                    foreach (var property0 in property.Value.EnumerateObject())
                    {
                        dictionary.Add(property0.Name, property0.Value.GetObject());
                    }
                    meta = dictionary;
                    continue;
                }
                if (property.NameEquals("type"))
                {
                    type = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("id"))
                {
                    id = property.Value.GetString();
                    continue;
                }
            }
            return new DealNote(type, id, attributes.Value, relationships.Value, links.Value, Optional.ToDictionary(meta));
        }
    }
}
