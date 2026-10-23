using System.Net;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Microsoft.Kiota.Http.HttpClientLibrary.Middleware.Options;
using MyCrmSampleClient.Auth;
using MyCrmSampleClient.Kiota;
using MyCrmSampleClient.Kiota.Models;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Xunit;

namespace MyCrmSampleClient.Tests;

public sealed class AuthenticationTests
{
    [Fact]
    public async Task ValidTokenIsReusedAndAdviserHeaderIsPreserved()
    {
        var clock = new TestClock();
        using var authentication = new MyCrmKiotaAuthProvider(Token(clock, "valid"), 123,
            _ => throw new InvalidOperationException("A valid token should be reused."), clock);

        for (var i = 0; i < 2; i++)
        {
            var request = new RequestInformation();
            await authentication.AuthenticateRequestAsync(request);

            Assert.Equal("Bearer valid", Assert.Single(request.Headers["Authorization"]));
            Assert.Equal("123", Assert.Single(request.Headers["UserId"]));
        }
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(60)]
    public async Task ExpiredOrExpiringTokenIsRenewedBeforeSending(int secondsRemaining)
    {
        var clock = new TestClock();
        var acquired = 0;
        using var authentication = new MyCrmKiotaAuthProvider(Token(clock, "old", secondsRemaining), 123,
            _ => { acquired++; return Task.FromResult(Token(clock, "renewed")); }, clock);
        var request = new RequestInformation();
        request.Headers.Add("Authorization", "Bearer old");

        await authentication.AuthenticateRequestAsync(request);
        await authentication.AuthenticateRequestAsync(request);

        Assert.Equal(1, acquired);
        Assert.Equal("Bearer renewed", Assert.Single(request.Headers["Authorization"]));
        Assert.Equal("123", Assert.Single(request.Headers["UserId"]));
    }

    [Fact]
    public async Task ConcurrentRequestsShareOneTokenRenewal()
    {
        var clock = new TestClock();
        var acquired = 0;
        using var authentication = new MyCrmKiotaAuthProvider(Token(clock, "expired", -1), 123,
            async _ => { acquired++; await Task.Yield(); return Token(clock, "renewed"); }, clock);
        var requests = Enumerable.Range(0, 10).Select(_ => new RequestInformation()).ToArray();

        await Task.WhenAll(requests.Select(request => authentication.AuthenticateRequestAsync(request)));

        Assert.Equal(1, acquired);
        Assert.All(requests, request => Assert.Equal("Bearer renewed", Assert.Single(request.Headers["Authorization"])));
    }

    [Theory]
    [InlineData("failed")]
    [InlineData("empty")]
    [InlineData("expired")]
    public async Task UnusableRenewalStopsBeforeSendingTheApiRequest(string result)
    {
        var clock = new TestClock();
        using var authentication = new MyCrmKiotaAuthProvider(Token(clock, "old", -1), 123,
            _ => Task.FromResult(result switch
            {
                "failed" => new AuthResult(false, null, DateTime.MinValue),
                "empty" => Token(clock, ""),
                _ => Token(clock, "expired", -1)
            }), clock);
        using var handler = new RecordingHandler(_ => throw new InvalidOperationException("The API must not be called."));
        using var http = new HttpClient(handler);
        using var adapter = new HttpClientRequestAdapter(authentication, httpClient: http) { BaseUrl = "https://example.test" };

        var error = await Assert.ThrowsAsync<InvalidOperationException>(() => new MyCrmApiClient(adapter).Jsonapi.ContactGroups[2561123].GetAsync());

        Assert.Contains("API request was not sent", error.Message);
        Assert.Equal(0, handler.Calls);
    }

    [Fact]
    public async Task SnapshotAfterLongPromptUsesNewTokenAndDoesNotReplayAnEmpty401()
    {
        var clock = new TestClock();
        var acquired = 0;
        using var authentication = new MyCrmKiotaAuthProvider(Token(clock, "initial"), 123,
            _ => { acquired++; return Task.FromResult(Token(clock, "renewed")); }, clock);
        await authentication.AuthenticateRequestAsync(new RequestInformation());

        clock.Now = clock.Now.AddHours(2); // The user left the import confirmation open.
        using var handler = new RecordingHandler(request =>
        {
            Assert.Equal(HttpMethod.Post, request.Method);
            Assert.Equal("https://example.test/jsonapi/contact-groups/2561123/financial-snapshot", request.RequestUri!.AbsoluteUri);
            Assert.Equal("renewed", request.Headers.Authorization!.Parameter);

            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        });
        using var http = KiotaClientFactory.Create(KiotaClientFactory.CreateDefaultHandlers(), handler);
        using var adapter = new HttpClientRequestAdapter(authentication, httpClient: http) { BaseUrl = "https://example.test" };
        var client = new MyCrmApiClient(adapter);

        var error = await Assert.ThrowsAsync<ApiException>(() => client.Jsonapi.ContactGroups[2561123].FinancialSnapshot.PostAsync(
            new FinancialSnapshotDocument { Data = new FinancialSnapshot { Type = "financial-snapshots", Attributes = new FinancialSnapshotAttributes() } },
            request => request.Options.Add(new RetryHandlerOption { MaxRetry = 0 })));

        Assert.Equal(401, error.ResponseStatusCode);
        Assert.Equal(1, handler.Calls);
        Assert.Equal(1, acquired);
        Assert.Contains("Authentication was rejected", Samples.FinancialRequestAdvice(error.ResponseStatusCode));
        Assert.DoesNotContain("uncertain", Samples.FinancialRequestAdvice(error.ResponseStatusCode));
        Assert.Contains("uncertain", Samples.FinancialRequestAdvice(500));
    }

    [Fact]
    public async Task FailedRequestDiagnosticsIncludeExactUrlAndStatusWithoutCredentialsOrPayload()
    {
        var sink = new RecordingSink();
        using var logger = new LoggerConfiguration().WriteTo.Sink(sink).CreateLogger();
        using var handler = new ApiRequestDiagnosticsHandler(logger)
        {
            InnerHandler = new RecordingHandler(_ => new HttpResponseMessage(HttpStatusCode.Unauthorized))
        };
        using var http = new HttpClient(handler);
        using var request = new HttpRequestMessage(HttpMethod.Post, "https://example.test/jsonapi/contact-groups/2561123/financial-snapshot")
        {
            Content = new StringContent("private financial payload")
        };
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "private-token");

        using var response = await http.SendAsync(request);

        var logged = Assert.Single(sink.Events).RenderMessage();
        Assert.Contains("POST", logged);
        Assert.Contains(request.RequestUri!.AbsoluteUri, logged);
        Assert.Contains("401", logged);
        Assert.DoesNotContain("private", logged);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    private static AuthResult Token(TestClock clock, string value, int secondsRemaining = 3600) =>
        new(true, value, clock.GetUtcNow().UtcDateTime.AddSeconds(secondsRemaining));

    private sealed class TestClock : TimeProvider
    {
        public DateTimeOffset Now { get; set; } = new(2026, 10, 7, 0, 0, 0, TimeSpan.Zero);
        public override DateTimeOffset GetUtcNow() => Now;
    }

    private sealed class RecordingHandler(Func<HttpRequestMessage, HttpResponseMessage> respond) : HttpMessageHandler
    {
        public int Calls { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Calls++;
            return Task.FromResult(respond(request));
        }
    }

    private sealed class RecordingSink : ILogEventSink
    {
        public List<LogEvent> Events { get; } = [];
        public void Emit(LogEvent logEvent) => Events.Add(logEvent);
    }
}
