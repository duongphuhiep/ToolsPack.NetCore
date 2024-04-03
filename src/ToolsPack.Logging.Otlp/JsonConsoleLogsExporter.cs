using System;
using System.Text.Json;
using System.Text.Json.Nodes;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

namespace ToolsPack.Logging.Otlp;

/// <summary>
///     Export <see cref="OpenTelemetry.Logs.LogRecord" /> to Console in Json format
/// </summary>
public class JsonConsoleLogsExporter : BaseExporter<LogRecord>
{
    private readonly JsonLogsExporterOptions _options = new();
    private Resource? _cachedParentResource;
    private JsonObject? _cachedParentResourceJson;

    /// <summary>
    ///     Create and configure the exporter.
    /// </summary>
    /// <param name="configure"></param>
    public JsonConsoleLogsExporter(Action<JsonLogsExporterOptions>? configure = null)
    {
        configure?.Invoke(_options);
    }

    private JsonObject? ComputeResourceJson()
    {
        var resource = ParentProvider.GetResource();
        if (resource == _cachedParentResource) return _cachedParentResourceJson;
        _cachedParentResource = resource;
        _cachedParentResourceJson = resource.Attributes.ConvertToJsonObject(_options.UnknownValueTypeBehavior);
        return _cachedParentResourceJson;
    }

    /// <summary>
    ///     Export to Console
    /// </summary>
    /// <param name="batch"></param>
    /// <returns></returns>
    public override ExportResult Export(in Batch<LogRecord> batch)
    {
        var resourceJson = ComputeResourceJson();
        foreach (var record in batch)
        {
            var recordDto = record.Convert(resourceJson, _options.UnknownValueTypeBehavior);
            var s = JsonSerializer.Serialize(recordDto, _options.JsonSerializerOptions);
            Console.WriteLine(s);
        }

        return ExportResult.Success;
    }
}