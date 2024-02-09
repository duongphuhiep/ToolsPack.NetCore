using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using ToolsPack.String;
using static ToolsPack.String.StopwatchDisplayer;

namespace ToolsPack.Logging
{
    /// <summary>
    /// Micro benchmark a block of code, add elapsed time in each log message and the total elapsed time of the code block
    /// 
    ///            private static readonly ILogger Log = LogManager.GetLogger(typeof(MyClass));
    /// 
    ///            using (var etw = ElapsedTimeWatcher.Create(Log, "checkIntraday"))
    ///            {
    ///                Thread.Sleep(100);
    ///                etw.LogDebug("step 1");
    /// 
    ///                Thread.Sleep(200);
    ///                etw.LogDebug"step 2");
    /// 
    ///                Thread.Sleep(300);
    ///                etw.LogInformation("final step)");
    /// 
    ///                Thread.Sleep(400);
    ///            }
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Localization is fixed US-EN")]
    public sealed class ElapsedTimeLogger : IDisposable, ILogger
    {
        private bool _autoJumpContext;
        private int _autoJumpContextToInfo = 5;
        private int _autoJumpContextToWarning = 10;
        private bool _autoJump;
        private int _autoJumpToInfo = 2;
        private int _autoJumpToWarning = 5;
        private TimeUnit? _timeUnit;
        private LogLevel _startLogLevel = LogLevel.Debug;
        private LogLevel _endLogLevel = LogLevel.Information;
        private readonly ILogger _log;
        private readonly Stopwatch _scopeSw;
        private readonly Stopwatch _unitarySw;
        private readonly string? _scopeId;
        private readonly string? _startContext;
        private readonly string? _endContext;

        private ElapsedTimeLogger(ILogger log, string scopeId, string? startContext, string? endContext, string? spaceBeforeLog)
        {
            _log = log;

            if (string.IsNullOrEmpty(spaceBeforeLog))
            {
                _scopeId = scopeId;
                _startContext = startContext;
                _endContext = endContext;
            }
            else
            {
                _scopeId = spaceBeforeLog + scopeId;
                _startContext = spaceBeforeLog + startContext;
                _endContext = spaceBeforeLog + endContext;
            }

            LogBeginMessage();
            _scopeSw = Stopwatch.StartNew();
            _unitarySw = Stopwatch.StartNew();
        }

        #region Fluent API

        /// <summary>
        /// Decorate (or wrap) the log (input argument) to be a ElapsedTimeLogger
        /// </summary>
        /// <param name="log">the log to be wrap</param>
        /// <param name="scopeId">the prefix of all log message</param>
        /// <param name="beginContext">message log when the logger is created</param>
        /// <param name="endContext">message log when the logger is disposed</param>
        /// <param name="spaceBeforeLog">the margin</param>
        /// <returns></returns>
        public static ElapsedTimeLogger Create(ILogger log, string scopeId, string? beginContext = null, string? endContext = null,
            string? spaceBeforeLog = null)
        {
            if (string.IsNullOrEmpty(beginContext))
            {
                beginContext = scopeId;
            }
            if (string.IsNullOrEmpty(endContext))
            {
                endContext = beginContext != null && beginContext.Length < 256 ? beginContext : scopeId;
            }
            return new ElapsedTimeLogger(log, scopeId, beginContext, endContext, spaceBeforeLog);
        }

        /// <summary>
        /// For the last log (the total elpased time log)
        /// The log level automaticly jump up to INFO or WARN if the elapsed time exceed the threshold
        /// </summary>
        public ElapsedTimeLogger AutoJumpLastLog(int miliSecondToInfo = 5, int miliSecondToWarning = 10)
        {
            _autoJumpContext = true;
            _autoJumpContextToInfo = miliSecondToInfo;
            _autoJumpContextToWarning = miliSecondToWarning;
            return this;
        }

        /// <summary>
        /// The log level automaticly jump up to INFO or WARN if the elapsed time exceed the threshold
        /// </summary>
        public ElapsedTimeLogger AutoJump(int miliSecondToInfo = 2, int miliSecondToWarning = 5)
        {
            _autoJump = true;
            _autoJumpToInfo = miliSecondToInfo;
            _autoJumpToWarning = miliSecondToWarning;
            return this;
        }

