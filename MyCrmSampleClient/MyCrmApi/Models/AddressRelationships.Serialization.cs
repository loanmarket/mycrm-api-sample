// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class AddressRelationships : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(AddressDetails))
            {
                writer.WritePropertyName("addressDetails");
                writer.WriteObjectValue(AddressDetails);
            }
            writer.WriteEndObject();
        }

        internal static AddressRelationships DeserializeAddressRelationships(JsonElement element)
        {
            Optional<RelationshipsMultipleDocument> addressDetails = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("addressDetails"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    addressDetails = RelationshipsMultipleDocument.DeserializeRelationshipsMultipleDocument(property.Value);
                    continue;
                }
            }
            return new AddressRelationships(addressDetails.Value);
        }
    }
}