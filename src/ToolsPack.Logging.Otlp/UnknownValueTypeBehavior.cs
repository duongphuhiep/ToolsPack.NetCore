namespace ToolsPack.Logging.Otlp;

/// <summary>
///     The OTLP implementation usually used generic C# <see cref="object" /> as attributes value.
///     If the type of the value was simple values such as "string", "char", int, float... then we can include them in the
///     Json. However, if we met a complex value type, we can choose to Ignore it or to throw a NotImplementedException.
/// </summary>
public enum UnknownValueTypeBehavior
{
    /// <summary>
    ///     Throw NotImplementedException when serialize unknown types.
    /// </summary>
    ThrowNotImplementedException,

    /// <summary>
    ///     Do not serialize attribute of unknown types.
    /// </summary>
    Ignore,

    /// <summary>
    ///     Put the error message in place of the value
    /// </summary>
    ReplaceValueWithErrorMessage
}