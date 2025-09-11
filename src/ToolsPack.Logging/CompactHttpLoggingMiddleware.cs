using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ToolsPack.String;

namespace ToolsPack.Logging;

/// <summary>
/// This is a HttpClient's middleware that logs the Http's Request/Response and the elapsed time of the request.
/// Unlike <see cref="SimpleHttpLoggingMiddleware"/> and <see cref="CustomSimpleHttpLoggingMiddleware"/>,
/// It generates only one log message per request and response. So the log message doesn't need to have a `HttpRequestCorrelationId`.
/// <example>
/// Usage:
/// <code>
/// var httpClient = new HttpClient(new CompactHttpLoggingMiddleware(logger));
/// </code>
/// </example>
/// </summary>
/// <seealso cref="CustomCompactHttpLoggingMiddleware" />
public class CompactHttpLoggingMiddleware : DelegatingHandler
{
    private readonly ILogger<CompactHttpLoggingMiddleware> _logger;

    /// <summary>
    /// Middleware constructor.
    /// </summary>
    /// <param name="logger">non nullable</param>
    /// <exception cref="ArgumentNullException"></exception>
    public CompactHttpLoggingMiddleware(ILogger<CompactHttpLoggingMiddleware> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        Stopwatch sw = Stopwatch.StartNew();
        try
        {
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            await LogRequestResponse(request, response, sw.ElapsedMilliseconds).ConfigureAwait(false);
            return response;
        }
        catch (Exception ex)
        {
            await LogRequestException(request, ex, sw.ElapsedMilliseconds).ConfigureAwait(false);
            throw;
        }
    }

    private async Task LogRequestResponse(HttpRequestMessage request, HttpResponseMessage response, long elapsedMilliseconds)
    {
        string? requestBody = request.Content is null ? null : await request.Content.ReadAsStringAsync().ConfigureAwait(false);
        string? responseBody = response.Content is null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        _logger.LogInformation("Sent HTTP Request {Method} {Uri} {RequestBody}. Got response {HttpStatus} {ResponseBody}. Elapsed {ElapsedMilliseconds}", request.Method, request.RequestUri, requestBody, (int)response.StatusCode, responseBody, elapsedMilliseconds);
    }
    private async Task LogRequestException(HttpRequestMessage request, Exception exception, long elapsedMilliseconds)
    {
        string? requestBody = request.Content is null ? null : await request.Content.ReadAsStringAsync().ConfigureAwait(false);
        _logger.LogInformation(exception, "Sent HTTP Request {Method} {Uri} {RequestBody}. Got error. Elapsed {ElapsedMilliseconds}", request.Method, request.RequestUri, requestBody, elapsedMilliseconds);
    }
}