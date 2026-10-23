using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Kiota.Abstractions;

namespace MyCrmSampleClient.KiotaExtensions;

public sealed record RateLimitRetry(string Request, int Retry, int MaximumRetries, TimeSpan Delay);

/// <summary>Retries only explicit HTTP 429 responses; never replays an uncertain write.</summary>
public sealed class RateLimitBackoff(
    Action<RateLimitRetry> report = null,
    Func<TimeSpan, CancellationToken, Task> delay = null,
    TimeProvider clock = null)
{
    public const int MaximumRetries = 8;
    private readonly Func<TimeSpan, CancellationToken, Task> _delay = delay ?? Task.Delay;
    private readonly TimeProvider _clock = clock ?? TimeProvider.System;

    public async Task<T> ExecuteAsync<T>(Func<Task<T>> send, string request, CancellationToken cancellationToken = default)
    {
        for (var retry = 0; ; retry++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            try
            {
                return await send();
            }
            catch (ApiException error) when (error.ResponseStatusCode == 429 && retry < MaximumRetries)
            {
                // 2, 4, 8, 16, 32, then 60 seconds, with up to a second of jitter.
                // A longer server-requested delay is always respected, including HTTP dates.
                var wait = TimeSpan.FromSeconds(Math.Min(60, Math.Pow(2, retry + 1) + Random.Shared.NextDouble()));
                if (RetryAfter(error) is { } serverWait && serverWait > wait) wait = serverWait;

                report?.Invoke(new RateLimitRetry(request, retry + 1, MaximumRetries, wait));
                await WaitAsync(wait, cancellationToken);
            }
        }
    }

    private TimeSpan? RetryAfter(ApiException error)
    {
        var values = error.ResponseHeaders?.Where(header => header.Key.Equals("Retry-After", StringComparison.OrdinalIgnoreCase))
            .SelectMany(header => header.Value) ?? [];
        TimeSpan? longest = null;

        foreach (var value in values)
        {
            if (!RetryConditionHeaderValue.TryParse(value, out var header)) continue;

            var wait = header.Delta ?? header.Date - _clock.GetUtcNow();
            if (wait > TimeSpan.Zero && (!longest.HasValue || wait > longest)) longest = wait;
        }

        return longest;
    }

    private async Task WaitAsync(TimeSpan remaining, CancellationToken cancellationToken)
    {
        // Split long server delays to stay within Task.Delay's supported range.
        while (remaining > TimeSpan.Zero)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var step = remaining > TimeSpan.FromSeconds(60) ? TimeSpan.FromSeconds(60) : remaining;
            await _delay(step, cancellationToken);
            remaining -= step;
        }
    }
}
