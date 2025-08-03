using System;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ToolsPack.Logging.Tests;

public class Fixture3 : IDisposable
{
    private readonly IMessageSink _diagnosticMessageSink;

    public Fixture3(IMessageSink diagnosticMessageSink)
    {
        _diagnosticMessageSink = diagnosticMessageSink;
        _diagnosticMessageSink.OnMessage(new DiagnosticMessage("start fixture 3"));
    }

    public void Dispose()
    {
        _diagnosticMessageSink.OnMessage(new DiagnosticMessage("end fixture 3"));
    }
}