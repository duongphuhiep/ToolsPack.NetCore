using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using Xunit;

namespace ToolsPack.Logging.Tests;

public class FakeLoggerTests
{
    private readonly FakeLogger _logger = new FakeLogger<FakeLoggerTests>();

    /// <summary>
    ///     Demontrate how to use mock logger to spy on normal log message
    /// </summary>
    [Fact]
    public void FakeLoggerBasicTest()
    {
        //Act: make some normal & high performence logging 
        _logger.LogInformation("haha"); //normal log 
        _logger.LogError(new Exception("some exception"), "some error on {method}", "Greeting"); //normal log
        _logger.LogSendRequestInfo("toto"); //high perf logging
        _logger.LogGetResponseError(new Exception("Failed to do thing")); //high perf logging

        //Assert
        var collector = _logger.Collector;
        var logs = collector.GetSnapshot();

        Assert.Equal(LogLevel.Information, logs[0].Level);
        Assert.Equal("haha", logs[0].Message);
        Assert.Null(logs[0].Exception);

        Assert.Equal(LogLevel.Error, logs[1].Level);
        Assert.Contains("some error on Greeting", logs[1].Message);
        Assert.Equal("some exception", logs[1].Exception?.Message);

        Assert.Equal(LogLevel.Information, logs[2].Level);
        Assert.Contains("Send request toto", logs[2].Message);
        Assert.Null(logs[2].Exception);

        Assert.Equal(LogLevel.Error, logs[3].Level);
        Assert.Contains("Received something wrong", logs[3].Message);
        Assert.Equal("Failed to do thing", logs[3].Exception?.Message);
    }
}