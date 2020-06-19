using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json;
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
        public static readonly JsonSerializerOptions DefaultJsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true, MaxDepth = 10, IgnoreNullValues = true };

        /// <summary>
        /// The default messages layout
        /// </summary>
        public static readonly string DefaultPattern = defaultPattern;

        private const string defaultPattern = "${longdate} ${pad:padding=5:inner=${level:uppercase=true}} ${message}${exception:format=ToString}  [${logger:shortname=true}]";
        private const string defaultPatternConsole = "${time} ${pad:padding=5:inner=${level:uppercase=true}} ${message}${exception:format=ToString}  [${logger:shortname=true}]";

        /// <summary>
        /// Setup NLog to write to file
        /// </summary>
        public static N.Config.LoggingConfiguration SetupFile(string filePath, string pattern = defaultPattern)
        {
            N.Config.LoggingConfiguration config = CreateConfigFile(filePath, pattern);
            return ApplyConfig(config);
        }

        /// <summary>
        /// Setup NLog to write to console
        /// </summary>
        public static N.Config.LoggingConfiguration SetupConsole(string pattern = defaultPatternConsole)
        {
            N.Config.LoggingConfiguration config = CreateConfigConsole(pattern);
            return ApplyConfig(config);
        }

        /// <summary>
        /// Setup NLog to write to both file and console
        /// </summary>
        public static N.Config.LoggingConfiguration SetupFileAndConsole(string filePath, string pattern = defaultPattern)
        {
            N.Config.LoggingConfiguration config = CreateConfigFileAndConsole(filePath, pattern);
            return ApplyConfig(config);
        }

        /// <summary>
        /// Apply config and set the default json serializer
        /// </summary>
        private static N.Config.LoggingConfiguration ApplyConfig(N.Config.LoggingConfiguration config)
        {
            // Apply config
            N.LogManager.Configuration = config;
            return config;
        }

        /// <summary>
        /// make NLog use Newtonsoft with the given settings to serialize objects (structured logging)
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static NewtonsoftJsonSerializer UseNewtonsoftJson(JsonSerializerSettings settings=null)
        {
            if (settings == null)
            {
                settings = DefaultJsonSerializerSettings;
            }
            var serializer = new NewtonsoftJsonSerializer(settings);
            N.Config.ConfigurationItemFactory.Default.JsonConverter = serializer;
            return serializer;
        }

        /// <summary>
        /// make NLog use System.Text.Json with the given settings to serialize objects (structured logging)
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static MicrosoftJsonSerializer UseMicrosoftJson(JsonSerializerOptions options = null)
        {
            if (options == null)
            {
                options = DefaultJsonSerializerOptions;
            }
            var serializer = new MicrosoftJsonSerializer(options);
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
            config.AddRule(N.LogLevel.Trace, N.LogLevel.Fatal, targetFile);
            config.AddRule(N.LogLevel.Trace, N.LogLevel.Fatal, targetConsole);
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
            config.AddRule(N.LogLevel.Trace, N.LogLevel.Fatal, targetFile);
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
            config.AddRule(N.LogLevel.Trace, N.LogLevel.Fatal, targetConsole);
            config.DefaultCultureInfo = CultureInfo.InvariantCulture;
            return config;
        }

        public static N.Targets.ConsoleTarget GetConsoleTarget(string pattern)
        {
            return new N.Targets.ConsoleTarget("logconsole")
            {
                Layout = pattern
            };
        }

        public static N.Targets.FileTarget GetFileTarget(string filePath, string pattern)
        {
            return new N.Targets.FileTarget("logfile")
            {
                FileName = filePath,
                ArchiveNumbering = N.Targets.ArchiveNumberingMode.DateAndSequence,
                ArchiveEvery = N.Targets.FileArchivePeriod.Day,
                KeepFileOpen = true,
                OpenFileCacheTimeout = 30,
                ConcurrentWrites = true,
                ArchiveOldFileOnStartup = false,
                EnableArchiveFileCompression = true,
                Encoding = Encoding.UTF8,
                Layout = pattern
            };
        }
    }
}
