using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

namespace MyCrmSampleClient.Auth;

internal sealed class MyCrmKiotaAuthProvider : IAuthenticationProvider
{
    private readonly string _accessToken;
    private readonly int _adviserContactId;

    public MyCrmKiotaAuthProvider(string accessToken, int adviserContactId)
    {
        _accessToken = accessToken ?? throw new ArgumentNullException(nameof(accessToken));
        _adviserContactId = adviserContactId;
    }

    public Task AuthenticateRequestAsync(
        RequestInformation request,
        Dictionary<string, object> additionalAuthenticationContext = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        request.Headers.TryAdd("Authorization", $"Bearer {_accessToken}");

        if (_adviserContactId > 0)
        {
            request.Headers.TryAdd("UserId", _adviserContactId.ToString());
        }

        return Task.CompletedTask;
    }
}
