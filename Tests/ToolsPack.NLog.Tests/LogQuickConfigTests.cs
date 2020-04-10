using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToolsPack.NLog;
using N = NLog;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;

namespace ToolsPack.NLog.Tests
{
    [TestClass()]
    public class LogQuickConfigTests
    {
        private N.ILogger nlog = N.LogManager.GetCurrentClassLogger();
        private ILoggerFactory loggerFactory;

        [TestMethod()]
        public void SetupFileAndConsoleTest()
        {
            var conf = LogQuickConfig.SetupFileAndConsole("./logs/LogQuickConfigTests.log");
            loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddNLog(conf);
                builder.SetMinimumLevel(LogLevel.Trace);
            });

            var someObject = new { abool = true, astring = "string", aint = 12 };

            var mlog = loggerFactory.CreateLogger<LogQuickConfigTests>();
            mlog.LogTrace("trace microsoft logger {someObject}", someObject);
            mlog.LogDebug("debug microsoft logger {someObject}", someObject);
            mlog.LogInformation("info microsoft logger {someObject}", someObject);

            nlog.Trace("trace nlog {someObject}", someObject);
            nlog.Debug("debug nlog {someObject}", someObject);
            nlog.Info("info nlog {someObject}", someObject);
        }
    }
}