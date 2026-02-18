using System;

namespace MyCrmSampleClient.Console;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class SampleAttribute : Attribute
{
    public SampleAttribute(string name, string apiMethod)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Sample name cannot be empty", nameof(name));
        if (string.IsNullOrWhiteSpace(apiMethod)) throw new ArgumentException("Sample API method cannot be empty", nameof(apiMethod));

        Name = name;
        ApiMethod = apiMethod;
    }
    public string Name { get; }
    public string ApiMethod { get; }
}