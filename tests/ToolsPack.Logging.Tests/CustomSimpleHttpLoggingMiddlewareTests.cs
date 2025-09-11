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

public class CustomSimpleHttpLoggingMiddlewareTests
{
    private readonly VerifySettings _vsettings = new();
    private readonly FakeLogger<CustomSimpleHttpLoggingMiddleware> _fakeLogger = new();
    private readonly MockHttpMessageHandler _mockHttp;
    
    public CustomSimpleHttpLoggingMiddlewareTests()
    {
        _mockHttp = new MockHttpMessageHandler();
        _mockHttp.When(HttpMethod.Post,"https://api.example.com/user/42?metadata=off")
            .Respond("application/json", "{ 'id': 42, 'name': 'Alice' }");
        _mockHttp.When(HttpMethod.Post,"https://wrongly.com")
            .Throw(new HttpRequestException("Oups! Something wrong"));
        //ignore timestamp value in the snapshot
        _vsettings.ScrubMember<FakeLogRecord>(record => record.Timestamp);  
        _vsettings.ScrubMember("StackTrace");
    }
    
    /// <summary>
    /// This snapshot test would fail on the first run
    /// </summary>
    [Fact]
    public async Task FakeLoggerSnapshotTest()
    {
        SimpleHttpLoggingMiddlewareConfig config = new()
        {
            CorrelationIdGenerator = () => "fixedCorrelationIdForTest",
            RequestBodyRedactor = body => body.Replace("world", "country"),
            ResponseBodyRedactor = body => body.Replace("Alice", "Peter"),
            RequestBodyLogLevel = (method, uri) => LogLevel.Debug,
            RequestUriLogLevel = (method, uri) => LogLevel.Information,
            ResponseBodyLogLevel = (method, uri, statusCode) => LogLevel.Warning,
            ResponseStatusLogLevel = (method, uri, statusCode) => LogLevel.Error 
        };
        var httpClient = new HttpClient(new CustomSimpleHttpLoggingMiddleware(_fakeLogger, config)
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