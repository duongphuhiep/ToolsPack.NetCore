using System;
using Microsoft.Extensions.Logging;

namespace ToolsPack.Logging.Tests;

/// <summary>
///     - in normal time, this is NullLogger
///     - but whenever a Xunit TestOutput is set in the TestOutputInitializer then the Logger will log to the testOutput
/// </summary>
public class TestOutputLogger : ILogger
{
    private readonly string _category;
    private readonly TestOutputInitializer _testOutput;

    public TestOutputLogger(TestOutputInitializer testRegistration, string category)
    {
        _testOutput = testRegistration;
        _category = category;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return _testOutput.Logger.BeginScope(state);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _testOutput.Logger.IsEnabled(logLevel);
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        //TODO find a way to include the category in the message?
        _testOutput.Logger.Log(logLevel, eventId, state, exception, formatter);
        //Microsoft.Extensions.Logging.Formatted
    }
}