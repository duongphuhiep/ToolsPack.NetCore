using Microsoft.Extensions.Logging;
using System;
using Xunit;
using Xunit.Abstractions;

namespace ToolsPack.Logging.Tests;

public class Tests : IDisposable
{
    private ILoggerFactory _loggerFactory;
    private ILogger _logger;
    public Tests(ITestOutputHelper testOutputHelper)
    {
        new TestOutputTextWriter(testOutputHelper);

        _loggerFactory = LoggerFactory.Create(builder =>
            builder.SetMinimumLevel(LogLevel.Debug)
            .AddConsole()
        );
        _logger = _loggerFactory.CreateLogger<Tests>();
        _logger.LogInformation("Setup");
    }

    public void Dispose()
    {
        _logger.LogInformation("TearDown");
    }

    [Fact]
    public void Test1()
    {
        Console.Write("oOoOoO");
        _logger.LogInformation("Test1");
    }
}
