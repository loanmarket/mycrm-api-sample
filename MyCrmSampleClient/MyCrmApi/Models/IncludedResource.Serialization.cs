// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class IncludedResource : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("type");
            writer.WriteStringValue(Type);
            writer.WritePropertyName("id");
            writer.WriteStringValue(Id);
            writer.WriteEndObject();
        }

        internal static IncludedResource DeserializeIncludedResource(JsonElement element)
        {
            if (element.TryGetProperty("type", out JsonElement discriminator))
            {
                switch (discriminator.GetString())
                {
                    case "address": return Address.DeserializeAddress(element);
                    case "advisers": return Adviser.DeserializeAdviser(element);
                    case "contact-address": return ContactAddress.DeserializeContactAddress(element);
                    case "contact-address-details": return ContactAddressDetail.DeserializeContactAddressDetail(element);
                    case "contact-groups": return ContactGroup.DeserializeContactGroup(element);
                    case "contacts": return Contact.DeserializeContact(element);
                    case "deal-external-references": return DealExternalReference.DeserializeDealExternalReference(element);
                    case "deal-important-dates": return DealImportantDate.DeserializeDealImportantDate(element);
                    case "deal-notes": return DealNote.DeserializeDealNote(element);
                    case "deal-participants": return DealParticipant.DeserializeDealParticipant(element);
                    case "deal-scenario": return DealScenario.DeserializeDealScenario(element);
                    case "deal-structure-important-dates": return DealStructureImportantDate.DeserializeDealStructureImportantDate(element);
                    case "deal-structures": return DealStructure.DeserializeDealStructure(element);
                    case "deals": return Deal.DeserializeDeal(element);
                    case "high-level-summary": return HighLevelSummary.DeserializeHighLevelSummary(element);
                    case "integrations": return Integration.DeserializeIntegration(element);
                    case "contact-external-references": return ContactExternalReference.DeserializeContactExternalReference(element);
                }
            }
            string type = default;
            string id = default;
            foreach (var property in element.EnumerateObject())
            {
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
            return new IncludedResource(type, id);
        }
    }
}