using System;
using System.Collections.Generic;
using System.Linq;

namespace MyCrmSampleClient.Console;

public sealed class SampleScopes
{
    private readonly HashSet<string> _scopes;
    
    public string RawScopes { get; }

    public SampleScopes(string scopes)
    {
        RawScopes = scopes;
        _scopes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var tokens = (scopes ?? string.Empty)
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim());

        foreach (var token in tokens)
        {
            _scopes.Add(token);
        }
    }

    /// <summary>
    /// Tests if the credentials have the necessary access for the process to work.
    /// </summary>
    /// <param name="requiredScope">The specific scope to test, e.g. api.contacts.read</param>
    /// <returns>True if the scopes meet the requirement to run the process</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="requiredScope"/> is null, empty, or not a three-part value (e.g. 'api.contacts.read').</exception>
    public bool HasScope(string requiredScope)
    {
        if (string.IsNullOrWhiteSpace(requiredScope))
        {
            throw new ArgumentException("Required scope cannot be empty.", nameof(requiredScope));
        }

        var parts = requiredScope.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length != 3)
        {
            throw new ArgumentException("Required scope must be a three-part value, e.g. 'api.contacts.read'.", nameof(requiredScope));
        }

        var part0 = parts[0];
        var part1 = parts[1];
        var part2 = parts[2];

        return _scopes.Contains(requiredScope) ||
               _scopes.Contains(part0) ||
               _scopes.Contains($"{part0}.{part1}") ||
               _scopes.Contains($"{part0}.{part2}");
    }
}
