// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class OrganisationAttributes : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteEndObject();
        }

        internal static OrganisationAttributes DeserializeOrganisationAttributes(JsonElement element)
        {
            Optional<string> emailForCommissions = default;
            Optional<string> primaryBrandColour = default;
            Optional<string> name = default;
            Optional<string> tradingName = default;
            Optional<string> website = default;
            Optional<string> companyEmail = default;
            Optional<string> brandedCategory = default;
            Optional<string> status = default;
            Optional<string> slug = default;
            Optional<string> abn = default;
            Optional<string> acn = default;
            Optional<string> officeDisplayName = default;
            Optional<string> phone = default;
            Optional<string> fax = default;
            Optional<string> brandLogoUrl = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("emailForCommissions"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        emailForCommissions = null;
                        continue;
                    }
                    emailForCommissions = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("primaryBrandColour"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        primaryBrandColour = null;
                        continue;
                    }
                    primaryBrandColour = property.Value.GetString();
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
                if (property.NameEquals("tradingName"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        tradingName = null;
                        continue;
                    }
                    tradingName = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("website"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        website = null;
                        continue;
                    }
                    website = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("companyEmail"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        companyEmail = null;
                        continue;
                    }
                    companyEmail = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("brandedCategory"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        brandedCategory = null;
                        continue;
                    }
                    brandedCategory = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("status"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        status = null;
                        continue;
                    }
                    status = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("slug"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        slug = null;
                        continue;
                    }
                    slug = property.Value.GetString();
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
                if (property.NameEquals("officeDisplayName"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        officeDisplayName = null;
                        continue;
                    }
                    officeDisplayName = property.Value.GetString();
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
                if (property.NameEquals("fax"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        fax = null;
                        continue;
                    }
                    fax = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("brandLogoUrl"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        brandLogoUrl = null;
                        continue;
                    }
                    brandLogoUrl = property.Value.GetString();
                    continue;
                }
            }
            return new OrganisationAttributes(emailForCommissions.Value, primaryBrandColour.Value, name.Value, tradingName.Value, website.Value, companyEmail.Value, brandedCategory.Value, status.Value, slug.Value, abn.Value, acn.Value, officeDisplayName.Value, phone.Value, fax.Value, brandLogoUrl.Value);
        }
    }
}
