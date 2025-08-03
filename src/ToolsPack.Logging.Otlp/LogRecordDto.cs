using System;
using System.Diagnostics;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;

namespace ToolsPack.Logging.Otlp;

/// <summary>
///     A serializable version of the <see cref="OpenTelemetry.Logs.LogRecord" />
/// </summary>
public record LogRecordDto
{
    public DateTime Timestamp { get; init; }
    public string? TraceId { get; init; }
    public string? SpanId { get; init; }
    public ActivityTraceFlags TraceFlags { get; init; }
    public string? CategoryName { get; init; }
    public LogLevel Severity { get; init; }
    public string? FormattedMessage { get; init; }
    public EventId? EventId { get; init; }
    public string? Body { get; init; }
    public JsonObject? Attributes { get; init; }
    public JsonArray ScopeValues { get; } = new();
    public Exception? Exception { get; init; }
    public JsonObject? Resource { get; init; }
}