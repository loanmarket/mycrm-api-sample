// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class ContactAddressRelationships : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(AddressDetails))
            {
                writer.WritePropertyName("addressDetails");
                writer.WriteObjectValue(AddressDetails);
            }
            if (Optional.IsDefined(Contact))
            {
                writer.WritePropertyName("contact");
                writer.WriteObjectValue(Contact);
            }
            writer.WriteEndObject();
        }

        internal static ContactAddressRelationships DeserializeContactAddressRelationships(JsonElement element)
        {
            Optional<RelationshipsSingleDocument> addressDetails = default;
            Optional<RelationshipsSingleDocument> contact = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("addressDetails"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    addressDetails = RelationshipsSingleDocument.DeserializeRelationshipsSingleDocument(property.Value);
                    continue;
                }
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
            }
            return new ContactAddressRelationships(addressDetails.Value, contact.Value);
        }
    }
}