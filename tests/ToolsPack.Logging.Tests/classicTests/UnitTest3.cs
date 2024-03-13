using Microsoft.Extensions.Logging;
using System;
using Xunit;
using Xunit.Abstractions;

namespace ToolsPack.Logging.Tests;

public class UnitTest3 : IDisposable, IClassFixture<Fixture3>
{
    private ILoggerFactory _loggerFactory;
    private ILogger _logger;

    public UnitTest3(ITestOutputHelper testOutputHelper)
    {
        _loggerFactory = LoggerFactory.Create(builder =>
            builder.SetMinimumLevel(LogLevel.Debug)
            .AddConsole()
        );
        _logger = _loggerFactory.CreateLogger<UnitTest3>();
        _logger.LogInformation("Setup");
    }

    public void Dispose()
    {
        _logger.LogInformation("TearDown");
    }

    [Fact]
    public void Test1()
    {
        Console.Write("My Test 1");
        _logger.LogInformation("Test1");
    }
}
