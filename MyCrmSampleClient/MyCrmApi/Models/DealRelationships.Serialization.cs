// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class DealRelationships : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(DealScenario))
            {
                writer.WritePropertyName("dealScenario");
                writer.WriteObjectValue(DealScenario);
            }
            if (Optional.IsDefined(DealNotes))
            {
                writer.WritePropertyName("dealNotes");
                writer.WriteObjectValue(DealNotes);
            }
            if (Optional.IsDefined(Participants))
            {
                writer.WritePropertyName("participants");
                writer.WriteObjectValue(Participants);
            }
            if (Optional.IsDefined(ExternalReferences))
            {
                writer.WritePropertyName("externalReferences");
                writer.WriteObjectValue(ExternalReferences);
            }
            if (Optional.IsDefined(DealStructures))
            {
                writer.WritePropertyName("dealStructures");
                writer.WriteObjectValue(DealStructures);
            }
            if (Optional.IsDefined(DealStatus))
            {
                writer.WritePropertyName("dealStatus");
                writer.WriteObjectValue(DealStatus);
            }
            if (Optional.IsDefined(Contacts))
            {
                writer.WritePropertyName("contacts");
                writer.WriteObjectValue(Contacts);
            }
            if (Optional.IsDefined(ImportantDates))
            {
                writer.WritePropertyName("importantDates");
                writer.WriteObjectValue(ImportantDates);
            }
            writer.WriteEndObject();
        }

        internal static DealRelationships DeserializeDealRelationships(JsonElement element)
        {
            Optional<RelationshipsSingleDocument> dealScenario = default;
            Optional<RelationshipsMultipleDocument> dealNotes = default;
            Optional<RelationshipsMultipleDocument> participants = default;
            Optional<RelationshipsMultipleDocument> externalReferences = default;
            Optional<RelationshipsMultipleDocument> dealStructures = default;
            Optional<RelationshipsSingleDocument> dealStatus = default;
            Optional<DealRelationshipsContacts> contacts = default;
            Optional<RelationshipsMultipleDocument> importantDates = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("dealScenario"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    dealScenario = RelationshipsSingleDocument.DeserializeRelationshipsSingleDocument(property.Value);
                    continue;
                }
                if (property.NameEquals("dealNotes"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    dealNotes = RelationshipsMultipleDocument.DeserializeRelationshipsMultipleDocument(property.Value);
                    continue;
                }
                if (property.NameEquals("participants"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    participants = RelationshipsMultipleDocument.DeserializeRelationshipsMultipleDocument(property.Value);
                    continue;
                }
                if (property.NameEquals("externalReferences"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    externalReferences = RelationshipsMultipleDocument.DeserializeRelationshipsMultipleDocument(property.Value);
                    continue;
                }
                if (property.NameEquals("dealStructures"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    dealStructures = RelationshipsMultipleDocument.DeserializeRelationshipsMultipleDocument(property.Value);
                    continue;
                }
                if (property.NameEquals("dealStatus"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    dealStatus = RelationshipsSingleDocument.DeserializeRelationshipsSingleDocument(property.Value);
                    continue;
                }
                if (property.NameEquals("contacts"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    contacts = DealRelationshipsContacts.DeserializeDealRelationshipsContacts(property.Value);
                    continue;
                }
                if (property.NameEquals("importantDates"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    importantDates = RelationshipsMultipleDocument.DeserializeRelationshipsMultipleDocument(property.Value);
                    continue;
                }
            }
            return new DealRelationships(dealScenario.Value, dealNotes.Value, participants.Value, externalReferences.Value, dealStructures.Value, dealStatus.Value, contacts.Value, importantDates.Value);
        }
    }
}
