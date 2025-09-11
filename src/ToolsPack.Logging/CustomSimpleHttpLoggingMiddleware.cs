using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ToolsPack.Logging;

/// <summary>
/// This is a HttpClient's middleware that logs the Http's Request/Response.
/// It is a configurable implementation of <see cref="SimpleHttpLoggingMiddleware"/>.
/// With default configuration, it behaves the same as <see cref="SimpleHttpLoggingMiddleware"/> with slightly worse performance. 
/// The Log level is customizable via <see cref="SimpleHttpLoggingMiddlewareConfig"/>. You can
/// configure it to choose which Http's Request/Response you want (or don't want) to log.
/// You can also configure the log redaction to hide sensitive information in the request and response's body before logging.
/// </summary>
public class CustomSimpleHttpLoggingMiddleware : DelegatingHandler
{
    private readonly ILogger<CustomSimpleHttpLoggingMiddleware> _logger;
    private readonly SimpleHttpLoggingMiddlewareConfig _config;

    /// <summary>
    /// Middleware constructor.
    /// </summary>
    /// <param name="logger">non nullable</param>
    /// <param name="config">non nullable, you can create a default config with the default constructor</param>
    /// <exception cref="ArgumentNullException"></exception>
    public CustomSimpleHttpLoggingMiddleware(ILogger<CustomSimpleHttpLoggingMiddleware> logger,
        SimpleHttpLoggingMiddlewareConfig config)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        string correlationId = _config.CorrelationIdGenerator();
        using var scope = _logger.BeginScope("HTTP-middleware {HttpRequestCorrelationId}", correlationId);

        var requestUriLogLevel = _config.RequestUriLogLevel(request.Method, request.RequestUri);
        if (requestUriLogLevel != LogLevel.None)
        {
            _logger.Log(logLevel: requestUriLogLevel,
                message: "Send HTTP Request {Method} {Uri}", request.Method, request.RequestUri);
        }

        if (request.Content is not null)
        {
            var requestBodyLogLevel = _config.RequestBodyLogLevel(request.Method, request.RequestUri);
            if (requestBodyLogLevel != LogLevel.None)
            {
                var requestBody = await request.Content.ReadAsStringAsync().ConfigureAwait(false);
                _logger.Log(logLevel: requestBodyLogLevel, message: "Send HTTP Request Body {Body}", _config.RequestBodyRedactor(requestBody));
            }
        }

        var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        var responseStatusLogLevel = _config.ResponseStatusLogLevel(request.Method, request.RequestUri, response.StatusCode);
        if (responseStatusLogLevel != LogLevel.None)
        {
            _logger.Log(logLevel: responseStatusLogLevel,
                message: "Received HTTP Response {HttpStatus}", (int)response.StatusCode);
        }
        
        if (response.Content is not null)
        {
            var responseBodyLogLevel = _config.ResponseBodyLogLevel(request.Method, request.RequestUri, response.StatusCode);
            if (responseBodyLogLevel != LogLevel.None)
            {
                var responseBody = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                _logger.Log(logLevel: responseBodyLogLevel,
                    message: "Received HTTP Response Body {Body}", _config.ResponseBodyRedactor(responseBody));
            }
        }

        return response;
    }
}