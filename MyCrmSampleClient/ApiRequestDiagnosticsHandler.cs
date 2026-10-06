using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace MyCrmSampleClient;

/// <summary>Reports failed request URLs without logging tokens, headers or financial payloads.</summary>
internal sealed class ApiRequestDiagnosticsHandler(ILogger logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await base.SendAsync(request, cancellationToken);
            if (!response.IsSuccessStatusCode)
                logger.Warning("Request failed: {Method} {Url}; HTTP {StatusCode}", request.Method, request.RequestUri?.AbsoluteUri, (int)response.StatusCode);

            return response;
        }
        catch (Exception error) when (error is HttpRequestException or OperationCanceledException)
        {
            logger.Warning("Request failed: {Method} {Url}; no completed HTTP response ({ErrorType})", request.Method, request.RequestUri?.AbsoluteUri, error.GetType().Name);
            throw;
        }
    }
}
