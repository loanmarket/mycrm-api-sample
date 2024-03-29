// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class OwnerRelationships : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(Contact))
            {
                writer.WritePropertyName("contact");
                writer.WriteObjectValue(Contact);
            }
            if (Optional.IsDefined(Business))
            {
                writer.WritePropertyName("business");
                writer.WriteObjectValue(Business);
            }
            if (Optional.IsDefined(Asset))
            {
                writer.WritePropertyName("asset");
                writer.WriteObjectValue(Asset);
            }
            if (Optional.IsDefined(Expense))
            {
                writer.WritePropertyName("expense");
                writer.WriteObjectValue(Expense);
            }
            if (Optional.IsDefined(Liability))
            {
                writer.WritePropertyName("liability");
                writer.WriteObjectValue(Liability);
            }
            if (Optional.IsDefined(Income))
            {
                writer.WritePropertyName("income");
                writer.WriteObjectValue(Income);
            }
            writer.WriteEndObject();
        }

        internal static OwnerRelationships DeserializeOwnerRelationships(JsonElement element)
        {
            Optional<RelationshipsSingleDocument> contact = default;
            Optional<RelationshipsSingleDocument> business = default;
            Optional<OwnerRelationshipsAsset> asset = default;
            Optional<OwnerRelationshipsExpense> expense = default;
            Optional<OwnerRelationshipsLiability> liability = default;
            Optional<OwnerRelationshipsIncome> income = default;
            foreach (var property in element.EnumerateObject())
            {
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
                if (property.NameEquals("asset"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    asset = OwnerRelationshipsAsset.DeserializeOwnerRelationshipsAsset(property.Value);
                    continue;
                }
                if (property.NameEquals("expense"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    expense = OwnerRelationshipsExpense.DeserializeOwnerRelationshipsExpense(property.Value);
                    continue;
                }
                if (property.NameEquals("liability"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    liability = OwnerRelationshipsLiability.DeserializeOwnerRelationshipsLiability(property.Value);
                    continue;
                }
                if (property.NameEquals("income"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    income = OwnerRelationshipsIncome.DeserializeOwnerRelationshipsIncome(property.Value);
                    continue;
                }
            }
            return new OwnerRelationships(contact.Value, business.Value, asset.Value, expense.Value, liability.Value, income.Value);
        }
    }
}
