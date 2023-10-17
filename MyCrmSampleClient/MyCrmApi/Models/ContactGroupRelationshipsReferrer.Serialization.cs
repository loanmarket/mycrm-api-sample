// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class ContactGroupRelationshipsReferrer : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("data");
            writer.WriteObjectValue(Data);
            writer.WriteEndObject();
        }

        internal static ContactGroupRelationshipsReferrer DeserializeContactGroupRelationshipsReferrer(JsonElement element)
        {
            Optional<RelationshipsSingleDocumentLinks> links = default;
            Optional<IReadOnlyDictionary<string, object>> meta = default;
            ResourceIdentifier data = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("links"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    links = RelationshipsSingleDocumentLinks.DeserializeRelationshipsSingleDocumentLinks(property.Value);
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
                if (property.NameEquals("data"))
                {
                    data = ResourceIdentifier.DeserializeResourceIdentifier(property.Value);
                    continue;
                }
            }
            return new ContactGroupRelationshipsReferrer(links.Value, Optional.ToDictionary(meta), data);
        }
    }
}
