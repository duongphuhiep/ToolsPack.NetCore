using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Serilog;
using Serilog.Extensions.Logging;
using ToolsPack.Logging.Testing;
using Xunit.Abstractions;

namespace ToolsPack.Logging.Tests;

/// <summary>
/// Helper to configure logging to the TestOutput, Seq and MockLogger.
/// You can start a Seq server on localhost with help of the `docker-compose.yaml` file in this project
/// </summary>
public static class TestOutputMockLogging
{
    /// <summary>
    /// Create a new Logger factory which will log
    /// - to the TestOutput
    /// - to the given MockLogger (optional)
    /// - and to Seq localhost:5341 (only in debug mode).
    /// </summary>
    public static ILoggerFactory CreateLoggerFactory(this ITestOutputHelper testOutputHelper, MockLogger? mockLogger = null)
    {
        var serilogLogger = CreateSerilogLogger(testOutputHelper);
        return LoggerFactory.Create(builder => builder.BuildTestLogging(serilogLogger, mockLogger));
    }

    /// <summary>
    /// Create a "combo" Logger factory and a NSubstitute's MockLogger. The Logger factory will log
    /// - to the TestOutput
    /// - to the MockLogger
    /// - and to Seq localhost:5341 (only in debug mode).
    /// </summary>
    public static void CreateLoggerFactoryAndMockLogger(this ITestOutputHelper testOutputHelper, out ILoggerFactory loggerFactory, out MockLogger mockLogger)
    {
        mockLogger = Substitute.For<MockLogger>();
        loggerFactory = testOutputHelper.CreateLoggerFactory(mockLogger);
    }

    /// <summary>
    /// Inject TestOutput, Seq (debug only) and MockLogger (optional) to the application logging
    /// </summary>
    public static IServiceCollection AddTestLogging(this IServiceCollection service, ITestOutputHelper testOutputHelper, MockLogger? mockLogger = null)
    {
        var serilogLogger = CreateSerilogLogger(testOutputHelper);
        service.AddLogging(builder => builder.BuildTestLogging(serilogLogger, mockLogger));
        return service;
    }

    private static Serilog.Core.Logger CreateSerilogLogger(ITestOutputHelper testOutputHelper)
    {
        var serilogLogger = new LoggerConfiguration()
                   .MinimumLevel.Verbose()
                   .WriteTo.TestOutput(testOutputHelper)
#if DEBUG
                   .WriteTo.Seq("http://localhost:5341/")
#endif
                   .CreateLogger();
        return serilogLogger;
    }

    private static ILoggingBuilder BuildTestLogging(this ILoggingBuilder builder, Serilog.Core.Logger? serilogLogger, MockLogger? mockLogger)
    {
        if (serilogLogger is not null)
        {
            builder.AddSerilog(serilogLogger);
            builder.AddFilter<SerilogLoggerProvider>("Microsoft", LogLevel.Error);
            builder.AddFilter<SerilogLoggerProvider>("System", LogLevel.Error);
            builder.AddFilter<SerilogLoggerProvider>("App.Telemetry", LogLevel.Error);
        }
        if (mockLogger is not null)
        {
            builder.AddMockLogger(mockLogger);
            builder.AddFilter(null, LogLevel.Trace);
            builder.AddFilter("Microsoft", LogLevel.Error);
            builder.AddFilter("System", LogLevel.Error);
            builder.AddFilter("App.Telemetry", LogLevel.Error);
        }
        return builder;
    }

    #region Generics
    /// <summary>
    /// Create a new Logger factory which will log
    /// - to the TestOutput
    /// - to the given MockLogger (optional)
    /// - and to Seq localhost:5341 (only in debug mode).
    /// </summary>
    public static ILoggerFactory CreateLoggerFactory<T>(this ITestOutputHelper testOutputHelper, MockLogger<T>? mockLogger = null)
    {
        var serilogLogger = CreateSerilogLogger(testOutputHelper);
        return LoggerFactory.Create(builder => builder.BuildTestLogging(serilogLogger, mockLogger));
    }

    /// <summary>
    /// Create a "combo" Logger factory and a NSubstitute's MockLogger. The Logger factory will log
    /// - to the TestOutput
    /// - to the MockLogger
    /// - and to Seq localhost:5341 (only in debug mode).
    /// </summary>
    public static void CreateLoggerFactoryAndMockLogger<T>(this ITestOutputHelper testOutputHelper, out ILoggerFactory loggerFactory, out MockLogger<T> mockLogger)
    {
        mockLogger = Substitute.For<MockLogger<T>>();
        loggerFactory = testOutputHelper.CreateLoggerFactory(mockLogger);
    }

    /// <summary>
    /// Inject TestOutput, Seq (debug only) and MockLogger (optional) to the application logging
    /// </summary>
    public static IServiceCollection AddTestLogging<T>(this IServiceCollection service, ITestOutputHelper testOutputHelper, MockLogger<T>? mockLogger = null)
    {
        var serilogLogger = CreateSerilogLogger(testOutputHelper);
        service.AddLogging(builder => builder.BuildTestLogging(serilogLogger, mockLogger));
        return service;
    }
    #endregion
}