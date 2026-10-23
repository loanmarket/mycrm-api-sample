using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;

namespace MyCrmSampleClient.Auth;

internal sealed class MyCrmKiotaAuthProvider : IAuthenticationProvider, IDisposable
{
    private AuthResult _token;
    private readonly int _adviserContactId;
    private readonly Func<CancellationToken, Task<AuthResult>> _acquireToken;
    private readonly TimeProvider _clock;
    private readonly SemaphoreSlim _tokenLock = new(1, 1);

    public MyCrmKiotaAuthProvider(AuthResult token, int adviserContactId,
        Func<CancellationToken, Task<AuthResult>> acquireToken, TimeProvider clock = null)
    {
        _token = token ?? throw new ArgumentNullException(nameof(token));
        _adviserContactId = adviserContactId;
        _acquireToken = acquireToken ?? throw new ArgumentNullException(nameof(acquireToken));
        _clock = clock ?? TimeProvider.System;
    }

    public async Task AuthenticateRequestAsync(
        RequestInformation request,
        Dictionary<string, object> additionalAuthenticationContext = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        await _tokenLock.WaitAsync(cancellationToken);
        try
        {
            // Renew before sending, including after a long pause at an import prompt.
            // Never replay a snapshot POST in response to an authentication failure.
            if (!_token.Success || string.IsNullOrWhiteSpace(_token.Token) ||
                _token.ExpiresAt.ToUniversalTime() <= _clock.GetUtcNow().UtcDateTime.AddMinutes(1))
            {
                var renewed = await _acquireToken(cancellationToken);
                if (renewed == null || !renewed.Success || string.IsNullOrWhiteSpace(renewed.Token) ||
                    renewed.ExpiresAt.ToUniversalTime() <= _clock.GetUtcNow().UtcDateTime)
                    throw new InvalidOperationException("Could not renew API authentication. The API request was not sent. Check the configured credentials.");

                _token = renewed;
            }

            request.Headers.Remove("Authorization");
            request.Headers.Add("Authorization", $"Bearer {_token.Token}");
        }
        finally
        {
            _tokenLock.Release();
        }

        if (_adviserContactId > 0)
        {
            request.Headers.TryAdd("UserId", _adviserContactId.ToString());
        }
    }

    public void Dispose() => _tokenLock.Dispose();
}
