using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using ToolsPack.NLog;
using Xunit;

namespace ToolsPack.Logging.Tests
{
    public class ElapsedTimeLoggerTests
    {
        public readonly ILoggerFactory loggerFactory = LoggerFactory.Create(c =>
        {
            c.SetMinimumLevel(LogLevel.Trace);
            c.ClearProviders();
            c.AddNLog(LogQuickConfig.SetupFileAndConsole("./log/ElapsedTimeLoggerTests.log"));
        });

        [Fact()]
        public async Task LogTest()
        {
            var log = loggerFactory.CreateLogger<ElapsedTimeLoggerTests>();
            log.LogInformation("Normal log");
            using (ElapsedTimeLogger etl = ElapsedTimeLogger.Create(log, "toto", "beginContext", "endContext", "  "))
            {

                log.LogDebug("Normal log");
                await Task.Delay(600).ConfigureAwait(true);
                etl.LogInformation("ETL log 10");
                await Task.Delay(20).ConfigureAwait(true);

                using (etl.BeginScope("foo"))
                {
                    etl.LogDebug("ETL log 20");
                    await Task.Delay(400).ConfigureAwait(true);
                    etl.LogWarning("ETL log 30");
                    await Task.Delay(500).ConfigureAwait(true);
                }
            }
            log.LogInformation("Normal log");
        }

        [Fact()]
        public async Task NullLogTest()
        {
            var log = loggerFactory.CreateLogger<ElapsedTimeLoggerTests>();
            log.LogInformation("Normal log");
            using (ElapsedTimeLogger etl = ElapsedTimeLogger.Create(NullLogger.Instance, "toto", "beginContext", "endContext", "  "))
            {

                log.LogDebug("Normal log");
                await Task.Delay(600).ConfigureAwait(true);
                etl.LogInformation("ETL log 10");
                await Task.Delay(20).ConfigureAwait(true);

                using (etl.BeginScope("foo"))
                {
                    etl.LogDebug("ETL log 20");
                    await Task.Delay(400).ConfigureAwait(true);
                    etl.LogWarning("ETL log 30");
                    await Task.Delay(500).ConfigureAwait(true);
                }
            }
            log.LogInformation("Normal log");
        }
    }
}