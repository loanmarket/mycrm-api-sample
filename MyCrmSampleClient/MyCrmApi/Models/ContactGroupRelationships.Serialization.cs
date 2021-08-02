// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class ContactGroupRelationships : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(Contacts))
            {
                writer.WritePropertyName("contacts");
                writer.WriteObjectValue(Contacts);
            }
            writer.WriteEndObject();
        }

        internal static ContactGroupRelationships DeserializeContactGroupRelationships(JsonElement element)
        {
            Optional<RelationshipsMultipleDocument> contacts = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("contacts"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    contacts = RelationshipsMultipleDocument.DeserializeRelationshipsMultipleDocument(property.Value);
                    continue;
                }
            }
            return new ContactGroupRelationships(contacts.Value);
        }
    }
}
