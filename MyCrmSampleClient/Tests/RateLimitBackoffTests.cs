using Microsoft.Kiota.Abstractions;
using MyCrmSampleClient.KiotaExtensions;
using Xunit;

namespace MyCrmSampleClient.Tests;

public sealed class RateLimitBackoffTests
{
    private const string Request = "POST https://example.test/jsonapi/contact-groups/5001/financial-snapshot";

    [Fact]
    public async Task BackoffIncreasesCapsAtSixtySecondsAndStopsAfterEightRetries()
    {
        var calls = 0;
        var notices = new List<RateLimitRetry>();
        var error = Throttled();
        var backoff = new RateLimitBackoff(notices.Add, (_, _) => Task.CompletedTask);

        var thrown = await Assert.ThrowsAsync<ApiException>(() => backoff.ExecuteAsync<int>(() => { calls++; throw error; }, Request));

        Assert.Same(error, thrown);
        Assert.Equal(9, calls);
        Assert.Equal(8, notices.Count);
        var minimums = new[] { 2, 4, 8, 16, 32, 60, 60, 60 };
        for (var index = 0; index < notices.Count; index++)
        {
            Assert.InRange(notices[index].Delay.TotalSeconds, minimums[index], Math.Min(60, minimums[index] + 1));
            Assert.Equal(index + 1, notices[index].Retry);
            Assert.Equal(Request, notices[index].Request);
        }
    }

    [Theory]
    [InlineData("90", 90)]
    [InlineData("Mon, 19 Oct 2026 00:01:30 GMT", 90)]
    [InlineData(null, 2)]
    [InlineData("invalid", 2)]
    [InlineData("-1", 2)]
    [InlineData("0", 2)]
    [InlineData("Sun, 18 Oct 2026 00:00:00 GMT", 2)]
    public async Task RetryAfterSupportsSecondsDatesAndFallback(string header, int minimum)
    {
        var calls = 0;
        var notices = new List<RateLimitRetry>();
        var waits = new List<TimeSpan>();
        var backoff = new RateLimitBackoff(notices.Add, (wait, _) => { waits.Add(wait); return Task.CompletedTask; }, new TestClock());

        Assert.Equal(42, await backoff.ExecuteAsync(() => ++calls == 1 ? throw Throttled(header) : Task.FromResult(42), Request));

        Assert.Equal(2, calls);
        var notice = Assert.Single(notices);
        Assert.InRange(notice.Delay.TotalSeconds, minimum, minimum == 90 ? 90 : 3);
        Assert.Equal(notice.Delay.TotalSeconds, waits.Sum(wait => wait.TotalSeconds));
        Assert.All(waits, wait => Assert.True(wait <= TimeSpan.FromSeconds(60)));
    }

    [Theory]
    [InlineData(400)]
    [InlineData(401)]
    [InlineData(403)]
    [InlineData(422)]
    [InlineData(500)]
    [InlineData(503)]
    [InlineData(504)]
    public async Task OtherResponsesAreNeverRetried(int status)
    {
        var calls = 0;
        var error = new ApiException("Failure") { ResponseStatusCode = status };
        var backoff = new RateLimitBackoff(delay: (_, _) => throw new Exception("Must not wait."));

        var thrown = await Assert.ThrowsAsync<ApiException>(() => backoff.ExecuteAsync<int>(() => { calls++; throw error; }, Request));

        Assert.Same(error, thrown);
        Assert.Equal(1, calls);
    }

    [Fact]
    public async Task TransportFailuresAndTimeoutsAreNeverRetried()
    {
        foreach (var error in new Exception[] { new HttpRequestException("Connection lost"), new TaskCanceledException("Timed out") })
        {
            var calls = 0;
            var backoff = new RateLimitBackoff(delay: (_, _) => throw new Exception("Must not wait."));
            var thrown = await Record.ExceptionAsync(() => backoff.ExecuteAsync<int>(() => { calls++; throw error; }, Request));

            Assert.Same(error, thrown);
            Assert.Equal(1, calls);
        }
    }

    [Fact]
    public async Task CancellingTheWaitPreventsAnotherRequest()
    {
        using var cancellation = new CancellationTokenSource();
        var calls = 0;
        var backoff = new RateLimitBackoff(delay: (_, token) => { cancellation.Cancel(); return Task.FromCanceled(token); });

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => backoff.ExecuteAsync<int>(() =>
        {
            calls++;
            throw Throttled();
        }, Request, cancellation.Token));

        Assert.Equal(1, calls);
    }

    private static ApiException Throttled(string retryAfter = null) => new("Too many requests")
    {
        ResponseStatusCode = 429,
        ResponseHeaders = retryAfter == null ? new Dictionary<string, IEnumerable<string>>()
            : new Dictionary<string, IEnumerable<string>> { ["rEtRy-AfTeR"] = [retryAfter] }
    };

    private sealed class TestClock : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => new(2026, 10, 19, 0, 0, 0, TimeSpan.Zero);
    }
}
