// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class HighLevelSummaryRelationships : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(DealScenario))
            {
                writer.WritePropertyName("dealScenario");
                writer.WriteObjectValue(DealScenario);
            }
            writer.WriteEndObject();
        }

        internal static HighLevelSummaryRelationships DeserializeHighLevelSummaryRelationships(JsonElement element)
        {
            Optional<HighLevelSummaryRelationshipsDealScenario> dealScenario = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("dealScenario"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    dealScenario = HighLevelSummaryRelationshipsDealScenario.DeserializeHighLevelSummaryRelationshipsDealScenario(property.Value);
                    continue;
                }
            }
            return new HighLevelSummaryRelationships(dealScenario.Value);
        }
    }
}
