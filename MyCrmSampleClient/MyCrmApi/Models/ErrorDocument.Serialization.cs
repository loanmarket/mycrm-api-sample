// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System.Collections.Generic;
using System.Text.Json;
using Azure.Core;

namespace MyCrmSampleClient.MyCrmApi.Models
{
    internal partial class ErrorDocument
    {
        internal static ErrorDocument DeserializeErrorDocument(JsonElement element)
        {
            Optional<TopLevelLinks> links = default;
            Optional<IReadOnlyList<ErrorObject>> errors = default;
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("links"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        property.ThrowNonNullablePropertyIsNull();
                        continue;
                    }
                    links = TopLevelLinks.DeserializeTopLevelLinks(property.Value);
                    continue;
                }
                if (property.NameEquals("errors"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        errors = null;
                        continue;
                    }
                    List<ErrorObject> array = new List<ErrorObject>();
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        array.Add(ErrorObject.DeserializeErrorObject(item));
                    }
                    errors = array;
                    continue;
                }
            }
            return new ErrorDocument(links.Value, Optional.ToList(errors));
        }
    }
}
