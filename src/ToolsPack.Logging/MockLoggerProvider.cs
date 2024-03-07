using Microsoft.Extensions.Logging;

namespace ToolsPack.Logging.Testing;

public sealed class MockLoggerProvider : ILoggerProvider
{
    private readonly MockLogger logger;

    public MockLoggerProvider(MockLogger logger)
    {
        this.logger = logger;
    }
    public ILogger CreateLogger(string categoryName)
    {
        return logger;
    }

    public void Dispose()
    {
    }
}

