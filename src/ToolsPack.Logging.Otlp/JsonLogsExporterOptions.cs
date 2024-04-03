using System.Text.Json;
using System.Text.Json.Serialization;

namespace ToolsPack.Logging.Otlp;

/// <summary>
///     Configure the <see cref="JsonConsoleLogsExporter" />
/// </summary>
public record JsonLogsExporterOptions
{
    /// <summary>
    ///     Define how the <see cref="LogRecordDto" /> should be serialized.
    /// </summary>
    public JsonSerializerOptions JsonSerializerOptions { get; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>
    ///     The OTLP implementation usually used generic C# <see cref="object" /> as value.
    ///     If the type of the value was simple values such as "string", "char", int, float... then we can include them in the
    ///     Json. However, if we met a complex value type, we can choose to Ignore it or to throw a NotImplementedException.
    /// </summary>
    public UnknownValueTypeBehavior UnknownValueTypeBehavior { get; set; } =
        UnknownValueTypeBehavior.ReplaceValueWithErrorMessage;
}