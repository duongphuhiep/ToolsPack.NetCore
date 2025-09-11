using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ToolsPack.String;

namespace ToolsPack.Logging;

/// <summary>
/// This is a HttpClient's middleware that logs the Http's Request/Response.
/// It generates multiple log messages (for request, response) with <see cref="LogLevel.Information"/>.
/// Each log message contains a `HttpRequestCorrelationId` (in the log's scope) to correlate
/// between requests and responses logs.
/// <example>
/// Usage:
/// <code>
/// var httpClient = new HttpClient(new SimpleHttpLoggingMiddleware(logger));
/// </code>
/// </example>
/// </summary>
/// <seealso cref="CustomSimpleHttpLoggingMiddleware" />
public class SimpleHttpLoggingMiddleware : DelegatingHandler
{
    private readonly ILogger<SimpleHttpLoggingMiddleware> _logger;

    /// <summary>
    /// Middleware constructor.
    /// </summary>
    /// <param name="logger">non nullable</param>
    /// <exception cref="ArgumentNullException"></exception>
    public SimpleHttpLoggingMiddleware(ILogger<SimpleHttpLoggingMiddleware> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        string correlationId = ShortGuid.New(true);
        using var scope = _logger.BeginScope("HTTP-middleware {HttpRequestCorrelationId}", correlationId);

        _logger.LogInformation("Send HTTP Request {Method} {Uri}", request.Method, request.RequestUri);
        if (request.Content is not null)
        {
            var requestBody = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
            _logger.LogInformation("Send HTTP Request Body {Body}", requestBody);
        }

        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        _logger.LogInformation("Received HTTP Response {HttpStatus}", (int)response.StatusCode);
        if (response.Content is not null)
        {
            var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            _logger.LogInformation("Received HTTP Response Body {Body}", responseBody);
        }

        return response;
    }
}