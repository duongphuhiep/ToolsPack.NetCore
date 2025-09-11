using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using ToolsPack.String;

namespace ToolsPack.Logging;

/// <summary>
/// This is the configuration for the <see cref="CustomSimpleHttpLoggingMiddleware"/>.
/// It decides which Http's Request/Response you want (or don't want) to log, logging's redaction.
/// The default behavior is to log all Http's Request/Response with <see cref="LogLevel.Information"/>; 
/// same behavior as <see cref="SimpleHttpLoggingMiddleware"/>.
/// </summary>
public class SimpleHttpLoggingMiddlewareConfig
{
    /// <summary>
    /// Implement this delegate to customize the log level for the request's URI depending on the request's Method and Uri.
    /// Return LogLevel.None to disable logging.
    /// </summary>
    public Func<HttpMethod, Uri?, LogLevel> RequestUriLogLevel { get; set; } = (_, _) => LogLevel.Information;

    /// <summary>
    /// Implement this delegate to customize the log level for the request's body depending on the request's Method and Uri.
    /// Return LogLevel.None to disable logging
    /// </summary>
    public Func<HttpMethod, Uri?, LogLevel> RequestBodyLogLevel { get; set; } = (_, _) => LogLevel.Information;

    /// <summary>
    /// Implement this delegate to customize the log level for the response's status code depending on the request's Method, request's Uri and the response's statusCode.
    /// Return LogLevel.None to disable logging
    /// </summary>
    public Func<HttpMethod, Uri?, HttpStatusCode, LogLevel> ResponseStatusLogLevel { get; set; } =
        (_, _, _) => LogLevel.Information;

    /// <summary>
    /// Implement this delegate to customize the log level for the response's body depending on the request's Method, request's Uri and the response's statusCode.
    /// Return LogLevel.None to disable logging
    /// </summary>
    public Func<HttpMethod, Uri?, HttpStatusCode, LogLevel> ResponseBodyLogLevel { get; set; } =
        (_, _, _) => LogLevel.Information;

    /// <summary>
    /// This function is used to generate the correlation id. By default, (if not set), it generates "short guid" with <see cref="ShortGuid.New(bool)" />.
    /// </summary>
    public Func<string> CorrelationIdGenerator { get; set; } = () => ShortGuid.New(true);
    
    /// <summary>
    /// This function defines How to hide sensitive data in the request body. By default, (if not set), it does nothing.
    /// You can base the redaction on the request's Method and Uri.
    /// </summary>
    public Func<string, HttpMethod, Uri?, string> RequestBodyRedactor { get; set; } = (body, _, _) => body;
    
    /// <summary>
    /// This function defines How to hide sensitive data in the response body. By default, (if not set), it does nothing.
    /// You can base the redaction on the request's Method, request's Uri and the response's statusCode.
    /// </summary>
    public Func<string, HttpMethod, Uri?, HttpStatusCode, string> ResponseBodyRedactor { get; set; } = (body, _, _, _) => body;
}