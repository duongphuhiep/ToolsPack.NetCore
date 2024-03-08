using System;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace ToolsPack.Logging.Tests
{
    public class SampleFixture : IDisposable
    {
        private readonly IMessageSink _diagnosticMessageSink;

        public SampleFixture(IMessageSink diagnosticMessageSink)
        {
            _diagnosticMessageSink = diagnosticMessageSink;
            _diagnosticMessageSink.OnMessage(new DiagnosticMessage("start sample fixture"));
        }
        public void Dispose()
        {
            _diagnosticMessageSink.OnMessage(new DiagnosticMessage("end sample fixture"));
        }
    }
}
