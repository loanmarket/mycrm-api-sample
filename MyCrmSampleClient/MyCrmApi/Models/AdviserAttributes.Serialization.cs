// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class AdviserAttributes : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WriteEndObject();
        }

        internal static AdviserAttributes DeserializeAdviserAttributes(JsonElement element)
        {
            Optional<string> googlePlaces = default;
            Optional<DateTimeOffset?> created = default;
            Optional<bool> isBrokerWebPublic = default;
            Optional<DateTimeOffset?> myLeadGenActivationDate = default;
            Optional<bool?> isMyLeadGenActive = default;
            Optional<string> bio = default;
            Optional<string> jobTitle = default;
            Optional<string> website = default;
            Optional<string> status = default;
            Optional<string> email = default;
            Optional<string> skype = default;
            Optional<string> facebook = default;
            Optional<string> linkedIn = default;
            Optional<string> twitter = default;
            Optional<string> youtubeFeatured = default;
            Optional<string> instagram = default;
            Optional<string> calendly = default;
            Optional<string> myLeadGenerator = default;
            Optional<string> profilePhotoHeadShot = default;
            Optional<string> profilePhotoHalfBody = default;
            Optional<string> profilePhotoFullBody = default;
            Optional<AdviserAttributesCountryCode> countryCode = default;
            Optional<string> youtubeChannel = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("googlePlaces"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        googlePlaces = null;
                        continue;
                    }
                    googlePlaces = property.Value.GetString();
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
                if (property.NameEquals("isBrokerWebPublic"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    isBrokerWebPublic = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("myLeadGenActivationDate"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        myLeadGenActivationDate = null;
                        continue;
                    }
                    myLeadGenActivationDate = property.Value.GetDateTimeOffset("O");
                    continue;
                }
                if (property.NameEquals("isMyLeadGenActive"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        isMyLeadGenActive = null;
                        continue;
                    }
                    isMyLeadGenActive = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("bio"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        bio = null;
                        continue;
                    }
                    bio = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("jobTitle"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        jobTitle = null;
                        continue;
                    }
                    jobTitle = property.Value.GetString();
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
                if (property.NameEquals("skype"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        skype = null;
                        continue;
                    }
                    skype = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("facebook"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        facebook = null;
                        continue;
                    }
                    facebook = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("linkedIn"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        linkedIn = null;
                        continue;
                    }
                    linkedIn = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("twitter"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        twitter = null;
                        continue;
                    }
                    twitter = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("youtubeFeatured"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        youtubeFeatured = null;
                        continue;
                    }
                    youtubeFeatured = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("instagram"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        instagram = null;
                        continue;
                    }
                    instagram = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("calendly"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        calendly = null;
                        continue;
                    }
                    calendly = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("myLeadGenerator"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        myLeadGenerator = null;
                        continue;
                    }
                    myLeadGenerator = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("profilePhotoHeadShot"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        profilePhotoHeadShot = null;
                        continue;
                    }
                    profilePhotoHeadShot = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("profilePhotoHalfBody"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        profilePhotoHalfBody = null;
                        continue;
                    }
                    profilePhotoHalfBody = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("profilePhotoFullBody"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        profilePhotoFullBody = null;
                        continue;
                    }
                    profilePhotoFullBody = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("countryCode"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    countryCode = new AdviserAttributesCountryCode(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("youtubeChannel"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        youtubeChannel = null;
                        continue;
                    }
                    youtubeChannel = property.Value.GetString();
                    continue;
                }
            }
            return new AdviserAttributes(googlePlaces.Value, Optional.ToNullable(created), Optional.ToNullable(isBrokerWebPublic), Optional.ToNullable(myLeadGenActivationDate), Optional.ToNullable(isMyLeadGenActive), bio.Value, jobTitle.Value, website.Value, status.Value, email.Value, skype.Value, facebook.Value, linkedIn.Value, twitter.Value, youtubeFeatured.Value, instagram.Value, calendly.Value, myLeadGenerator.Value, profilePhotoHeadShot.Value, profilePhotoHalfBody.Value, profilePhotoFullBody.Value, Optional.ToNullable(countryCode), youtubeChannel.Value);
        }
    }
}
