using Microsoft.Extensions.Logging;
using System;

namespace ToolsPack.Logging.Tests;

public record SomePersonObject(string Name, int Age);
public record SomeOrderObject(string Id, double Amount);

public static partial class LogMessagesSample
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Information, Message = "Send request {message}")]
    public static partial void LogSendRequestInfo(this ILogger logger, string message);

    [LoggerMessage(EventId = 1, Level = LogLevel.Error, Message = "Received something wrong")]
    public static partial void LogGetResponseError(this ILogger logger, Exception ex);

    [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "The person {person} odered {order}")]
    public static partial void LogStructuringSampleInfo(this ILogger logger, SomePersonObject person, SomeOrderObject order);
}

