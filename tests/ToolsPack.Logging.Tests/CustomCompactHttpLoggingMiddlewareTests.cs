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

public class CustomCompactHttpLoggingMiddlewareTests
{
    private readonly VerifySettings _vsettings = new();
    private readonly FakeLogger<CustomCompactHttpLoggingMiddleware> _fakeLogger = new();
    private readonly MockHttpMessageHandler _mockHttp;
    
    public CustomCompactHttpLoggingMiddlewareTests()
    {
        _mockHttp = new MockHttpMessageHandler();
        _mockHttp.When(HttpMethod.Post,"https://api.example.com/user/42?metadata=off")
            .Respond("application/json", "{ 'id': 42, 'name': 'Alice' }");
        _mockHttp.When(HttpMethod.Post,"https://wrongly.com")
            .Throw(new HttpRequestException("Oups! Something wrong"));
        _vsettings.ScrubMembers<FakeLogRecord>(
            record => record.Timestamp /*ignore timestamp value in the snapshot */, 
            record => record.Message /* ignore the message string which contains the elapsed time */);
        _vsettings.ScrubMembers("ElapsedMilliseconds", "StackTrace");
    }
    
    /// <summary>
    /// This snapshot test would fail on the first run
    /// </summary>
    [Fact]
    public async Task FakeLoggerSnapshotTest()
    {
        CompactHttpLoggingMiddlewareConfig config = new()
        {
            LogLevelSelector = (method, uri, code, length, exception, milliseconds) => LogLevel.Warning,
            RequestBodyRedactor = (body, method, uri, code, length) => body.Replace("world", "country"),
            ResponseBodyRedactor = (body, method, uri, code, length) => body.Replace("Alice", "Peter"),
        };
        var httpClient = new HttpClient(new CustomCompactHttpLoggingMiddleware(_fakeLogger, config)
        {
            InnerHandler = _mockHttp
        });
        
        //ACT
        await httpClient.PostAsync("https://api.example.com/user/42?metadata=off", new StringContent("hello world"));
        await Assert.ThrowsAsync<HttpRequestException>(() => httpClient.PostAsync("https://wrongly.com", new StringContent("coucou")));
        
        //ASSERT
        var logsSnapshot = _fakeLogger.Collector.GetSnapshot();
        await Verifier.Verify(logsSnapshot, _vsettings);
    }
}