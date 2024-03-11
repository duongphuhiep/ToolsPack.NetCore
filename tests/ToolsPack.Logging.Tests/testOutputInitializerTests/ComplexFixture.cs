using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using System;
using ToolsPack.Logging.Tests.lib;
using Xunit.Abstractions;

namespace ToolsPack.Logging.Tests
{
    /// <summary>
    /// This fixture has its own loggerFactory,
    /// we want to see all the log coming from this fixture's loggerFactory on the Test Unit Output
    /// </summary>
    public class ComplexFixture : IDisposable
    {
        public TestOutputInitializer TestOutputInitializer { get; init; } = new TestOutputInitializer();

        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<ComplexFixture> _logger;

        public ComplexFixture(IMessageSink messageSink)
        {
            var serilogLogger = new LoggerConfiguration()
                   .MinimumLevel.Verbose()
                   .WriteTo.TestOutput(messageSink)
                   .WriteTo.Seq("http://localhost:5341/")
                   .CreateLogger();

            _loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog(serilogLogger);
                builder.AddFilter<SerilogLoggerProvider>("Microsoft", LogLevel.Error);
                builder.AddFilter<SerilogLoggerProvider>("System", LogLevel.Error);
                builder.AddFilter<SerilogLoggerProvider>("App.Telemetry", LogLevel.Error);

                builder.AddProvider(new TestOutputLoggerProvider(TestOutputInitializer));
                builder.AddFilter("Microsoft", LogLevel.Error);
                builder.AddFilter("System", LogLevel.Error);
                builder.AddFilter("App.Telemetry", LogLevel.Error);
            });
            _logger = _loggerFactory.CreateLogger<ComplexFixture>();
            _logger.LogInformation("Fixture fixture started");
        }

        public void Dispose()
        {
            _logger.LogInformation("Fixture fixture finished");
        }

        public void LogFromFixture(string message)
        {
            _logger.LogInformation("Fixture scope: " + message);
        }
    }
}
