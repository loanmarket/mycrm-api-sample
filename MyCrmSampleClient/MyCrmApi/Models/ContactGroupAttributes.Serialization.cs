// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class ContactGroupAttributes : IUtf8JsonSerializable
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
            if (Optional.IsDefined(UtmSource))
            {
                if (UtmSource != null)
                {
                    writer.WritePropertyName("utmSource");
                    writer.WriteStringValue(UtmSource);
                }
                else
                {
                    writer.WriteNull("utmSource");
                }
            }
            if (Optional.IsDefined(UtmMedium))
            {
                if (UtmMedium != null)
                {
                    writer.WritePropertyName("utmMedium");
                    writer.WriteStringValue(UtmMedium);
                }
                else
                {
                    writer.WriteNull("utmMedium");
                }
            }
            if (Optional.IsDefined(UtmTerm))
            {
                if (UtmTerm != null)
                {
                    writer.WritePropertyName("utmTerm");
                    writer.WriteStringValue(UtmTerm);
                }
                else
                {
                    writer.WriteNull("utmTerm");
                }
            }
            if (Optional.IsDefined(UtmContent))
            {
                if (UtmContent != null)
                {
                    writer.WritePropertyName("utmContent");
                    writer.WriteStringValue(UtmContent);
                }
                else
                {
                    writer.WriteNull("utmContent");
                }
            }
            if (Optional.IsDefined(UtmCampaign))
            {
                if (UtmCampaign != null)
                {
                    writer.WritePropertyName("utmCampaign");
                    writer.WriteStringValue(UtmCampaign);
                }
                else
                {
                    writer.WriteNull("utmCampaign");
                }
            }
            writer.WriteEndObject();
        }

        internal static ContactGroupAttributes DeserializeContactGroupAttributes(JsonElement element)
        {
            Optional<string> name = default;
            Optional<DateTimeOffset?> created = default;
            Optional<DateTimeOffset?> updated = default;
            Optional<string> utmSource = default;
            Optional<string> utmMedium = default;
            Optional<string> utmTerm = default;
            Optional<string> utmContent = default;
            Optional<string> utmCampaign = default;
            foreach (var property in element.EnumerateObject())
            {
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
                if (property.NameEquals("utmSource"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        utmSource = null;
                        continue;
                    }
                    utmSource = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("utmMedium"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        utmMedium = null;
                        continue;
                    }
                    utmMedium = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("utmTerm"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        utmTerm = null;
                        continue;
                    }
                    utmTerm = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("utmContent"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        utmContent = null;
                        continue;
                    }
                    utmContent = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("utmCampaign"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        utmCampaign = null;
                        continue;
                    }
                    utmCampaign = property.Value.GetString();
                    continue;
                }
            }
            return new ContactGroupAttributes(name.Value, Optional.ToNullable(created), Optional.ToNullable(updated), utmSource.Value, utmMedium.Value, utmTerm.Value, utmContent.Value, utmCampaign.Value);
        }
    }
}
