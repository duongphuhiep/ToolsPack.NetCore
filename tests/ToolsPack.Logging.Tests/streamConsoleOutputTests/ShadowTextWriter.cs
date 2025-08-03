using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace ToolsPack.Logging.Tests;

public static class StreamConsole
{
    private static readonly object _lockMessageSink = new();
    private static readonly object _lockTestOutput = new();
    private static IMessageSink? _messageSink;
    private static ITestOutputHelper? _testOutput;

    public static void Setup(IMessageSink messageSink)
    {
        lock (_lockMessageSink)
        {
            if (_messageSink is not null)
                return;
            _messageSink = messageSink;
            Console.SetOut(new ShadowTextWriter(Console.Out, new MessageSinkWriteLiner(messageSink)));
        }
    }

    public static void Setup(ITestOutputHelper testOutput)
    {
        lock (_lockTestOutput)
        {
            if (_testOutput is not null)
                return;
            _testOutput = testOutput;
            Console.SetOut(new ShadowTextWriter(Console.Out, new TestOutputWriteLiner(testOutput)));
        }
    }
}

/// <summary>
///     Make a IWriteLiner shadowing a TextWriter
/// </summary>
public class ShadowTextWriter : TextWriter
{
    private readonly IWriteLiner _origin;
    private readonly TextWriter shadown;

    public ShadowTextWriter(TextWriter origin, IWriteLiner shadow)
    {
        shadown = origin;
        _origin = shadow;
    }

    public override Encoding Encoding => Encoding.UTF8;
    public override IFormatProvider FormatProvider => shadown.FormatProvider;
    public override string NewLine => shadown.NewLine;

    public override void Close()
    {
        shadown.Close();
    }

    public override ValueTask DisposeAsync()
    {
        return shadown.DisposeAsync();
    }

    public override void Flush()
    {
        shadown.Flush();
    }

    public override Task FlushAsync()
    {
        return shadown.FlushAsync();
    }

    public override Task FlushAsync(CancellationToken cancellationToken)
    {
        return shadown.FlushAsync(cancellationToken);
    }


    public override void Write(ulong value)
    {
        shadown.Write(value);
        _origin.WriteLine(value.ToString());
    }

    public override void Write(uint value)
    {
        shadown.Write(value);
        _origin.WriteLine(value.ToString());
    }

    public override void Write(StringBuilder? value)
    {
        shadown.Write(value);
        _origin.WriteLine(value?.ToString());
    }

    public override void Write([StringSyntax("CompositeFormat")] string format, params object?[] arg)
    {
        shadown.Write(format, arg);
        _origin.WriteLine(format, arg);
    }

