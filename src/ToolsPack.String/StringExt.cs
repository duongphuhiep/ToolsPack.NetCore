using System.Collections.Generic;
using System.Linq;

namespace ToolsPack.String;

/// <summary>
/// Convenient string.Join
/// </summary>
public static class StringExt
{
    public static string JoinNonEmpty(string? separator, params string?[] value)
        => string.Join(separator, value.Where(s => !string.IsNullOrEmpty(s)));
    public static string JoinNonEmpty(string? separator, string?[] value, int startIndex, int count)
        => string.Join(separator, value.Where(s => !string.IsNullOrEmpty(s)), startIndex, count);
    public static string JoinNonEmpty<T>(string? separator, IEnumerable<T> values)
        => string.Join<T>(separator, values.Where(s => s is not null));
    public static string JoinNonEmpty(string? separator, params object?[] values)
        => string.Join(separator, values.Where(s => s is not null));
    public static string JoinNonEmpty(string? separator, IEnumerable<string?> values)
        => string.Join(separator, values.Where(s => !string.IsNullOrEmpty(s)));
#if NET5_0_OR_GREATER
    public static string JoinNonEmpty<T>(char separator, IEnumerable<T> values)
        => string.Join(separator, values.Where(s => s is not null));
    public static string JoinNonEmpty(char separator, params object?[] values)
        => string.Join(separator, values.Where(s => s is not null));
    public static string JoinNonEmpty(char separator, string?[] value, int startIndex, int count)
        => string.Join(separator, value.Where(s => !string.IsNullOrEmpty(s)), startIndex, count);
    public static string JoinNonEmpty(char separator, params string?[] value)
        => string.Join(separator, value.Where(s => !string.IsNullOrEmpty(s)));
#endif
}