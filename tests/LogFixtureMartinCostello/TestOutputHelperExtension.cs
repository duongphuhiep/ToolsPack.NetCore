using MartinCostello.Logging.XUnit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace ToolsPack.Logging.TestingTools;

/// <summary>
/// Helper to configure logging to the TestOutput, Seq, and Console.
/// You can start a Seq server on localhost with help of the `docker-compose.yaml` file in this project
/// </summary>
public static class TestOutputHelperExtension
{
    /// <summary>
    /// This test logging config will override all other log config
    /// </summary>
    private static ILoggingBuilder BuildTestLogging(this ILoggingBuilder builder)
    {
        builder.ClearProviders();

        builder.AddSeq();
        builder.AddConsole();
        builder.AddFilter(null, LogLevel.Trace);
        builder.AddFilter("Microsoft", LogLevel.Error);
        builder.AddFilter("System", LogLevel.Error);
        builder.AddFilter("App.Telemetry", LogLevel.Error);
        builder.AddFilter("Microsoft.EntityFramework", LogLevel.Information);
        return builder;
    }

    /// <summary>
    /// Create a new Logger factory which will log
    /// - to the TestOutput
    /// - and to Seq localhost:5341.
    /// - and to Console
    /// </summary>
    public static ILoggerFactory CreateLoggerFactory(this ITestOutputHelper testOutputHelper)
        => LoggerFactory.Create(builder => builder.BuildTestLogging(testOutputHelper));

    /// <summary>
    /// Inject TestOutput, Seq and Console to the current application logging pipeline. It will override other log configs.
    /// </summary>
    public static IServiceCollection AddTestLogging(this IServiceCollection service, ITestOutputHelper testOutputHelper)
        => service.AddLogging(builder => builder.BuildTestLogging(testOutputHelper));

    /// <summary>
    /// Create a new Logger factory which will log
    /// - to the TestOutput
    /// - and to Seq localhost:5341.
    /// - and to Console
    /// </summary>
    public static ILoggerFactory CreateLoggerFactory(this IMessageSink messageSink, ITestOutputHelperAccessor? testOutputHelperAccessor = null)
        => LoggerFactory.Create(builder => builder.BuildTestLogging(messageSink, testOutputHelperAccessor));

    /// <summary>
    /// Inject TestOutput, Seq and Console to the current application logging pipeline. It will override other log configs.
    /// </summary>
    public static IServiceCollection AddTestLogging(this IServiceCollection service, IMessageSink messageSink, ITestOutputHelperAccessor? testOutputHelperAccessor = null)
        => service.AddLogging(builder => builder.BuildTestLogging(messageSink, testOutputHelperAccessor));

    private static ILoggingBuilder BuildTestLogging(this ILoggingBuilder builder, IMessageSink messageSink, ITestOutputHelperAccessor? testOutputHelperAccessor)
    {
        builder
            .BuildTestLogging()
            .AddXUnit(messageSink);
        if (testOutputHelperAccessor is not null)
        {
            builder.AddXUnit(testOutputHelperAccessor);
        }
        return builder;
    }

    private static ILoggingBuilder BuildTestLogging(this ILoggingBuilder builder, ITestOutputHelper testOutputHelper)
        => builder
            .BuildTestLogging()
            .AddXUnit(testOutputHelper);
}