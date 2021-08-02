// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class ContactAttributes : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(Title))
            {
                if (Title != null)
                {
                    writer.WritePropertyName("title");
                    writer.WriteStringValue(Title);
                }
                else
                {
                    writer.WriteNull("title");
                }
            }
            if (Optional.IsDefined(FirstName))
            {
                if (FirstName != null)
                {
                    writer.WritePropertyName("firstName");
                    writer.WriteStringValue(FirstName);
                }
                else
                {
                    writer.WriteNull("firstName");
                }
            }
            if (Optional.IsDefined(MiddleName))
            {
                if (MiddleName != null)
                {
                    writer.WritePropertyName("middleName");
                    writer.WriteStringValue(MiddleName);
                }
                else
                {
                    writer.WriteNull("middleName");
                }
            }
            if (Optional.IsDefined(LastName))
            {
                if (LastName != null)
                {
                    writer.WritePropertyName("lastName");
                    writer.WriteStringValue(LastName);
                }
                else
                {
                    writer.WriteNull("lastName");
                }
            }
            if (Optional.IsDefined(PreferredName))
            {
                if (PreferredName != null)
                {
                    writer.WritePropertyName("preferredName");
                    writer.WriteStringValue(PreferredName);
                }
                else
                {
                    writer.WriteNull("preferredName");
                }
            }
            if (Optional.IsDefined(Mobile))
            {
                if (Mobile != null)
                {
                    writer.WritePropertyName("mobile");
                    writer.WriteStringValue(Mobile);
                }
                else
                {
                    writer.WriteNull("mobile");
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
            if (Optional.IsDefined(DateOfBirth))
            {
                if (DateOfBirth != null)
                {
                    writer.WritePropertyName("dateOfBirth");
                    writer.WriteStringValue(DateOfBirth.Value, "O");
                }
                else
                {
                    writer.WriteNull("dateOfBirth");
                }
            }
            if (Optional.IsDefined(HasMarketingConsent))
            {
                if (HasMarketingConsent != null)
                {
                    writer.WritePropertyName("hasMarketingConsent");
                    writer.WriteBooleanValue(HasMarketingConsent.Value);
                }
                else
                {
                    writer.WriteNull("hasMarketingConsent");
                }
            }
            writer.WriteEndObject();
        }

        internal static ContactAttributes DeserializeContactAttributes(JsonElement element)
        {
            Optional<string> title = default;
            Optional<string> firstName = default;
            Optional<string> middleName = default;
            Optional<string> lastName = default;
            Optional<string> preferredName = default;
            Optional<string> mobile = default;
            Optional<string> email = default;
            Optional<DateTimeOffset?> created = default;
            Optional<DateTimeOffset?> dateOfBirth = default;
            Optional<bool?> hasMarketingConsent = default;
            Optional<DateTimeOffset?> updated = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("title"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        title = null;
                        continue;
                    }
                    title = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("firstName"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        firstName = null;
                        continue;
                    }
                    firstName = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("middleName"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        middleName = null;
                        continue;
                    }
                    middleName = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("lastName"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        lastName = null;
                        continue;
                    }
                    lastName = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("preferredName"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        preferredName = null;
                        continue;
                    }
                    preferredName = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("mobile"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        mobile = null;
                        continue;
                    }
                    mobile = property.Value.GetString();
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
                if (property.NameEquals("dateOfBirth"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        dateOfBirth = null;
                        continue;
                    }
                    dateOfBirth = property.Value.GetDateTimeOffset("O");
                    continue;
                }
                if (property.NameEquals("hasMarketingConsent"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        hasMarketingConsent = null;
                        continue;
                    }
                    hasMarketingConsent = property.Value.GetBoolean();
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
            return new ContactAttributes(title.Value, firstName.Value, middleName.Value, lastName.Value, preferredName.Value, mobile.Value, email.Value, Optional.ToNullable(created), Optional.ToNullable(dateOfBirth), Optional.ToNullable(hasMarketingConsent), Optional.ToNullable(updated));
        }
    }
}