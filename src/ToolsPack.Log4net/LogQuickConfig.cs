using System;
using System.Reflection;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;
using log4net.Repository;

namespace ToolsPack.Log4net
{
    public static class LogQuickConfig
    {
        private static readonly ILoggerRepository DefaultRepository = LogManager.GetRepository(Assembly.GetCallingAssembly());
        private const string DefaultPattern = "%date{HH:mm:ss,fff} [%-5level] %message  [%logger{1}:%line]%newline";

        /// <summary>
        /// Create a pattern which log a thread property value in all log message
        /// Example
        /// ThreadContext.Properties["SessionId"] = "FXA_36985";
        /// var patternWithSessionId = CreatePattern("SessionId");
        /// if the patternWithSessionId is use in the config log4net, so the "FXA_36985" will be displayed
        /// on every message comming from the Thread that set the property "SessionId"
        /// </summary>
        /// <param name="threadContextPropertyName"></param>
        /// <returns></returns>
        public static string CreatePattern(string threadContextPropertyName)
        {
            return "%date{HH:mm:ss,fff} [%-5level] [%property{" + threadContextPropertyName +
            "}] %message  [%logger{1}]%newline";
        }

        /// <summary>
        /// No date time, no logger name (fit to display in a console)
        /// </summary>
        public static string GetSimplePattern(string threadContextPropertyName = null)
        {
            if (string.IsNullOrEmpty(threadContextPropertyName))
            {
                return "[%-5level] %message%newline";
            }
            return "[%-5level] [%property{" + threadContextPropertyName +
            "}] %message%newline";
        }

        public static void SetupConsole(string pattern = DefaultPattern, ILoggerRepository repository = null)
        {
            var layout = new PatternLayout(pattern);
            var appender = new ConsoleAppender
            {
                Layout = layout,
                Threshold = Level.Debug
            };
            if (repository == null)
            {
                repository = DefaultRepository;
            }
            BasicConfigurator.Configure(repository, appender);
        }

        public static void SetupFile(string filePath, string pattern = DefaultPattern, ILoggerRepository repository = null)
        {
            var layout = new PatternLayout(pattern);
            var appender = new FileAppender()
            {
                Layout = layout,
                Threshold = Level.Debug,
                File = filePath,
            };
            if (repository == null)
            {
                repository = DefaultRepository;
            }
            BasicConfigurator.Configure(repository, appender);
        }
    }
}
