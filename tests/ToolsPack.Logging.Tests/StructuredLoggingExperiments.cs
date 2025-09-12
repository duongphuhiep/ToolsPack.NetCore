using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Xunit;
using Xunit.Abstractions;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace ToolsPack.Logging.Tests;

/// <summary>
/// Start victoria-logs on your local machine before running these experiments.
/// You can use `docker-compose.yml` in this project to start victoria-logs. 
/// </summary>
public class StructuredLoggingExperiments(ITestOutputHelper testOutputHelper)
{
    private static readonly ActivitySource ActivitySource = new(nameof(StructuredLoggingExperiments));
    
    private static readonly Dictionary<string, string[]> _SampleClaims = new()
        { { "k1", ["v1", "v2"] }, { "k2", ["v3", "v4"] } };

    /// <summary>
    /// This test show that:
    /// There's no different between "@claims" and "claims". Because only serilog would destruct the object automatically with '@'
    /// </summary>
    [Fact]
    public async Task StandardLoggingTest()
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddOpenTelemetry(options =>
            {
                options.SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(nameof(StandardLoggingTest))
                );
                options.IncludeScopes = true;
                options.IncludeFormattedMessage = true;
                options.AddOtlpExporter();
            });
        });
        ILogger<StructuredLoggingExperiments> logger = loggerFactory.CreateLogger<StructuredLoggingExperiments>();

        logger.LogClaims("Without @", _SampleClaims);
        logger.LogClaimsAutoDestructuring("With @", _SampleClaims);
        logger.LogInformation("Normal LogInformation {@claims} {CurrentTraceId}", _SampleClaims, Activity.Current?.TraceId);    
    }
    
    /// <summary>
    /// This test show that:
    /// Serilog indeed try to destructure automatically '{@claims}', but it is only applied for standard log methods:
    /// `logger.LogInformation()`, `logger.LogWarning()`, `logger.LogError()`... If the log is auto-generated for high-performance logging then
    /// the '@' in `{@claims}` won't do anything.
    /// For auto-generated high-performance logging codes you should use only primitive types (complex object can be serialized to json, and treated as normal string in the log structure)
    /// </summary>
    [Fact]
    public async Task SeriLogTest()
    {
        var seriLogger = new LoggerConfiguration()
            .WriteTo.OpenTelemetry(options =>
            {
                // options.Endpoint = "http://localhost:4317"; // OTLP/gRPC endpoint of your collector
                // options.Protocol = Serilog.Sinks.OpenTelemetry.OtlpProtocol.Grpc; // or HttpProtobuf
                options.ResourceAttributes = new Dictionary<string, object>
                {
                    ["service.name"] = nameof(SeriLogTest),
                    ["service.version"] = "1.0.0"
                };
            })
            .Enrich.FromLogContext()
            .CreateLogger();
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog(seriLogger, true);
        });
        ILogger<StructuredLoggingExperiments> logger = loggerFactory.CreateLogger<StructuredLoggingExperiments>();
        
        logger.LogClaims("Without @", _SampleClaims);
        logger.LogClaimsAutoDestructuring("With @", _SampleClaims);
        logger.LogInformation("Normal LogInformation {@claims}", _SampleClaims);
    }
    
    [Fact]
    public async Task LogTraceId_Use_StandardLoggingTest()
    {
        const string myServiceName = nameof(LogTraceId_Use_StandardLoggingTest);
        /*
         * this traceProvider seems to do nothing. But without it, the traceId/spanId won't be populated in the logs
         * because the Activity.Current would always be null. ActivitySource.StartActivity only creates an Activity
         * if there’s a listener (e.g. this tracerProvider)
         */
        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddSource(nameof(StructuredLoggingExperiments)) // must match your ActivitySource name
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(myServiceName))
            .Build();

        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddOpenTelemetry(options =>
            {
                options.SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService(myServiceName)
                );
                options.IncludeScopes = true;
                options.IncludeFormattedMessage = true;
                options.AddOtlpExporter();
            });
        });
        ILogger<StructuredLoggingExperiments> logger = loggerFactory.CreateLogger<StructuredLoggingExperiments>();

        using var activity = ActivitySource.StartActivity("myactivity");
        logger.LogClaims("Without @", _SampleClaims);
        logger.LogClaimsAutoDestructuring("With @", _SampleClaims);
        logger.LogInformation("Normal LogInformation {@claims} {CurrentTraceId}", _SampleClaims, Activity.Current?.TraceId);    
    }
    
    [Fact]
    public async Task LogTraceId_Use_SerilogTest()
    {
        const string myServiceName = nameof(LogTraceId_Use_SerilogTest);
        /*
         * this traceProvider seems to do nothing. But without it, the traceId/spanId won't be populated in the logs
         * because the Activity.Current would always be null. ActivitySource.StartActivity only creates an Activity
         * if there’s a listener (e.g. this tracerProvider)
         */
        using var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddSource(nameof(StructuredLoggingExperiments)) // must match your ActivitySource name
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(myServiceName))
            .Build();

        var seriLogger = new LoggerConfiguration()
            .WriteTo.OpenTelemetry(options =>
            {
                // options.Endpoint = "http://localhost:4317"; // OTLP/gRPC endpoint of your collector
                // options.Protocol = Serilog.Sinks.OpenTelemetry.OtlpProtocol.Grpc; // or HttpProtobuf
                options.ResourceAttributes = new Dictionary<string, object>
                {
                    ["service.name"] = myServiceName,
                    ["service.version"] = "1.0.0"
                };
            })
            .Enrich.FromLogContext()
            .CreateLogger();
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddSerilog(seriLogger, true);
        });
        
        ILogger<StructuredLoggingExperiments> logger = loggerFactory.CreateLogger<StructuredLoggingExperiments>();

        using var activity = ActivitySource.StartActivity("myactivity");
        logger.LogClaims("Without @", _SampleClaims);
        logger.LogClaimsAutoDestructuring("With @", _SampleClaims);
        logger.LogInformation("Normal LogInformation {@claims} {CurrentTraceId}", _SampleClaims, Activity.Current?.TraceId);    
    }
}

public static partial class StructuredLoggingExperimentsSampleLogs
{
    [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "Normal. The person {person} claims {claims}")]
    public static partial void LogClaims(this ILogger logger, string person, Dictionary<string, string[]> claims);
    
    [LoggerMessage(EventId = 3, Level = LogLevel.Information, Message = "Auto destructuring. The person {person} claims {@claims}")]
    public static partial void LogClaimsAutoDestructuring(this ILogger logger, string person, Dictionary<string, string[]> claims);
}