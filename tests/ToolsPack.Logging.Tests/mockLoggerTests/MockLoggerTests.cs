using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ToolsPack.Logging.Testing;
using Xunit;

namespace ToolsPack.Logging.Tests;

using LogState = IReadOnlyList<KeyValuePair<string, object?>>;

public class MockLoggerTests
{
    private readonly MockLogger _logger = Substitute.For<MockLogger>();

    /// <summary>
    /// Example how to use mock logger to spy on normal log message
    /// </summary>
    [Fact]

    public void MockLoggerBasicTest()
    {
        //Act: make some normal & high performence logging 
        _logger.LogInformation("haha"); //normal log 
        _logger.LogError(new Exception("some exception"), "some error on {method}", "Greeting"); //normal log
        _logger.LogSendRequestInfo("toto"); //high perf logging
        _logger.LogGetResponseError(new Exception("Failed to do thing")); //high perf logging

        //Assert
        _logger.Received().IsLogged(
            Arg.Is(LogLevel.Information),
            Arg.Any<EventId>(),
            Arg.Any<Exception?>(),
            Arg.Is("haha"));

        _logger.Received().IsLogged(
            Arg.Is(LogLevel.Error),
            Arg.Any<EventId>(),
            Arg.Is<Exception?>(ex => ex.Message == "some exception"),
            Arg.Is<string>(s => s.Contains("some error on Greeting")));

        _logger.Received().IsLogged(
            Arg.Is(LogLevel.Information),
            Arg.Any<EventId>(),
            Arg.Any<Exception?>(),
            Arg.Is("Send request toto"));

        _logger.Received().IsLogged(
            Arg.Is(LogLevel.Error),
            Arg.Any<EventId>(),
            Arg.Is<Exception?>(ex => ex.Message == "Failed to do thing"),
            Arg.Is<string>(s => s.Contains("Received something wrong")));
    }

    /// <summary>
    /// Demontrate how to use mock logger to spy on complex structuring logs (where the message is object rather than text)
    /// </summary>
    [Fact]
    public void AssertLogStructuringSampleTest()
    {
        //Act: Make same Structuring logging
        _logger.LogStructuringSampleInfo(new SomePersonObject("Hiep", 18), new SomeOrderObject("#1", 30.5));

        _logger.Received().IsLogged(
            Arg.Is(LogLevel.Information),
            Arg.Any<EventId>(),
            Arg.Any<Exception?>(),
            Arg.Is<string>(msg => Regex.IsMatch(msg, "The person .* odered .*")),
            Arg.Is<LogState>(state => AssertLogStructuring(state)));
    }

    private static bool AssertLogStructuring(LogState state)
    {
        var person = (from s in state
                      where s.Key == "person"
                      select s.Value as SomePersonObject).First();

        var order = (from s in state
                     where s.Key == "order"
                     select s.Value as SomeOrderObject).First();

        return person.Name == "Hiep" && person.Age == 18 && order.Id == "#1" && order.Amount == 30.5;
    }

    /// <summary>
    /// Demontrate how to use mock logger in complex application with Dependency injection (an ASP.NET application for eg)
    /// </summary>
    [Fact]
    public void SimulateApplicationLoggingTest()
    {
        //Arrange a service provider similar to a ASP.NET app
        var diContainer = new ServiceCollection();
        diContainer.AddLogging(builder =>
        {
            builder.AddMockLogger(_logger);
        });
        var services = diContainer.BuildServiceProvider();

        //inject the ILogger to some service class of the Application
        var someAppLogger = services.GetRequiredService<ILogger<MockLoggerTests>>();

        //Act: application logs some messages
        someAppLogger.LogInformation("haha");

        //Assert that application logged the right message
        _logger.Received().IsLogged(
            Arg.Is(LogLevel.Information),
            Arg.Any<EventId>(),
            Arg.Any<Exception?>(),
            Arg.Is("haha"));
    }
}
