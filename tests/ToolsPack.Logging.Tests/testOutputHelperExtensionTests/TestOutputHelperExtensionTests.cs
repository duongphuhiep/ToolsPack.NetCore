using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using ToolsPack.Logging.Testing;
using Xunit;
using Xunit.Abstractions;

namespace ToolsPack.Logging.Tests
{
    public class TestOutputHelperExtensionTests : IDisposable, IClassFixture<WithMessageSinkFixture>
    {
        private static readonly string MSG_SETUP = "setup unit test";
        private static readonly string MSG_TEARDOWN = "teardown unit test";

        private readonly ILoggerFactory _loggerFactory;
        private readonly MockLogger _mockLogger;
        private readonly ILogger _logger;

        public TestOutputHelperExtensionTests(WithMessageSinkFixture sampleFixture, ITestOutputHelper testOutputHelper)
        {
            _mockLogger = Substitute.For<MockLogger>();
            _loggerFactory = testOutputHelper.CreateLoggerFactory(_mockLogger);
            _logger = _loggerFactory.CreateLogger<TestOutputHelperExtensionTests>();

            _logger.LogInformation(MSG_SETUP);
        }

        public void Dispose()
        {
            _logger.LogInformation(MSG_TEARDOWN);
        }

        [Theory]
        [InlineData("execute my unit test 1")]
        [InlineData("execute my unit test 2")]
        public void ExecuteMyUnitTest(string executionMessage)
        {
            //Act
            _logger.LogInformation(executionMessage);

            //Assert
            _mockLogger.Received(1).IsLogged(
                Arg.Any<LogLevel>(),
                Arg.Any<EventId>(),
                Arg.Any<Exception?>(),
                MSG_SETUP);
            _mockLogger.Received(1).IsLogged(
                Arg.Any<LogLevel>(),
                Arg.Any<EventId>(),
                Arg.Any<Exception?>(),
                executionMessage);

            //that there is no other logs than the above 2 log messages
            _mockLogger.DidNotReceive().IsLogged(
                Arg.Any<LogLevel>(),
                Arg.Any<EventId>(),
                Arg.Any<Exception?>(),
                Arg.Is<string>(msg => msg != MSG_SETUP && msg != executionMessage));
        }
    }
}
