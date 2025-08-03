using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;

namespace ToolsPack.Logging.Otlp;

/// <summary>
///     Export <see cref="OpenTelemetry.Logs.LogRecord" /> to Console in Json format
/// </summary>
public class JsonFileLogsExporter : BaseExporter<LogRecord>
{
    private readonly JsonFileLogsExporterOptions _options = new();
    private Resource? _cachedParentResource;
    private JsonObject? _cachedParentResourceJson;

    /// <summary>
    ///     Create and configure the exporter.
    /// </summary>
    /// <param name="configure"></param>
    public JsonFileLogsExporter(Action<JsonFileLogsExporterOptions>? configure = null)
    {
        configure?.Invoke(_options);
        CreateLogFolder(_options.FilePath);
    }

    private JsonObject? ComputeResourceJson()
    {
        var resource = ParentProvider.GetResource();
        if (resource == _cachedParentResource) return _cachedParentResourceJson;
        _cachedParentResource = resource;
        _cachedParentResourceJson = resource.Attributes.ConvertToJsonObject(_options.UnknownValueTypeBehavior);
        return _cachedParentResourceJson;
    }

    private static void CreateLogFolder(string filePath)
    {
        var directoryPath = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directoryPath)) Directory.CreateDirectory(directoryPath);
    }

    /// <summary>
    ///     Export to File
    /// </summary>
    /// <param name="batch"></param>
    /// <returns></returns>
    public override ExportResult Export(in Batch<LogRecord> batch)
    {
        if (batch.Count == 0) return ExportResult.Success;
        var resourceJson = ComputeResourceJson();
        if (resourceJson is null) return ExportResult.Success;
        StringBuilder sb = new();
        foreach (var record in batch)
        {
            var recordDto = record.Convert(resourceJson, _options.UnknownValueTypeBehavior);
            sb.AppendLine(JsonSerializer.Serialize(recordDto, _options.JsonSerializerOptions));
        }

        File.AppendAllText(_options.FilePath, sb.ToString());
        return ExportResult.Success;
    }
}