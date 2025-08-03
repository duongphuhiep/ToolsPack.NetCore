using Microsoft.Extensions.Logging.Abstractions;
using Serilog;
using Serilog.Extensions.Logging;
using Xunit.Abstractions;
using FrameworkLogger = Microsoft.Extensions.Logging.ILogger;

namespace ToolsPack.Logging.Tests;

/// <summary>
///     Holds a Logger object
///     - in normal time, it holds a NullLogger
///     - but whenever a Xunit TestOutput is set then the holding Logger will log to the testOutput
/// </summary>
public class TestOutputInitializer
{
    public FrameworkLogger Logger { get; private set; } = NullLogger.Instance;

    public void Setup(ITestOutputHelper testOutputHelper)
    {
        if (Logger != NullLogger.Instance) return;

        var seriLogger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.TestOutput(testOutputHelper)
            .CreateLogger();
        var seriLoggerProvider = new SerilogLoggerProvider(seriLogger, true);
        Logger = seriLoggerProvider.CreateLogger(string.Empty);
    }
}