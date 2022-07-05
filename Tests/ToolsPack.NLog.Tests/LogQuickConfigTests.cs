using Microsoft.VisualStudio.TestTools.UnitTesting;
using N = NLog;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace ToolsPack.NLog.Tests
{
    [TestClass()]
    public class LogQuickConfigTests
    {
        private readonly N.ILogger _nlog = N.LogManager.GetCurrentClassLogger();
        private ILoggerFactory _loggerFactory;

        [TestMethod()]
        public void SetupFileAndConsoleTest()
        {
            var conf = LogQuickConfig.SetupFileAndConsole("./logs/LogQuickConfigTests.log");
            _loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddNLog(conf);
                builder.SetMinimumLevel(LogLevel.Trace);
            });

            var someObject = new {abool = true, astring = "string", aint = 12};

            var mlog = _loggerFactory.CreateLogger<LogQuickConfigTests>();
            mlog.LogTrace("trace microsoft logger {SomeObject}", someObject);
            mlog.LogDebug("debug microsoft logger {SomeObject}", someObject);
            mlog.LogInformation("info microsoft logger {SomeObject}", someObject);

            _nlog.Trace("trace nlog {SomeObject}", someObject);
            _nlog.Debug("debug nlog {SomeObject}", someObject);
            _nlog.Info("info nlog {SomeObject}", someObject);
        }

        [TestMethod()]
        public void UseMicrosoftJsonTest()
        {
            var conf = LogQuickConfig.SetupFileAndConsole("./logs/LogQuickConfigTests.log");

            _loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddNLog(conf);
                builder.SetMinimumLevel(LogLevel.Trace);
            });

            var someObject = new {AboOl = true, aString = "string", AiNt = 12, anull = (string) null};

            var mlog = _loggerFactory.CreateLogger<LogQuickConfigTests>();

            mlog.LogTrace("trace microsoft logger default = {@SomeObject}", someObject);

            LogQuickConfig.UseMicrosoftJson();
            mlog.LogTrace("trace microsoft logger msjson  = {@SomeObject}", someObject);

            LogQuickConfig.UseNewtonsoftJson();
            mlog.LogTrace("trace microsoft logger newton  = {@SomeObject}", someObject);
        }
    }
}