// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class LeadAttributes : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(DateOfBirth))
            {
                if (DateOfBirth != null)
                {
                    writer.WritePropertyName("dateOfBirth");
                    writer.WriteStringValue(DateOfBirth.Value, "D");
                }
                else
                {
                    writer.WriteNull("dateOfBirth");
                }
            }
            if (Optional.IsDefined(Title))
            {
                writer.WritePropertyName("title");
                writer.WriteStringValue(Title.Value.ToString());
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
            if (Optional.IsDefined(Gender))
            {
                writer.WritePropertyName("gender");
                writer.WriteStringValue(Gender.Value.ToString());
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
            if (Optional.IsDefined(IsGuarantor))
            {
                if (IsGuarantor != null)
                {
                    writer.WritePropertyName("isGuarantor");
                    writer.WriteBooleanValue(IsGuarantor.Value);
                }
                else
                {
                    writer.WriteNull("isGuarantor");
                }
            }
            if (Optional.IsDefined(IsDependant))
            {
                if (IsDependant != null)
                {
                    writer.WritePropertyName("isDependant");
                    writer.WriteBooleanValue(IsDependant.Value);
                }
                else
                {
                    writer.WriteNull("isDependant");
                }
            }
            if (Optional.IsDefined(IsPrimary))
            {
                if (IsPrimary != null)
                {
                    writer.WritePropertyName("isPrimary");
                    writer.WriteBooleanValue(IsPrimary.Value);
                }
                else
                {
                    writer.WriteNull("isPrimary");
                }
            }
            if (Optional.IsDefined(StreetAddress))
            {
                if (StreetAddress != null)
                {
                    writer.WritePropertyName("streetAddress");
                    writer.WriteStringValue(StreetAddress);
                }
                else
                {
                    writer.WriteNull("streetAddress");
                }
            }
            if (Optional.IsDefined(Suburb))
            {
                if (Suburb != null)
                {
                    writer.WritePropertyName("suburb");
                    writer.WriteStringValue(Suburb);
                }
                else
                {
                    writer.WriteNull("suburb");
                }
            }
            if (Optional.IsDefined(State))
            {
                if (State != null)
                {
                    writer.WritePropertyName("state");
                    writer.WriteStringValue(State);
                }
                else
                {
                    writer.WriteNull("state");
                }
            }
            if (Optional.IsDefined(PostCode))
            {
                if (PostCode != null)
                {
                    writer.WritePropertyName("postCode");
                    writer.WriteStringValue(PostCode);
                }
                else
                {
                    writer.WriteNull("postCode");
                }
            }
            if (Optional.IsDefined(Country))
            {
                if (Country != null)
                {
                    writer.WritePropertyName("country");
                    writer.WriteStringValue(Country);
                }
                else
                {
                    writer.WriteNull("country");
                }
            }
            if (Optional.IsDefined(AddressType))
            {
                writer.WritePropertyName("addressType");
                writer.WriteStringValue(AddressType.Value.ToString());
            }
            if (Optional.IsDefined(FormattedAddress))
            {
                if (FormattedAddress != null)
                {
                    writer.WritePropertyName("formattedAddress");
                    writer.WriteStringValue(FormattedAddress);
                }
                else
                {
                    writer.WriteNull("formattedAddress");
                }
            }
            if (Optional.IsDefined(NoteTitle))
            {
                if (NoteTitle != null)
                {
                    writer.WritePropertyName("noteTitle");
                    writer.WriteStringValue(NoteTitle);
                }
                else
                {
                    writer.WriteNull("noteTitle");
                }
            }
            if (Optional.IsDefined(NoteDetails))
            {
                if (NoteDetails != null)
                {
                    writer.WritePropertyName("noteDetails");
                    writer.WriteStringValue(NoteDetails);
                }
                else
                {
                    writer.WriteNull("noteDetails");
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
            if (Optional.IsDefined(SourceSystemUrl))
            {
                if (SourceSystemUrl != null)
                {
                    writer.WritePropertyName("sourceSystemUrl");
                    writer.WriteStringValue(SourceSystemUrl);
                }
                else
                {
                    writer.WriteNull("sourceSystemUrl");
                }
            }
            if (Optional.IsDefined(SourceId))
            {
                if (SourceId != null)
                {
                    writer.WritePropertyName("sourceId");
                    writer.WriteNumberValue(SourceId.Value);
                }
                else
                {
                    writer.WriteNull("sourceId");
                }
            }
            if (Optional.IsDefined(SourceCategoryId))
            {
                if (SourceCategoryId != null)
                {
                    writer.WritePropertyName("sourceCategoryId");
                    writer.WriteNumberValue(SourceCategoryId.Value);
                }
                else
                {
                    writer.WriteNull("sourceCategoryId");
                }
            }
            if (Optional.IsDefined(SourceAdditionalDetails))
            {
                if (SourceAdditionalDetails != null)
                {
                    writer.WritePropertyName("sourceAdditionalDetails");
                    writer.WriteStringValue(SourceAdditionalDetails);
                }
                else
                {
                    writer.WriteNull("sourceAdditionalDetails");
                }
            }
            writer.WriteEndObject();
        }

        internal static LeadAttributes DeserializeLeadAttributes(JsonElement element)
        {
            Optional<DateTimeOffset?> dateOfBirth = default;
            Optional<LeadAttributesTitle> title = default;
            Optional<string> firstName = default;
            Optional<string> preferredName = default;
            Optional<string> lastName = default;
            Optional<string> email = default;
            Optional<string> mobile = default;
            Optional<LeadAttributesGender> gender = default;
            Optional<bool?> hasMarketingConsent = default;
            Optional<bool?> isGuarantor = default;
            Optional<bool?> isDependant = default;
            Optional<bool?> isPrimary = default;
            Optional<string> streetAddress = default;
            Optional<string> suburb = default;
            Optional<string> state = default;
            Optional<string> postCode = default;
            Optional<string> country = default;
            Optional<LeadAttributesAddressType> addressType = default;
            Optional<string> formattedAddress = default;
            Optional<string> noteTitle = default;
            Optional<string> noteDetails = default;
            Optional<string> utmSource = default;
            Optional<string> utmMedium = default;
            Optional<string> utmCampaign = default;
            Optional<string> utmTerm = default;
            Optional<string> utmContent = default;
            Optional<string> sourceSystemUrl = default;
            Optional<int?> sourceId = default;
            Optional<int?> sourceCategoryId = default;
            Optional<string> sourceAdditionalDetails = default;
            Optional<string> customStatusName = default;
            Optional<LeadAttributesDealStatus> dealStatus = default;
            Optional<bool> sendNotification = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("dateOfBirth"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        dateOfBirth = null;
                        continue;
                    }
                    dateOfBirth = property.Value.GetDateTimeOffset("D");
                    continue;
                }
                if (property.NameEquals("title"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    title = new LeadAttributesTitle(property.Value.GetString());
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
                if (property.NameEquals("gender"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    gender = new LeadAttributesGender(property.Value.GetString());
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
                if (property.NameEquals("isGuarantor"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        isGuarantor = null;
                        continue;
                    }
                    isGuarantor = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("isDependant"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        isDependant = null;
                        continue;
                    }
                    isDependant = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("isPrimary"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        isPrimary = null;
                        continue;
                    }
                    isPrimary = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("streetAddress"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        streetAddress = null;
                        continue;
                    }
                    streetAddress = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("suburb"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        suburb = null;
                        continue;
                    }
                    suburb = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("state"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        state = null;
                        continue;
                    }
                    state = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("postCode"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        postCode = null;
                        continue;
                    }
                    postCode = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("country"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        country = null;
                        continue;
                    }
                    country = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("addressType"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    addressType = new LeadAttributesAddressType(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("formattedAddress"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        formattedAddress = null;
                        continue;
                    }
                    formattedAddress = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("noteTitle"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        noteTitle = null;
                        continue;
                    }
                    noteTitle = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("noteDetails"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        noteDetails = null;
                        continue;
                    }
                    noteDetails = property.Value.GetString();
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
                if (property.NameEquals("sourceSystemUrl"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        sourceSystemUrl = null;
                        continue;
                    }
                    sourceSystemUrl = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("sourceId"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        sourceId = null;
                        continue;
                    }
                    sourceId = property.Value.GetInt32();
                    continue;
                }
                if (property.NameEquals("sourceCategoryId"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        sourceCategoryId = null;
                        continue;
                    }
                    sourceCategoryId = property.Value.GetInt32();
                    continue;
                }
                if (property.NameEquals("sourceAdditionalDetails"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        sourceAdditionalDetails = null;
                        continue;
                    }
                    sourceAdditionalDetails = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("customStatusName"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        customStatusName = null;
                        continue;
                    }
                    customStatusName = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("dealStatus"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    dealStatus = new LeadAttributesDealStatus(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("sendNotification"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    sendNotification = property.Value.GetBoolean();
                    continue;
                }
            }
            return new LeadAttributes(Optional.ToNullable(dateOfBirth), Optional.ToNullable(title), firstName.Value, preferredName.Value, lastName.Value, email.Value, mobile.Value, Optional.ToNullable(gender), Optional.ToNullable(hasMarketingConsent), Optional.ToNullable(isGuarantor), Optional.ToNullable(isDependant), Optional.ToNullable(isPrimary), streetAddress.Value, suburb.Value, state.Value, postCode.Value, country.Value, Optional.ToNullable(addressType), formattedAddress.Value, noteTitle.Value, noteDetails.Value, utmSource.Value, utmMedium.Value, utmCampaign.Value, utmTerm.Value, utmContent.Value, sourceSystemUrl.Value, Optional.ToNullable(sourceId), Optional.ToNullable(sourceCategoryId), sourceAdditionalDetails.Value, customStatusName.Value, Optional.ToNullable(dealStatus), Optional.ToNullable(sendNotification));
        }
    }
}
