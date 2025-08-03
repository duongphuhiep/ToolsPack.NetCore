using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using OpenTelemetry.Logs;
using MyOtelAttributesNullable = System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<string, object?>>;

namespace ToolsPack.Logging.Otlp;

/// <summary>
///     Convert the <see cref="OpenTelemetry.Logs.LogRecord" />to a serializable <see cref="LogRecordDto" />
/// </summary>
public static class LogRecordConverter
{
    /// <summary>
    ///     Convert the <see cref="OpenTelemetry.Logs.LogRecord" />to a serializable <see cref="LogRecordDto" />
    /// </summary>
    /// <param name="record">the original non-serializable Log Record</param>
    /// <param name="attachedResource">the parent resource</param>
    /// <param name="unknownValueTypeBehavior">Define what to do if unable to convert the value</param>
    /// <returns></returns>
    public static LogRecordDto Convert(this LogRecord record, JsonObject attachedResource,
        UnknownValueTypeBehavior unknownValueTypeBehavior)
    {
        var dto = new LogRecordDto
        {
            Timestamp = record.Timestamp,
            TraceId = record.TraceId.ToString(),
            SpanId = record.SpanId.ToString(),
            TraceFlags = record.TraceFlags,
            CategoryName = record.CategoryName,
            Severity = record.LogLevel,
            FormattedMessage = record.FormattedMessage,
            Body = record.Body,
            Attributes = record.Attributes?.ConvertToJsonObject(unknownValueTypeBehavior),
            EventId = record.EventId,
            Exception = record.Exception,
            Resource = attachedResource
        };
        record.ForEachScope((logRecordScopes, jsonArray) =>
        {
            var jsonObject = logRecordScopes.ConvertToJsonObject(unknownValueTypeBehavior);
            jsonArray.Add(jsonObject);
        }, dto.ScopeValues);

        return dto;
    }

    /// <summary>
    ///     Convert a List of Key-Value pair to a JsonObject
    /// </summary>
    /// <param name="attributes">collection of KeyValue Pair</param>
    /// <param name="unknownValueTypeBehavior">Define what to do if unable to convert the value</param>
    /// <returns></returns>
    public static JsonObject ConvertToJsonObject(this MyOtelAttributesNullable attributes,
        UnknownValueTypeBehavior unknownValueTypeBehavior)
    {
        var jsonObject = new JsonObject();

        foreach (var attribute in attributes)
        {
            var isSuccess = ConvertValue(attribute.Value, out var jsonNode, unknownValueTypeBehavior);
            if (isSuccess)
                jsonObject[attribute.Key] = jsonNode;
            else //Unable to convert the value of this attribute
                switch (unknownValueTypeBehavior)
                {
                    case UnknownValueTypeBehavior.ReplaceValueWithErrorMessage:
                        jsonObject[attribute.Key] = ErrorInsteadOfRealValue(attribute.Value);
                        break;
                    case UnknownValueTypeBehavior.ThrowNotImplementedException:
                        throw new NotImplementedException($"Unsupported value type: {attribute.Value?.GetType()}");
                    default: //UnknownValueTypeBehavior.Ignore
                        continue; //ignore this attribute's key-value pair
                }
        }

        return jsonObject;
    }
    
    /// <summary>
    ///     Convert a LogRecordScope to a JsonObject
    /// </summary>
    /// <param name="logScopes">collection of KeyValue Pair</param>
    /// <param name="unknownValueTypeBehavior">Define what to do if unable to convert the value</param>
    /// <returns></returns>
    public static JsonObject ConvertToJsonObject(this LogRecordScope logScopes,
        UnknownValueTypeBehavior unknownValueTypeBehavior)
    {
        var jsonObject = new JsonObject();

        foreach (var attribute in logScopes)
        {
            var isSuccess = ConvertValue(attribute.Value, out var jsonNode, unknownValueTypeBehavior);
            if (isSuccess)
                jsonObject[attribute.Key] = jsonNode;
            else //Unable to convert the value of this attribute
                switch (unknownValueTypeBehavior)
                {
                    case UnknownValueTypeBehavior.ReplaceValueWithErrorMessage:
                        jsonObject[attribute.Key] = ErrorInsteadOfRealValue(attribute.Value);
                        break;
                    case UnknownValueTypeBehavior.ThrowNotImplementedException:
                        throw new NotImplementedException($"Unsupported value type: {attribute.Value?.GetType()}");
                    default: //UnknownValueTypeBehavior.Ignore
                        continue; //ignore this attribute's key-value pair
                }
        }

        return jsonObject;
    }

    private static string? ErrorInsteadOfRealValue(object? value)
    {
        return value?.GetType().ToString();
    }

    private static bool ConvertValue(object? value, out JsonNode? node,
        UnknownValueTypeBehavior unknownValueTypeBehavior)
    {
        switch (value)
        {
            case null:
                node = null;
                break;
            case char c:
                node = c.ToString();
                break;
            case string str:
                node = str;
                break;
            case bool b:
                node = b;
                break;
            case byte or sbyte or short or ushort or int or uint or long or float or double or decimal:
                node = JsonValue.Create(value);
                break;
            case IEnumerable<KeyValuePair<string, object?>> nestedAttributes:
                node = ConvertToJsonObject(nestedAttributes, unknownValueTypeBehavior);
                break;
            case LogRecordScope scope:
                node = ConvertToJsonObject(scope, unknownValueTypeBehavior);
                break;
            default:
                node = null;
                return false;
        }

        return true;
    }
}