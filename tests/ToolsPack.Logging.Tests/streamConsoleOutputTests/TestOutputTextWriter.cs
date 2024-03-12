using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace ToolsPack.Logging.Tests;

/// <summary>
/// Custom TextWriter that writes to ITestOutputHelper 
/// </summary>
public class TestOutputTextWriter : TextWriter
{
    private readonly ITestOutputHelper _new;
    private readonly TextWriter _old;

    public TestOutputTextWriter(ITestOutputHelper output)
    {
        _old = Console.Out;
        _new = output;
        Console.SetOut(this);
        Console.SetError(this);
    }

    public override Encoding Encoding => Encoding.UTF8;
    public override IFormatProvider FormatProvider => _old.FormatProvider;
    public override string NewLine => _old.NewLine;
    public override void Close() => _old.Close();
    public override ValueTask DisposeAsync() => _old.DisposeAsync();
    public override void Flush() => _old.Flush();
    public override Task FlushAsync() => _old.FlushAsync();
    public override Task FlushAsync(CancellationToken cancellationToken) => _old.FlushAsync(cancellationToken);


    public override void Write(ulong value)
    {
        _old.Write(value);
        _new.WriteLine(value.ToString());
    }
    public override void Write(uint value)
    {
        _old.Write(value);
        _new.WriteLine(value.ToString());
    }
    public override void Write(StringBuilder? value)
    {
        _old.Write(value);
        _new.WriteLine(value?.ToString());
    }
    public override void Write([StringSyntax("CompositeFormat")] string format, params object?[] arg)
    {
        _old.Write(format, arg);
        _new.WriteLine(format, arg);
    }
    public override void Write([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1, object? arg2)
    {
        _old.Write(format, arg0, arg1, arg2);
        _new.WriteLine(format, arg0, arg1, arg2);
    }
    public override void Write([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1)
    {
        _old.Write(format, arg0, arg1);
        _new.WriteLine(format, arg0, arg1);
    }
    public override void Write([StringSyntax("CompositeFormat")] string format, object? arg0)
    {
        _old.Write(format, arg0);
        _new.WriteLine(format, arg0);
    }
    public override void Write(string? value)
    {
        _old.Write(value);
        _new.WriteLine(value);
    }
    public override void Write(float value)
    {
        _old.Write(value);
        _new.WriteLine(value.ToString());
    }
    public override void Write(long value)
    {
        _old.Write(value);
        _new.WriteLine(value.ToString());
    }
    public override void Write(int value)
    {
        _old.Write(value);
        _new.WriteLine(value.ToString());
    }
    public override void Write(double value)
    {
        _old.Write(value);
        _new.WriteLine(value.ToString());
    }
    public override void Write(decimal value)
    {
        _old.Write(value);
        _new.WriteLine(value.ToString());
    }
    public override void Write(char[] buffer, int index, int count)
    {
        _old.Write(buffer, index, count);
        _new.WriteLine(buffer.ToString());
    }
    public override void Write(char[]? buffer)
    {
        _old.Write(buffer);
        _new.WriteLine(buffer?.ToString());
    }
    public override void Write(char value)
    {
        _old.Write(value);
        _new.WriteLine(value.ToString());
    }
    public override void Write(bool value)
    {
        _old.Write(value);
        _new.WriteLine(value.ToString());
    }
    public override void Write(ReadOnlySpan<char> buffer)
    {
        _old.Write(buffer);
        _new.WriteLine(buffer.ToString());
    }
    public override void Write(object? value)
    {
        _old.Write(value);
        _new.WriteLine(value?.ToString());
    }
    public override async Task WriteAsync(string? value)
    {
        await _old.WriteAsync(value).ConfigureAwait(false);
        _new.WriteLine(value);
    }
    public override async Task WriteAsync(StringBuilder? value, CancellationToken cancellationToken = default)
    {
        await _old.WriteAsync(value, cancellationToken).ConfigureAwait(false);
        _new.WriteLine(value?.ToString());
    }
    public override async Task WriteAsync(char value)
    {
        await _old.WriteAsync(value).ConfigureAwait(false);
        _new.WriteLine(value.ToString());
    }
    public override async Task WriteAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default)
    {
        await _old.WriteAsync(buffer, cancellationToken).ConfigureAwait(false);
        _new.WriteLine(buffer.ToString());
    }
    public override async Task WriteAsync(char[] buffer, int index, int count)
    {
        await _old.WriteAsync(buffer, index, count).ConfigureAwait(false);
        _new.WriteLine(buffer.ToString());
    }
    public override void WriteLine(ulong value)
    {
        _old.WriteLine(value);
        _new.WriteLine(value.ToString());
    }
    public override void WriteLine(uint value)
    {
        _old.WriteLine(value);
        _new.WriteLine(value.ToString());
    }
    public override void WriteLine(StringBuilder? value)
    {
        _old.WriteLine(value);
        _new.WriteLine(value?.ToString());
    }
    public override void WriteLine([StringSyntax("CompositeFormat")] string format, params object?[] arg)
    {
        _old.WriteLine(format, arg);
        _new.WriteLine(format, arg);
    }
    public override void WriteLine([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1, object? arg2)
    {
        _old.WriteLine(format, arg0, arg1, arg2);
        _new.WriteLine(format, arg0, arg1, arg2);
    }
    public override void WriteLine([StringSyntax("CompositeFormat")] string format, object? arg0, object? arg1)
    {
        _old.WriteLine(format, arg0, arg1);
        _new.WriteLine(format, arg0, arg1);
    }
    public override void WriteLine([StringSyntax("CompositeFormat")] string format, object? arg0)
    {
        _old.WriteLine(format, arg0);
        _new.WriteLine(format, arg0);
    }
    public override void WriteLine(string? value)
    {
        _old.WriteLine(value);
        _new.WriteLine(value);
    }
    public override void WriteLine(float value)
    {
        _old.WriteLine(value);
        _new.WriteLine(value.ToString());
    }
    public override void WriteLine(long value)
    {
        _old.WriteLine(value);
        _new.WriteLine(value.ToString());
    }
    public override void WriteLine(int value)
    {
        _old.WriteLine(value);
        _new.WriteLine(value.ToString());
    }
    public override void WriteLine(double value)
    {
        _old.WriteLine(value);
        _new.WriteLine(value.ToString());
    }
    public override void WriteLine(decimal value)
    {
        _old.WriteLine(value);
        _new.WriteLine(value.ToString());
    }
    public override void WriteLine(char[] buffer, int index, int count)
    {
        _old.WriteLine(buffer, index, count);
        _new.WriteLine(buffer.ToString());
    }
    public override void WriteLine(char[]? buffer)
    {
        _old.WriteLine(buffer);
        _new.WriteLine(buffer?.ToString());
    }
    public override void WriteLine(char value)
    {
        _old.WriteLine(value);
        _new.WriteLine(value.ToString());
    }
    public override void WriteLine(bool value)
    {
        _old.WriteLine(value);
        _new.WriteLine(value.ToString());
    }
    public override void WriteLine(ReadOnlySpan<char> buffer)
    {
        _old.WriteLine(buffer);
        _new.WriteLine(buffer.ToString());
    }
    public override void WriteLine(object? value)
    {
        _old.WriteLine(value);
        _new.WriteLine(value?.ToString());
    }
    public override async Task WriteLineAsync(string? value)
    {
        await _old.WriteLineAsync(value).ConfigureAwait(false);
        _new.WriteLine(value);
    }
    public override async Task WriteLineAsync(StringBuilder? value, CancellationToken cancellationToken = default)
    {
        await _old.WriteLineAsync(value, cancellationToken).ConfigureAwait(false);
        _new.WriteLine(value?.ToString());
    }
    public override async Task WriteLineAsync(char value)
    {
        await _old.WriteLineAsync(value).ConfigureAwait(false);
        _new.WriteLine(value.ToString());
    }
    public override async Task WriteLineAsync(ReadOnlyMemory<char> buffer, CancellationToken cancellationToken = default)
    {
        await _old.WriteLineAsync(buffer, cancellationToken).ConfigureAwait(false);
        _new.WriteLine(buffer.ToString());
    }
    public override async Task WriteLineAsync(char[] buffer, int index, int count)
    {
        await _old.WriteLineAsync(buffer, index, count).ConfigureAwait(false);
        _new.WriteLine(buffer.ToString());
    }

}