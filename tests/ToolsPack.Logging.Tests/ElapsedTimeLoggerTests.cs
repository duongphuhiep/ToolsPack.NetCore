using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Threading;
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
        public void LogTest()
        {
            var log = loggerFactory.CreateLogger<ElapsedTimeLoggerTests>();
            log.LogInformation("Normal log");
            using (ElapsedTimeLogger etl = ElapsedTimeLogger.Create(log, "toto", "beginContext", "endContext", "  "))
            {

                log.LogDebug("Normal log");
                Thread.Sleep(600);
                etl.LogInformation("ETL log 10");
                Thread.Sleep(20);

                using (etl.BeginScope("foo"))
                {
                    etl.LogDebug("ETL log 20");
                    Thread.Sleep(400);
                    etl.LogWarning("ETL log 30");
                    Thread.Sleep(500);
                }
            }
            log.LogInformation("Normal log");
        }

        [Fact()]
        public void NullLogTest()
        {
            var log = loggerFactory.CreateLogger<ElapsedTimeLoggerTests>();
            log.LogInformation("Normal log");
            using (ElapsedTimeLogger etl = ElapsedTimeLogger.Create(null, "toto", "beginContext", "endContext", "  "))
            {

                log.LogDebug("Normal log");
                Thread.Sleep(600);
                etl.LogInformation("ETL log 10");
                Thread.Sleep(20);

                using (etl.BeginScope("foo"))
                {
                    etl.LogDebug("ETL log 20");
                    Thread.Sleep(400);
                    etl.LogWarning("ETL log 30");
                    Thread.Sleep(500);
                }
            }
            log.LogInformation("Normal log");
        }
    }
}