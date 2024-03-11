using Microsoft.Extensions.Logging;

namespace ToolsPack.Logging.Tests;

/// <summary>
/// The provider for TestOutputLogger
/// </summary>
public class TestOutputLoggerProvider : ILoggerProvider
{
    private readonly TestOutputInitializer _testOutput;

    public TestOutputLoggerProvider(TestOutputInitializer testOutput)
    {
        _testOutput = testOutput;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new TestOutputLogger(_testOutput, categoryName);
    }

    public void Dispose()
    {
    }
}
