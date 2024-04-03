using MartinCostello.Logging.XUnit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace ToolsPack.Logging.TestingTools;

/// <summary>
/// Helper to configure logging to the TestOutput, Seq.
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
        //builder.AddConsole(); //<= it won't help, nothing will be output to the Console
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
    /// </summary>
    public static ILoggerFactory CreateLoggerFactory(this ITestOutputHelper testOutputHelper, Action<XUnitLoggerOptions>? options = null)
        => LoggerFactory.Create(builder => builder.BuildTestLogging(testOutputHelper, options));

    /// <summary>
    /// Inject TestOutput, Seq to the current application logging pipeline. It will override other log configs.
    /// </summary>
    public static IServiceCollection AddTestLogging(this IServiceCollection service, ITestOutputHelper testOutputHelper, Action<XUnitLoggerOptions>? options = null)
        => service.AddLogging(builder => builder.BuildTestLogging(testOutputHelper, options));

    /// <summary>
    /// Create a new Logger factory which will log
    /// - to the TestOutput
    /// - and to Seq localhost:5341.
    /// </summary>
    public static ILoggerFactory CreateLoggerFactory(this IMessageSink messageSink, ITestOutputHelperAccessor? testOutputHelperAccessor = null, Action<XUnitLoggerOptions>? options = null)
        => LoggerFactory.Create(builder => builder.BuildTestLogging(messageSink, testOutputHelperAccessor, options));

    /// <summary>
    /// Inject TestOutput, Seq to the current application logging pipeline. It will override other log configs.
    /// </summary>
    public static IServiceCollection AddTestLogging(this IServiceCollection service, IMessageSink messageSink, ITestOutputHelperAccessor? testOutputHelperAccessor = null, Action<XUnitLoggerOptions>? options = null)
        => service.AddLogging(builder => builder.BuildTestLogging(messageSink, testOutputHelperAccessor, options));

    private static ILoggingBuilder BuildTestLogging(this ILoggingBuilder builder, IMessageSink messageSink, ITestOutputHelperAccessor? testOutputHelperAccessor, Action<XUnitLoggerOptions>? options)
    {
        builder.BuildTestLogging();
        if (options is null)
        {
            builder.AddXUnit(messageSink);
        }
        else
        {
            builder.AddXUnit(messageSink, options);
        }
        if (testOutputHelperAccessor is not null)
        {
            if (options is null)
            {
                builder.AddXUnit(testOutputHelperAccessor);
            }
            else
            {
                builder.AddXUnit(testOutputHelperAccessor, options);
            }
        }
        return builder;
    }

    private static ILoggingBuilder BuildTestLogging(this ILoggingBuilder builder, ITestOutputHelper testOutputHelper, Action<XUnitLoggerOptions>? options)
    {
        builder.BuildTestLogging();
        if (options is null)
        {
            builder.AddXUnit(testOutputHelper);
        }
        else
        {
            builder.AddXUnit(testOutputHelper, options);
        }

        return builder;
    }
}