    public override void Write([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1,
        object? arg2)
    {
        shadown.Write(format, arg0, arg1, arg2);
        _origin.WriteLine(format, arg0, arg1, arg2);
    }

    public override void Write([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1)
    {
        shadown.Write(format, arg0, arg1);
        _origin.WriteLine(format, arg0, arg1);
    }

    public override void Write([StringSyntax("CompositeFormat")] string format, object? arg0)
    {
        shadown.Write(format, arg0);
        _origin.WriteLine(format, arg0);
    }

    public override void Write(string? value)
    {
        shadown.Write(value);
        _origin.WriteLine(value);
    }

    public override void Write(float value)
    {
        shadown.Write(value);
        _origin.WriteLine(value.ToString());
    }

    public override void Write(long value)
    {
        shadown.Write(value);
        _origin.WriteLine(value.ToString());
    }

    public override void Write(int value)
    {
        shadown.Write(value);
        _origin.WriteLine(value.ToString());
    }

    public override void Write(double value)
    {
        shadown.Write(value);
        _origin.WriteLine(value.ToString());
    }

    public override void Write(decimal value)
    {
        shadown.Write(value);
        _origin.WriteLine(value.ToString());
    }

    public override void Write(char[] buffer, int index, int count)
    {
        shadown.Write(buffer, index, count);
        _origin.WriteLine(buffer.ToString());
    }

    public override void Write(char[]? buffer)
    {
        shadown.Write(buffer);
        _origin.WriteLine(buffer?.ToString());
    }

    public override void Write(char value)
    {
        shadown.Write(value);
        _origin.WriteLine(value.ToString());
    }

    public override void Write(bool value)
    {
        shadown.Write(value);
        _origin.WriteLine(value.ToString());
    }

    public override void Write(ReadOnlySpan<char> buffer)
    {
        shadown.Write(buffer);
        _origin.WriteLine(buffer.ToString());
    }

    public override void Write(object? value)
    {
        shadown.Write(value);
        _origin.WriteLine(value?.ToString());
    }

    public override async Task WriteAsync(string? value)
    {
        await shadown.WriteAsync(value).ConfigureAwait(false);
        _origin.WriteLine(value);
    }

    public override async Task WriteAsync(StringBuilder? value, CancellationToken cancellationToken = default)
    {
        await shadown.WriteAsync(value, cancellationToken).ConfigureAwait(false);
        _origin.WriteLine(value?.ToString());
    }

    public override async Task WriteAsync(char value)
    {
        await shadown.WriteAsync(value).ConfigureAwait(false);
        _origin.WriteLine(value.ToString());
    }

    public override async Task WriteAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default)
    {
        await shadown.WriteAsync(buffer, cancellationToken).ConfigureAwait(false);
        _origin.WriteLine(buffer.ToString());
    }

    public override async Task WriteAsync(char[] buffer, int index, int count)
    {
        await shadown.WriteAsync(buffer, index, count).ConfigureAwait(false);
        _origin.WriteLine(buffer.ToString());
    }

    public override void WriteLine(ulong value)
    {
        shadown.WriteLine(value);
        _origin.WriteLine(value.ToString());
    }

    public override void WriteLine(uint value)
    {
        shadown.WriteLine(value);
        _origin.WriteLine(value.ToString());
    }

    public override void WriteLine(StringBuilder? value)
    {
        shadown.WriteLine(value);
        _origin.WriteLine(value?.ToString());
    }

    public override void WriteLine([StringSyntax("CompositeFormat")] string format, params object?[] arg)
    {
        shadown.WriteLine(format, arg);
        _origin.WriteLine(format, arg);
    }

    public override void WriteLine([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1,
        object? arg2)
    {
        shadown.WriteLine(format, arg0, arg1, arg2);
        _origin.WriteLine(format, arg0, arg1, arg2);
    }

    public override void WriteLine([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1)
    {
        shadown.WriteLine(format, arg0, arg1);
        _origin.WriteLine(format, arg0, arg1);
    }

    public override void WriteLine([StringSyntax("CompositeFormat")] string format, object? arg0)
    {
        shadown.WriteLine(format, arg0);
        _origin.WriteLine(format, arg0);
    }

    public override void WriteLine(string? value)
    {
        shadown.WriteLine(value);
        _origin.WriteLine(value);
    }

    public override void WriteLine(float value)
    {
        shadown.WriteLine(value);
        _origin.WriteLine(value.ToString());
    }

    public override void WriteLine(long value)
    {
        shadown.WriteLine(value);
        _origin.WriteLine(value.ToString());
    }

    public override void WriteLine(int value)
    {
        shadown.WriteLine(value);
        _origin.WriteLine(value.ToString());
    }

    public override void WriteLine(double value)
    {
        shadown.WriteLine(value);
        _origin.WriteLine(value.ToString());
    }

    public override void WriteLine(decimal value)
    {
        shadown.WriteLine(value);
        _origin.WriteLine(value.ToString());
    }

    public override void WriteLine(char[] buffer, int index, int count)
    {
        shadown.WriteLine(buffer, index, count);
        _origin.WriteLine(buffer.ToString());
    }

    public override void WriteLine(char[]? buffer)
    {
        shadown.WriteLine(buffer);
        _origin.WriteLine(buffer?.ToString());
    }

    public override void WriteLine(char value)
    {
        shadown.WriteLine(value);
        _origin.WriteLine(value.ToString());
    }

    public override void WriteLine(bool value)
    {
        shadown.WriteLine(value);
        _origin.WriteLine(value.ToString());
    }

    public override void WriteLine(ReadOnlySpan<char> buffer)
    {
        shadown.WriteLine(buffer);
        _origin.WriteLine(buffer.ToString());
    }

    public override void WriteLine(object? value)
    {
        shadown.WriteLine(value);
        _origin.WriteLine(value?.ToString());
    }

    public override async Task WriteLineAsync(string? value)
    {
        await shadown.WriteLineAsync(value).ConfigureAwait(false);
        _origin.WriteLine(value);
    }

    public override async Task WriteLineAsync(StringBuilder? value, CancellationToken cancellationToken = default)
    {
        await shadown.WriteLineAsync(value, cancellationToken).ConfigureAwait(false);
        _origin.WriteLine(value?.ToString());
    }

    public override async Task WriteLineAsync(char value)
    {
        await shadown.WriteLineAsync(value).ConfigureAwait(false);
        _origin.WriteLine(value.ToString());
    }

    public override async Task WriteLineAsync(ReadOnlyMemory<char> buffer,
        CancellationToken cancellationToken = default)
    {
        await shadown.WriteLineAsync(buffer, cancellationToken).ConfigureAwait(false);
        _origin.WriteLine(buffer.ToString());
    }

    public override async Task WriteLineAsync(char[] buffer, int index, int count)
    {
        await shadown.WriteLineAsync(buffer, index, count).ConfigureAwait(false);
        _origin.WriteLine(buffer.ToString());
    }
}