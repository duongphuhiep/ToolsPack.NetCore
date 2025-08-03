using System;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace ToolsPack.Logging.Tests;

public class UnitTest4 : IDisposable, IClassFixture<Fixture41>
{
    private readonly ILogger _logger;
    private readonly ILoggerFactory _loggerFactory;

    public UnitTest4(ITestOutputHelper testOutputHelper)
    {
        StreamConsole.Setup(testOutputHelper);
        _loggerFactory = LoggerFactory.Create(builder =>
            builder.SetMinimumLevel(LogLevel.Debug)
                .AddConsole()
        );
        _logger = _loggerFactory.CreateLogger<UnitTest4>();
        _logger.LogInformation("Setup");
    }

    public void Dispose()
    {
        _logger.LogInformation("TearDown");
    }

    [Fact(Skip = "This is a failed experiment")]
    public void Test1()
    {
        Console.Write("oOoOoO");
        _logger.LogInformation("UnitTest4");
    }
}