// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class BusinessAttributes : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(Name))
            {
                if (Name != null)
                {
                    writer.WritePropertyName("name");
                    writer.WriteStringValue(Name);
                }
                else
                {
                    writer.WriteNull("name");
                }
            }
            if (Optional.IsDefined(Phone))
            {
                if (Phone != null)
                {
                    writer.WritePropertyName("phone");
                    writer.WriteStringValue(Phone);
                }
                else
                {
                    writer.WriteNull("phone");
                }
            }
            if (Optional.IsDefined(Email))
            {
                if (Email != null)
                {
                    writer.WritePropertyName("email");
                    writer.WriteStringValue(Email);
                }
                else
                {
                    writer.WriteNull("email");
                }
            }
            if (Optional.IsDefined(TaxNumber))
            {
                if (TaxNumber != null)
                {
                    writer.WritePropertyName("taxNumber");
                    writer.WriteStringValue(TaxNumber);
                }
                else
                {
                    writer.WriteNull("taxNumber");
                }
            }
            if (Optional.IsDefined(CompanyNumber))
            {
                if (CompanyNumber != null)
                {
                    writer.WritePropertyName("companyNumber");
                    writer.WriteStringValue(CompanyNumber);
                }
                else
                {
                    writer.WriteNull("companyNumber");
                }
            }
            if (Optional.IsDefined(Abn))
            {
                if (Abn != null)
                {
                    writer.WritePropertyName("abn");
                    writer.WriteStringValue(Abn);
                }
                else
                {
                    writer.WriteNull("abn");
                }
            }
            if (Optional.IsDefined(Acn))
            {
                if (Acn != null)
                {
                    writer.WritePropertyName("acn");
                    writer.WriteStringValue(Acn);
                }
                else
                {
                    writer.WriteNull("acn");
                }
            }
            if (Optional.IsDefined(Notes))
            {
                if (Notes != null)
                {
                    writer.WritePropertyName("notes");
                    writer.WriteStringValue(Notes);
                }
                else
                {
                    writer.WriteNull("notes");
                }
            }
            if (Optional.IsDefined(Description))
            {
                if (Description != null)
                {
                    writer.WritePropertyName("description");
                    writer.WriteStringValue(Description);
                }
                else
                {
                    writer.WriteNull("description");
                }
            }
            writer.WriteEndObject();
        }

        internal static BusinessAttributes DeserializeBusinessAttributes(JsonElement element)
        {
            Optional<DateTimeOffset?> created = default;
            Optional<string> name = default;
            Optional<string> phone = default;
            Optional<string> email = default;
            Optional<string> taxNumber = default;
            Optional<string> companyNumber = default;
            Optional<string> abn = default;
            Optional<string> acn = default;
            Optional<string> notes = default;
            Optional<string> description = default;
            Optional<string> businessType = default;
            Optional<DateTimeOffset?> updated = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("created"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        created = null;
                        continue;
                    }
                    created = property.Value.GetDateTimeOffset("O");
                    continue;
                }
                if (property.NameEquals("name"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        name = null;
                        continue;
                    }
                    name = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("phone"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        phone = null;
                        continue;
                    }
                    phone = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("email"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        email = null;
                        continue;
                    }
                    email = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("taxNumber"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        taxNumber = null;
                        continue;
                    }
                    taxNumber = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("companyNumber"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        companyNumber = null;
                        continue;
                    }
                    companyNumber = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("abn"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        abn = null;
                        continue;
                    }
                    abn = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("acn"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        acn = null;
                        continue;
                    }
                    acn = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("notes"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        notes = null;
                        continue;
                    }
                    notes = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("description"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        description = null;
                        continue;
                    }
                    description = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("businessType"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        businessType = null;
                        continue;
                    }
                    businessType = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("updated"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        updated = null;
                        continue;
                    }
                    updated = property.Value.GetDateTimeOffset("O");
                    continue;
                }
            }
            return new BusinessAttributes(Optional.ToNullable(created), name.Value, phone.Value, email.Value, taxNumber.Value, companyNumber.Value, abn.Value, acn.Value, notes.Value, description.Value, businessType.Value, Optional.ToNullable(updated));
        }
    }
}