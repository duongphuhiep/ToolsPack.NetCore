using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using ToolsPack.Logging.Testing;
using Xunit.Abstractions;

namespace ToolsPack.Logging.Tests;

/// <summary>
/// Helper to configure logging to the TestOutput, Seq and MockLogger.
/// You can start a Seq server on localhost with help of the `docker-compose.yaml` file in this project
/// </summary>
public static class TestOutputHelperExtension
{
    const string DefaultOutputTemplateConst = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}  //[{SourceContext}]{NewLine}{Exception}";

    public static readonly string DefaultOutputTemplate = DefaultOutputTemplateConst;

    /// <summary>
    /// Create a new Logger factory which will log
    /// - to the TestOutput
    /// - to the given MockLogger (optional)
    /// - and to Seq localhost:5341 (only in debug mode).
    /// </summary>
    public static ILoggerFactory CreateLoggerFactory(this ITestOutputHelper testOutputHelper, MockLogger? mockLogger = null, string outputTemplate = DefaultOutputTemplateConst)
    {
        var serilogLogger = testOutputHelper.CreateSerilogLogger(outputTemplate);
        return LoggerFactory.Create(builder => builder.BuildTestLogging(serilogLogger, mockLogger));
    }

    /// <summary>
    /// Inject TestOutput, Seq (debug only) and MockLogger (optional) to the application logging
    /// </summary>
    public static IServiceCollection AddTestLogging(this IServiceCollection service, ITestOutputHelper testOutputHelper, MockLogger? mockLogger = null, string outputTemplate = DefaultOutputTemplateConst)
    {
        var serilogLogger = CreateSerilogLogger(testOutputHelper, outputTemplate);
        service.AddLogging(builder => builder.BuildTestLogging(serilogLogger, mockLogger));
        return service;
    }

    private static Serilog.Core.Logger CreateSerilogLogger(this ITestOutputHelper testOutputHelper, string outputTemplate)
    {
        var serilogLogger = new LoggerConfiguration()
                   .MinimumLevel.Verbose()
                   .WriteTo.TestOutput(testOutputHelper, outputTemplate: outputTemplate)
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
    public static ILoggerFactory CreateLoggerFactory<T>(this ITestOutputHelper testOutputHelper, MockLogger<T>? mockLogger = null, string outputTemplate = DefaultOutputTemplateConst)
    {
        var serilogLogger = CreateSerilogLogger(testOutputHelper, outputTemplate);
        return LoggerFactory.Create(builder => builder.BuildTestLogging(serilogLogger, mockLogger));
    }

    /// <summary>
    /// Inject TestOutput, Seq (debug only) and MockLogger (optional) to the application logging
    /// </summary>
    public static IServiceCollection AddTestLogging<T>(this IServiceCollection service, ITestOutputHelper testOutputHelper, MockLogger<T>? mockLogger = null, string outputTemplate = DefaultOutputTemplateConst)
    {
        var serilogLogger = CreateSerilogLogger(testOutputHelper, outputTemplate);
        service.AddLogging(builder => builder.BuildTestLogging(serilogLogger, mockLogger));
        return service;
    }
    #endregion
}