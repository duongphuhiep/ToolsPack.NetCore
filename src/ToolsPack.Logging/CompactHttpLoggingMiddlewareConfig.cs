using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using ToolsPack.String;

namespace ToolsPack.Logging;

/// <summary>
/// This is the configuration for the <see cref="CustomCompactHttpLoggingMiddleware"/>.
/// It decides which Http's Request/Response you want (or don't want) to log and also the logging's redaction.
/// The default behavior is to log all Http's Request/Response with <see cref="LogLevel.Information"/> with no redaction, 
/// same behavior as the <see cref="CompactHttpLoggingMiddleware"/>.
/// </summary>
public class CompactHttpLoggingMiddlewareConfig
{
    public delegate LogLevel GetLogLevel(HttpMethod method, Uri uri, HttpStatusCode? statusCode, long? responseBodyLength, Exception? exception, long elapsedMilliseconds);

    /// <summary>
    /// Implement this delegate to customize the log level depending on the request response situation which you wanted to log.
    /// Return <see cref="LogLevel.None"/> to disable logging. You can for example promote the log level to <see cref="LogLevel.Warning"/>
    /// for slow requests (big elapsedMilliseconds);
    /// OR when the response was too big; Promote the log level to <see cref="LogLevel.Error"/> in case exception or for a particular HttpStatusCode;
    /// OR skip (<see cref="LogLevel.None"/>) for GET requests/responses.
    /// By default, (if not set) the log level is <see cref="LogLevel.Information"/> 
    /// </summary>
    public GetLogLevel LogLevelSelector { get; set; } =
        (method, uri, statusCode, responseBodyLength, exception, elapsedMilliseconds) => LogLevel.Information; 

    /// <summary>
    /// This function defines How to hide sensitive data in the request body. By default, (if not set), it does nothing.
    /// </summary>
    public Func<string, string> RequestBodyRedactor { get; set; } = body => body;
    
    /// <summary>
    /// This function defines How to hide sensitive data in the response body. By default, (if not set), it does nothing.
    /// </summary>
    public Func<string, string> ResponseBodyRedactor { get; set; } = body => body;
}