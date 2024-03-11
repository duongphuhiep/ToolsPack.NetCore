using Microsoft.Extensions.Logging;
using System;
using Xunit;
using Xunit.Abstractions;

namespace ToolsPack.Logging.Tests
{
    public class SampleUnitTest22 : IDisposable, IClassFixture<ComplexFixture>, IClassFixture<WithMessageSinkFixture>
    {
        private static readonly string MSG_SETUP = "setup unit test";
        private static readonly string MSG_TEARDOWN = "teardown unit test";

        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger _logger;
        private readonly ComplexFixture _complexFixture;

        public SampleUnitTest22(ComplexFixture complexFixture, ITestOutputHelper testOutputHelper)
        {
            _complexFixture = complexFixture;
            _loggerFactory = testOutputHelper.CreateLoggerFactory();
            _logger = _loggerFactory.CreateLogger<TestOutputHelperExtensionTests>();
            _logger.LogInformation(MSG_SETUP);
            _complexFixture.TestOutputInitializer.Setup(testOutputHelper);
        }

        public void Dispose()
        {
            _logger.LogInformation(MSG_TEARDOWN);
        }

        [Theory]
        [InlineData("execute my unit test 1")]
        [InlineData("execute my unit test 2")]
        [InlineData("execute my unit test 3")]
        public void ExecuteMyUnitTest(string executionMessage)
        {
            _logger.LogInformation("UnitTest scope: " + executionMessage);
            _complexFixture.LogFromFixture("hello 22: " + executionMessage);
        }
    }
}
