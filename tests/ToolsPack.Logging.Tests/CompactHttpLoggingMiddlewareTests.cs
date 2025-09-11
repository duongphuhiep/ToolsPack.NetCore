using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using RichardSzalay.MockHttp;
using VerifyTests;
using VerifyXunit;
using Xunit;

namespace ToolsPack.Logging.Tests;

public class CompactHttpLoggingMiddlewareTests
{
    private readonly VerifySettings _vsettings = new();
    private readonly FakeLogger<CompactHttpLoggingMiddleware> _fakeLogger = new();
    private readonly MockHttpMessageHandler _mockHttp;
    
    public CompactHttpLoggingMiddlewareTests()
    {
        _mockHttp = new MockHttpMessageHandler();
        _mockHttp.When("https://api.example.com/user/42?metadata=off")
            .Respond("application/json", "{ 'id': 42, 'name': 'Alice' }");
        _vsettings.ScrubMembers<FakeLogRecord>(
            record => record.Timestamp /*ignore timestamp value in the snapshot */, 
            record => record.Message /* ignore the message string which contains the elapsed time */);
        _vsettings.ScrubMembers("ElapsedMilliseconds", "StackTrace");
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
        var logger = loggerFactory.CreateLogger<CompactHttpLoggingMiddleware>();
        var httpClient = new HttpClient(new CompactHttpLoggingMiddleware(logger)
        {
            InnerHandler = _mockHttp
        });
        await httpClient.GetAsync("https://api.example.com/user/42?metadata=off");
    }
    
    /// <summary>
    /// This snapshot test would fail on the first run
    /// </summary>
    [Fact]
    public async Task FakeLoggerSnapshotTest()
    {
        var httpClient = new HttpClient(new CompactHttpLoggingMiddleware(_fakeLogger)
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