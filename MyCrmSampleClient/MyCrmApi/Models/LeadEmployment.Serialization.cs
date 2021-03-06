// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    public partial class LeadEmployment : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            if (Optional.IsDefined(Lid))
            {
                if (Lid != null)
                {
                    writer.WritePropertyName("lid");
                    writer.WriteStringValue(Lid);
                }
                else
                {
                    writer.WriteNull("lid");
                }
            }
            if (Optional.IsDefined(EmploymentStatus))
            {
                writer.WritePropertyName("employmentStatus");
                writer.WriteStringValue(EmploymentStatus.Value.ToString());
            }
            if (Optional.IsDefined(EmploymentType))
            {
                writer.WritePropertyName("employmentType");
                writer.WriteStringValue(EmploymentType.Value.ToString());
            }
            if (Optional.IsDefined(DateStartedAsString))
            {
                if (DateStartedAsString != null)
                {
                    writer.WritePropertyName("dateStartedAsString");
                    writer.WriteStringValue(DateStartedAsString.Value, "D");
                }
                else
                {
                    writer.WriteNull("dateStartedAsString");
                }
            }
            if (Optional.IsDefined(DateStarted))
            {
                if (DateStarted != null)
                {
                    writer.WritePropertyName("dateStarted");
                    writer.WriteStringValue(DateStarted.Value, "O");
                }
                else
                {
                    writer.WriteNull("dateStarted");
                }
            }
            if (Optional.IsDefined(DateEndedAsString))
            {
                if (DateEndedAsString != null)
                {
                    writer.WritePropertyName("dateEndedAsString");
                    writer.WriteStringValue(DateEndedAsString.Value, "D");
                }
                else
                {
                    writer.WriteNull("dateEndedAsString");
                }
            }
            if (Optional.IsDefined(DateEnded))
            {
                if (DateEnded != null)
                {
                    writer.WritePropertyName("dateEnded");
                    writer.WriteStringValue(DateEnded.Value, "O");
                }
                else
                {
                    writer.WriteNull("dateEnded");
                }
            }
            if (Optional.IsDefined(EmploymentRoleName))
            {
                if (EmploymentRoleName != null)
                {
                    writer.WritePropertyName("employmentRoleName");
                    writer.WriteStringValue(EmploymentRoleName);
                }
                else
                {
                    writer.WriteNull("employmentRoleName");
                }
            }
            if (Optional.IsDefined(EmploymentBasis))
            {
                writer.WritePropertyName("employmentBasis");
                writer.WriteStringValue(EmploymentBasis.Value.ToString());
            }
            if (Optional.IsDefined(IsProbation))
            {
                if (IsProbation != null)
                {
                    writer.WritePropertyName("isProbation");
                    writer.WriteBooleanValue(IsProbation.Value);
                }
                else
                {
                    writer.WriteNull("isProbation");
                }
            }
            if (Optional.IsDefined(BusinessNumber))
            {
                if (BusinessNumber != null)
                {
                    writer.WritePropertyName("businessNumber");
                    writer.WriteStringValue(BusinessNumber);
                }
                else
                {
                    writer.WriteNull("businessNumber");
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
            if (Optional.IsDefined(EmployerName))
            {
                if (EmployerName != null)
                {
                    writer.WritePropertyName("employerName");
                    writer.WriteStringValue(EmployerName);
                }
                else
                {
                    writer.WriteNull("employerName");
                }
            }
            if (Optional.IsDefined(EmployerContactTitle))
            {
                writer.WritePropertyName("employerContactTitle");
                writer.WriteStringValue(EmployerContactTitle.Value.ToString());
            }
            if (Optional.IsDefined(EmployerContactFirstName))
            {
                if (EmployerContactFirstName != null)
                {
                    writer.WritePropertyName("employerContactFirstName");
                    writer.WriteStringValue(EmployerContactFirstName);
                }
                else
                {
                    writer.WriteNull("employerContactFirstName");
                }
            }
            if (Optional.IsDefined(EmployerContactLastName))
            {
                if (EmployerContactLastName != null)
                {
                    writer.WritePropertyName("employerContactLastName");
                    writer.WriteStringValue(EmployerContactLastName);
                }
                else
                {
                    writer.WriteNull("employerContactLastName");
                }
            }
            if (Optional.IsDefined(EmployerType))
            {
                writer.WritePropertyName("employerType");
                writer.WriteStringValue(EmployerType.Value.ToString());
            }
            if (Optional.IsDefined(EmployerPhone))
            {
                if (EmployerPhone != null)
                {
                    writer.WritePropertyName("employerPhone");
                    writer.WriteStringValue(EmployerPhone);
                }
                else
                {
                    writer.WriteNull("employerPhone");
                }
            }
            if (Optional.IsDefined(Address))
            {
                writer.WritePropertyName("address");
                writer.WriteObjectValue(Address);
            }
            writer.WriteEndObject();
        }

        internal static LeadEmployment DeserializeLeadEmployment(JsonElement element)
        {
            Optional<string> lid = default;
            Optional<EmploymentStatus> employmentStatus = default;
            Optional<EmploymentType> employmentType = default;
            Optional<DateTimeOffset?> dateStartedAsString = default;
            Optional<DateTimeOffset?> dateStarted = default;
            Optional<DateTimeOffset?> dateEndedAsString = default;
            Optional<DateTimeOffset?> dateEnded = default;
            Optional<string> employmentRoleName = default;
            Optional<EmploymentBasis> employmentBasis = default;
            Optional<bool?> isProbation = default;
            Optional<string> businessNumber = default;
            Optional<string> companyNumber = default;
            Optional<string> employerName = default;
            Optional<Title> employerContactTitle = default;
            Optional<string> employerContactFirstName = default;
            Optional<string> employerContactLastName = default;
            Optional<EmployerType> employerType = default;
            Optional<string> employerPhone = default;
            Optional<LeadAddressReference> address = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("lid"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        lid = null;
                        continue;
                    }
                    lid = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("employmentStatus"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    employmentStatus = new EmploymentStatus(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("employmentType"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    employmentType = new EmploymentType(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("dateStartedAsString"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        dateStartedAsString = null;
                        continue;
                    }
                    dateStartedAsString = property.Value.GetDateTimeOffset("D");
                    continue;
                }
                if (property.NameEquals("dateStarted"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        dateStarted = null;
                        continue;
                    }
                    dateStarted = property.Value.GetDateTimeOffset("O");
                    continue;
                }
                if (property.NameEquals("dateEndedAsString"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        dateEndedAsString = null;
                        continue;
                    }
                    dateEndedAsString = property.Value.GetDateTimeOffset("D");
                    continue;
                }
                if (property.NameEquals("dateEnded"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        dateEnded = null;
                        continue;
                    }
                    dateEnded = property.Value.GetDateTimeOffset("O");
                    continue;
                }
                if (property.NameEquals("employmentRoleName"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        employmentRoleName = null;
                        continue;
                    }
                    employmentRoleName = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("employmentBasis"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    employmentBasis = new EmploymentBasis(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("isProbation"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        isProbation = null;
                        continue;
                    }
                    isProbation = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("businessNumber"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        businessNumber = null;
                        continue;
                    }
                    businessNumber = property.Value.GetString();
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
                if (property.NameEquals("employerName"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        employerName = null;
                        continue;
                    }
                    employerName = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("employerContactTitle"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    employerContactTitle = new Title(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("employerContactFirstName"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        employerContactFirstName = null;
                        continue;
                    }
                    employerContactFirstName = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("employerContactLastName"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        employerContactLastName = null;
                        continue;
                    }
                    employerContactLastName = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("employerType"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    employerType = new EmployerType(property.Value.GetString());
                    continue;
                }
                if (property.NameEquals("employerPhone"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        employerPhone = null;
                        continue;
                    }
                    employerPhone = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("address"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    address = LeadAddressReference.DeserializeLeadAddressReference(property.Value);
                    continue;
                }
            }
            return new LeadEmployment(lid.Value, Optional.ToNullable(employmentStatus), Optional.ToNullable(employmentType), Optional.ToNullable(dateStartedAsString), Optional.ToNullable(dateStarted), Optional.ToNullable(dateEndedAsString), Optional.ToNullable(dateEnded), employmentRoleName.Value, Optional.ToNullable(employmentBasis), Optional.ToNullable(isProbation), businessNumber.Value, companyNumber.Value, employerName.Value, Optional.ToNullable(employerContactTitle), employerContactFirstName.Value, employerContactLastName.Value, Optional.ToNullable(employerType), employerPhone.Value, address.Value);
        }
    }
}
