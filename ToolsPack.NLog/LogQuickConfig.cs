using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using N = NLog;

namespace ToolsPack.NLog
{
    /// <summary>
    /// Quick config the NLog for most case. You will no longer need to add a nlog.config file in your small project / unit test just for basic logging
    /// </summary>
    public static class LogQuickConfig
    {
        /// <summary>
        /// The default setting for NLog to serialize Object to json. You should also setup your Newtonsoft.Json to use the same setting. For example:
        /// JsonConvert.DefaultSettings = () => DefaultJsonSerializerSettings;
        /// </summary>
        public static readonly JsonSerializerSettings DefaultJsonSerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ReferenceLoopHandling = ReferenceLoopHandling.Ignore, Formatting = Formatting.Indented };

        /// <summary>
        /// The default messages layout
        /// </summary>
        public static readonly string DefaultPattern = defaultPattern;

        private const string defaultPattern = "${longdate} ${level:uppercase=true} ${message}${exception:format=ToString}  [${logger:shortname=true}]";
        private const string defaultPatternConsole = "${time} ${level:uppercase=true} ${message}${exception:format=ToString}  [${logger:shortname=true}]";

        /// <summary>
        /// Setup NLog to write to file
        /// </summary>
        public static N.Config.LoggingConfiguration SetupFile(string filePath, string pattern = defaultPattern, JsonSerializerSettings jsonSerializerSettings = null)
        {
            N.Config.LoggingConfiguration config = CreateConfigFile(filePath, pattern);
            return ApplyConfig(config, jsonSerializerSettings);
        }

        /// <summary>
        /// Setup NLog to write to console
        /// </summary>
        public static N.Config.LoggingConfiguration SetupConsole(string pattern = defaultPatternConsole, JsonSerializerSettings jsonSerializerSettings = null)
        {
            N.Config.LoggingConfiguration config = CreateConfigConsole(pattern);
            return ApplyConfig(config, jsonSerializerSettings);
        }

        /// <summary>
        /// Setup NLog to write to both file and console
        /// </summary>
        public static N.Config.LoggingConfiguration SetupFileAndConsole(string filePath, string pattern = defaultPattern, JsonSerializerSettings jsonSerializerSettings = null)
        {
            N.Config.LoggingConfiguration config = CreateConfigFileAndConsole(filePath, pattern);
            return ApplyConfig(config, jsonSerializerSettings);
        }

        /// <summary>
        /// Apply config and set the default json serializer
        /// </summary>
        private static N.Config.LoggingConfiguration ApplyConfig(N.Config.LoggingConfiguration config, JsonSerializerSettings jsonSerializerSettings)
        {
            // Apply config
            N.LogManager.Configuration = config;

            //make NLog use Newtonsoft with the given jsonSerializerSettings to serialize object for structured logging
            UseNewtonsoftJson(jsonSerializerSettings);

            return config;
        }

        /// <summary>
        /// make NLog use Newtonsoft with the given settings to serialize objects (structured logging)
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static JsonNetSerializer UseNewtonsoftJson(JsonSerializerSettings settings=null)
        {
            if (settings == null)
            {
                settings = DefaultJsonSerializerSettings;
            }
            var serializer = new JsonNetSerializer(settings);
            N.Config.ConfigurationItemFactory.Default.JsonConverter = serializer;
            return serializer;
        }

        /// <summary>
        /// Create NLog config to log in 2 targets: file and console
        /// </summary>
        public static N.Config.LoggingConfiguration CreateConfigFileAndConsole(string filePath, string pattern)
        {
            var config = new N.Config.LoggingConfiguration();
            N.Targets.FileTarget targetFile = GetFileTarget(filePath, pattern);
            N.Targets.ConsoleTarget targetConsole = GetConsoleTarget(pattern);
            config.AddRule(N.LogLevel.Debug, N.LogLevel.Fatal, targetFile);
            config.AddRule(N.LogLevel.Debug, N.LogLevel.Fatal, targetConsole);
            config.DefaultCultureInfo = CultureInfo.InvariantCulture;
            return config;
        }

        /// <summary>
        /// Create NLog config to log in rolling file
        /// </summary>
        public static N.Config.LoggingConfiguration CreateConfigFile(string filePath, string pattern)
        {
            var config = new N.Config.LoggingConfiguration();
            N.Targets.FileTarget targetFile = GetFileTarget(filePath, pattern);
            config.AddRule(N.LogLevel.Debug, N.LogLevel.Fatal, targetFile);
            config.DefaultCultureInfo = CultureInfo.InvariantCulture;
            return config;
        }

        /// <summary>
        /// Create NLog config to log in console
        /// </summary>
        public static N.Config.LoggingConfiguration CreateConfigConsole(string pattern)
        {
            var config = new N.Config.LoggingConfiguration();
            N.Targets.ConsoleTarget targetConsole = GetConsoleTarget(pattern);
            config.AddRule(N.LogLevel.Debug, N.LogLevel.Fatal, targetConsole);
            config.DefaultCultureInfo = CultureInfo.InvariantCulture;
            return config;
        }

        private static N.Targets.ConsoleTarget GetConsoleTarget(string pattern)
        {
            return new N.Targets.ConsoleTarget("logconsole")
            {
                Layout = pattern
            };
        }

        private static N.Targets.FileTarget GetFileTarget(string filePath, string pattern)
        {
            return new N.Targets.FileTarget("logfile")
            {
                FileName = filePath,
                ArchiveNumbering = N.Targets.ArchiveNumberingMode.DateAndSequence,
                ArchiveEvery = N.Targets.FileArchivePeriod.Day,
                KeepFileOpen = true,
                ArchiveOldFileOnStartup = true,
                EnableArchiveFileCompression = true,
                Encoding = Encoding.UTF8,
                Layout = pattern
            };
        }
    }
}
