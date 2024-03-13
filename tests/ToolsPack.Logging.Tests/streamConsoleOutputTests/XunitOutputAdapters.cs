using Xunit.Abstractions;
using Xunit.Sdk;

namespace ToolsPack.Logging.Tests;

/// <summary>
/// Common interface for ITestOuputHelper & IMessageSink
/// </summary>
public interface IWriteLiner
{
    void WriteLine(string message);
    void WriteLine(string format, params object[] args);
}

public class TestOutputWriteLiner : IWriteLiner
{
    private readonly ITestOutputHelper _testOutputHelper;
    public TestOutputWriteLiner(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }
    public void WriteLine(string message)
        => _testOutputHelper.WriteLine(message);

    public void WriteLine(string format, params object[] args)
        => _testOutputHelper.WriteLine(format, args);
}

public class MessageSinkWriteLiner : IWriteLiner
{
    private readonly IMessageSink _messageSink;
    public MessageSinkWriteLiner(IMessageSink messageSink)
    {
        _messageSink = messageSink;
    }
    public void WriteLine(string message)
        => _messageSink.OnMessage(new DiagnosticMessage(message));

    public void WriteLine(string format, params object[] args)
        => _messageSink.OnMessage(new DiagnosticMessage(format, args));
}