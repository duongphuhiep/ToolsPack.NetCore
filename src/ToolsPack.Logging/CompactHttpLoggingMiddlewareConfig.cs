using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace ToolsPack.Logging;

/// <summary>
/// This is the configuration for the <see cref="CustomCompactHttpLoggingMiddleware"/>.
/// It decides which Http's Request/Response you want (or don't want) to log and also the logging's redaction.
/// The default behavior is to log all Http's Request/Response with <see cref="LogLevel.Information"/> with no redaction, 
/// same behavior as the <see cref="CompactHttpLoggingMiddleware"/>.
/// </summary>
public class CompactHttpLoggingMiddlewareConfig
{
    public delegate LogLevel GetLogLevel(HttpMethod method, Uri? uri, HttpStatusCode? statusCode, long? responseBodyLength, Exception? exception, long elapsedMilliseconds);
    public delegate string? BodyRedactor(string? body, HttpMethod method, Uri? uri, HttpStatusCode? statusCode, int? responseBodyLength);
    
    /// <summary>
    /// Implement this delegate to customize the log level depending on the request response situation which you wanted to log.
    /// Return <see cref="LogLevel.None"/> to disable logging. You can for example promote the log level to <see cref="LogLevel.Warning"/>
    /// for slow requests (big elapsedMilliseconds);
    /// OR when the response was too big; Promote the log level to <see cref="LogLevel.Error"/> in case exception or for a particular HttpStatusCode;
    /// OR skip (<see cref="LogLevel.None"/>) for GET requests/responses.
    /// By default, (if not set) the log level is <see cref="LogLevel.Information"/> 
    /// </summary>
    public GetLogLevel LogLevelSelector { get; set; } = (_, _, _, _, _, _) => LogLevel.Information; 

    /// <summary>
    /// This function defines How to hide sensitive data in the request body. By default, (if not set), it does nothing.
    /// You can base the redaction on the request's Method, request's Uri and the response's statusCode and the response's body length.
    /// </summary>
    public BodyRedactor RequestBodyRedactor { get; set; } = (body, _, _, _, _) => body;
    
    /// <summary>
    /// This function defines How to hide sensitive data in the response body. By default, (if not set), it does nothing.
    /// You can base the redaction on the request's Method, request's Uri and the response's statusCode and the response's body length.
    /// </summary>
    public BodyRedactor ResponseBodyRedactor { get; set; } = (body, _, _, _, _) => body;
}