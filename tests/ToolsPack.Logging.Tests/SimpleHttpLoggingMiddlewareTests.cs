using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using RichardSzalay.MockHttp;
using Serilog;
using Serilog.Formatting.Json;
using VerifyTests;
using VerifyXunit;
using Xunit;

namespace ToolsPack.Logging.Tests;

public class SimpleHttpLoggingMiddlewareTests
{
    private readonly VerifySettings _vsettings = new();
    private readonly FakeLogger<SimpleHttpLoggingMiddleware> _fakeLogger = new();
    private readonly MockHttpMessageHandler _mockHttp;
    
    public SimpleHttpLoggingMiddlewareTests()
    {
        _mockHttp = new MockHttpMessageHandler();
        _mockHttp.When("https://api.example.com/user/42?metadata=off")
            .Respond("application/json", "{ 'id': 42, 'name': 'Alice' }");
        
        //ignore timestamp value in the snapshot
        _vsettings.ScrubMember<FakeLogRecord>(record => record.Timestamp);  
        _vsettings.ScrubMembers("HttpRequestCorrelationId", "StackTrace");
    }
    
    /// <summary>
    /// Run this test to see the logs in the VictoriaLogs. Use the docker-compose.yaml in this project to start VictoriaLogs.
    /// See also: https://github.com/duongphuhiep/learn-otel-collector
    /// </summary>
    [Fact]
    public async Task LogToOtelTest()
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddOpenTelemetry(options =>
            {
                options.SetResourceBuilder(
                    ResourceBuilder.CreateDefault()
                        .AddService("ToolsPack.Logging.Tests")
                );
                options.IncludeScopes = true;
                options.AddOtlpExporter();
            });
        });
        var logger = loggerFactory.CreateLogger<SimpleHttpLoggingMiddleware>();
        var httpClient = new HttpClient(new SimpleHttpLoggingMiddleware(logger)
        {
            InnerHandler = _mockHttp
        });
        //ACT
        await httpClient.GetAsync("https://api.example.com/user/42?metadata=off");
    }
    
    /// <summary>
    /// Run this test to see the logs in the file.
    /// </summary>
    [Fact]
    public async Task LogToFileTest()
    {
        var seriLogger = new LoggerConfiguration()
            .WriteTo.File(new JsonFormatter(),  $"{Path.GetTempPath()}/{nameof(SimpleHttpLoggingMiddlewareTests)}_{nameof(LogToFileTest)}.log", fileSizeLimitBytes: 1024*1024)
            .CreateLogger();
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.ClearProviders();
            builder.AddSerilog(seriLogger, true);
        });
        var logger = loggerFactory.CreateLogger<SimpleHttpLoggingMiddleware>();
        var httpClient = new HttpClient(new SimpleHttpLoggingMiddleware(logger)
        {
            InnerHandler = _mockHttp
        });
        //ACT
        await httpClient.GetAsync("https://api.example.com/user/42?metadata=off");
    }
    
    /// <summary>
    /// This snapshot test would fail on the first run
    /// </summary>
    [Fact]
    public async Task FakeLoggerSnapshotTest()
    {
        var httpClient = new HttpClient(new SimpleHttpLoggingMiddleware(_fakeLogger)
        {
            InnerHandler = _mockHttp
        });
        
        //ACT
        await httpClient.GetAsync("https://api.example.com/user/42?metadata=off");
        
        //ASSERT
        var logsSnapshot = _fakeLogger.Collector.GetSnapshot();
        await Verifier.Verify(logsSnapshot, _vsettings);
    }
}