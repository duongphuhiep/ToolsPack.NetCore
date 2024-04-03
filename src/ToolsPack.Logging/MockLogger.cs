using Microsoft.Extensions.Logging;
using System;

namespace ToolsPack.Logging.Testing;


using LogState = System.Collections.Generic.IReadOnlyList<System.Collections.Generic.KeyValuePair<string, object?>>;

/// <summary>
/// The MockLogger provides abstracted IsLogged hooks which are invoked for every log events.
/// A MockLogger is usually instantiated by a Mock library (such as Moq, NSubstitute..).
/// So that unit tests can spy (or verify) on the IsLogged hooks in order to Assert the log content.
/// 
/// Other alternative is the FakeLogger in the package [Microsoft.Extensions.Diagnostics.Testing]
/// </summary>
public abstract class MockLogger : ILogger
{
    public bool IsEnabled(LogLevel logLevel) => true;

    public abstract IDisposable BeginScope<TState>(TState state);

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var formattedMessage = formatter(state, exception);
        IsLogged(logLevel, eventId, exception, formattedMessage);
        if (typeof(LogState).IsAssignableFrom(typeof(TState)))
        {
            var logState = state as LogState;
            IsLogged(logLevel, eventId, exception, formattedMessage, logState);
        }
    }

    /// <summary>
    /// This hook is invoked on all log events
    /// </summary>
    public abstract void IsLogged(LogLevel logLevel, EventId eventId, Exception? exception, string formattedMessage);

    /// <summary>
    /// This hook is (pratically) invoked on all log events although there is no guarantee. In case of structured logging, we can spy/verify on
    /// the state argument which exposes all the logging components.
    /// </summary>
    public abstract void IsLogged(LogLevel logLevel, EventId eventId, Exception? exception, string formattedMessage, LogState? state);
}

public abstract class MockLogger<T> : MockLogger { }
