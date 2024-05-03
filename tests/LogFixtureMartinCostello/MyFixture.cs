using MartinCostello.Logging.XUnit;
using Microsoft.Extensions.Logging;
using ToolsPack.Logging.TestingTools;
using Xunit.Abstractions;

namespace ToolsPack.Logging.Tests;

public class MyFixture : IDisposable, ITestOutputHelperAccessor
{
    private readonly ILogger<MyFixture> _logger;
    public ILoggerFactory LoggerFactory { get; private set; }
    public ITestOutputHelper? OutputHelper { get; set; }

    private readonly IDisposable? _fixtureScope;

    public MyFixture(IMessageSink diagnosticMessageSink)
    {
        LoggerFactory = diagnosticMessageSink.CreateLoggerFactory(this, opt =>
        {
            opt.IncludeScopes = true;
        });
        _logger = LoggerFactory.CreateLogger<MyFixture>();
        _fixtureScope = _logger.BeginScope("My fixture scope");
        _logger.LogInformation("Start MyFixture, we won't see this diagnostic messages in the Test Output");
    }

    public void CallApplicationCodes()
    {
        _logger.LogInformation("Application codes is called from fixture, and we see this diagnostic messages in the Test Output, Yay!");
    }

    public void Dispose()
    {
        _logger.LogInformation("End MyFixture, we won't see this diagnostic messages in the Test Output");
        _fixtureScope?.Dispose();
    }
}
