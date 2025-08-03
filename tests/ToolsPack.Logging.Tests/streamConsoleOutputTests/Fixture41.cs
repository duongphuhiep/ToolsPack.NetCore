using System;
using Xunit.Abstractions;

namespace ToolsPack.Logging.Tests;

public class Fixture41 : IDisposable
{
    public Fixture41(IMessageSink messageSink)
    {
        StreamConsole.Setup(messageSink);
        Console.WriteLine("Fixture41 start");
    }

    public void Dispose()
    {
        //will the console shadowing will crash here
        //the testOutputHelper was disposed and cannot be use here
        Console.WriteLine("Fixture41 end");
    }
}