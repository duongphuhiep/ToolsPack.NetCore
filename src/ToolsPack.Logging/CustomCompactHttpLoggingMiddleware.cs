using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ToolsPack.Logging;

/// <summary>
/// This is a HttpClient's middleware that logs the Http's Request/Response and the elapsed time of the request.
/// It generates only one log message per request and response. It is a configurable implementation of the <see cref="CompactHttpLoggingMiddleware"/>.
/// With default configuration, it behaves the same as <see cref="CompactHttpLoggingMiddleware"/> with slightly worse performance.
/// The Logging level and redaction are customizable via <see cref="CompactHttpLoggingMiddlewareConfig"/>.
/// </summary>
/// <seealso cref="CompactHttpLoggingMiddleware" />
public class CustomCompactHttpLoggingMiddleware : DelegatingHandler
{
    private readonly ILogger<CustomCompactHttpLoggingMiddleware> _logger;
    private readonly CompactHttpLoggingMiddlewareConfig _config;

    /// <summary>
    /// Middleware constructor.
    /// </summary>
    /// <param name="logger">non-nullable</param>
    /// <exception cref="ArgumentNullException"></exception>
    public CustomCompactHttpLoggingMiddleware(ILogger<CustomCompactHttpLoggingMiddleware> logger,
        CompactHttpLoggingMiddlewareConfig config)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _config = config ?? throw new ArgumentNullException(nameof(config));
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

    private async Task LogRequestResponse(HttpRequestMessage request, HttpResponseMessage response,
        long elapsedMilliseconds)
    {
        string? requestBody = request.Content is null
            ? null
            : await request.Content.ReadAsStringAsync().ConfigureAwait(false);
        string? responseBody = response.Content is null
            ? null
            : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        var logLevel = _config.LogLevelSelector(request.Method, request.RequestUri, response.StatusCode,
            responseBody?.Length, null, elapsedMilliseconds);
        if (logLevel == LogLevel.None)
        {
            return;
        }

        _logger.Log(logLevel: logLevel,
            message:
            "Sent HTTP Request {Method} {Uri} {RequestBody}. Got response {HttpStatus} {ResponseBody}. Elapsed {ElapsedMilliseconds}",
            request.Method,
            request.RequestUri,
            _config.RequestBodyRedactor(requestBody, request.Method, request.RequestUri, response.StatusCode,
                responseBody?.Length),
            (int)response.StatusCode,
            _config.ResponseBodyRedactor(responseBody, request.Method, request.RequestUri, response.StatusCode,
                responseBody?.Length),
            elapsedMilliseconds);
    }

    private async Task LogRequestException(HttpRequestMessage request, Exception exception, long elapsedMilliseconds)
    {
        string? requestBody = request.Content is null
            ? null
            : await request.Content.ReadAsStringAsync().ConfigureAwait(false);
        
        var logLevel = _config.LogLevelSelector(request.Method, request.RequestUri, null, null, exception,
            elapsedMilliseconds);
        if (logLevel == LogLevel.None)
        {
            return;
        }

        _logger.Log(logLevel: logLevel, exception: exception,
            message: "Sent HTTP Request {Method} {Uri} {RequestBody}. Got error. Elapsed {ElapsedMilliseconds}",
            request.Method, request.RequestUri, 
            _config.RequestBodyRedactor(requestBody, request.Method, request.RequestUri, null, null), elapsedMilliseconds);
    }
}