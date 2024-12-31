namespace ToolsPack.Logging.Otlp;

/// <summary>
///     Configure the <see cref="JsonFileLogsExporter" />
/// </summary>
public record JsonFileLogsExporterOptions : JsonLogsExporterOptions
{
    /// <summary>
    ///     where to log
    /// </summary>
    public string FilePath { get; set; } = "./logs.log";
}