        /// <summary>
        /// Set log level of the last message (on disposal)
        /// </summary>
        /// <param name="level">level parameter on LevelEnd</param>
        public ElapsedTimeLogger LevelEnd(LogLevel level)
        {
            _endLogLevel = level;
            return this;
        }

        /// <summary>
        /// The log level on start
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public ElapsedTimeLogger LevelBegin(LogLevel level)
        {
            _startLogLevel = level;
            return this;
        }

        /// <summary>
        /// force a TimeUnit for the benchmark, if not set
        /// </summary>
        /// <param name="timeUnit">timeUnit force to use</param>
        /// <returns></returns>
        public ElapsedTimeLogger TimeUnit(TimeUnit timeUnit)
        {
            _timeUnit = timeUnit;
            return this;
        }

        #endregion

        /// <summary>
        /// restart the internal scope stopwatch
        /// </summary>
        public void RestartScopeStopwatch()
        {
            _scopeSw.Stop();
            _scopeSw.Reset();
            _scopeSw.Start();
        }

        /// <summary>
        /// Restart the unitary stopwatch
        /// </summary>
        public void Restart()
        {
            _unitarySw.Stop();
            _unitarySw.Reset();
            _unitarySw.Start();
        }

        private LogLevel? GetLevel()
        {
            if (_scopeSw.ElapsedMilliseconds >= _autoJumpContextToWarning)
            {
                return LogLevel.Warning;
            }
            else if (_scopeSw.ElapsedMilliseconds >= _autoJumpContextToInfo)
            {
                return LogLevel.Information;
            }
            return null;
        }

        private LogLevel? GetLevelInScope()
        {
            if (_unitarySw.ElapsedMilliseconds >= _autoJumpToWarning)
            {
                return LogLevel.Warning;
            }
            else if (_unitarySw.ElapsedMilliseconds >= _autoJumpToInfo)
            {
                return LogLevel.Information;
            }
            return null;
        }

        /// <summary>
        /// Log the total elapsed time
        /// </summary>
        public void Dispose()
        {
            var logLevel = _endLogLevel;
            if (_autoJumpContext)
            {
                var level = GetLevel();
                if (level.HasValue && level.Value > logLevel)
                {
                    logLevel = level.Value;
                }
            }

            _log?.Log(logLevel, "End {0} : Total elapsed {1}", _endContext, _scopeSw.Display(_timeUnit));
        }

        private void LogBeginMessage()
        {
            _log?.Log(_startLogLevel, "Begin {0}", _startContext);
        }

        #region ILogger

        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <typeparam name="TState">The type of the object to be written.</typeparam>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="state">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a System.String message of the state and exception.</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }
            _unitarySw.Stop();
            if (_autoJump)
            {
                var level = GetLevelInScope();
                if (level.HasValue && level.Value > logLevel)
                {
                    logLevel = level.Value;
                }
            }

            if (formatter != null)
            {
                var message = formatter(state, exception);
                _log.Log(logLevel, _scopeId + " - " + _unitarySw.Display(_timeUnit) + " - " + message);
            }
            else
            {
                _log.Log(logLevel, eventId, state, exception, (s, e) => $"logstate: {s}, logException: {e}");
            }

            _unitarySw.Reset();
            _unitarySw.Start();
        }

        /// <summary>
        /// Checks if the given logLevel is enabled.
        /// </summary>
        /// <param name="logLevel">level to be checked.</param>
        /// <returns>true if enabled.</returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            if (_log == null) return false;
            return _log.IsEnabled(logLevel);
        }

        /// <summary>
        /// Begins a logical operation scope.
        /// </summary>
        /// <typeparam name="TState">The type of the state to begin scope for.</typeparam>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>An System.IDisposable that ends the logical operation scope on dispose.</returns>
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => _log?.BeginScope(state);

        #endregion

        /// <summary>
        /// Total elapsed from birth
        /// </summary>
        public long TotalElapsedMilliseconds => _scopeSw.ElapsedMilliseconds;

        /// <summary>
        /// Current unitary elapsed
        /// </summary>
        public long ElapsedMilliseconds => _unitarySw.ElapsedMilliseconds;
    }
}