using System;
using System.Text;
using System.Text.Json;
using N = NLog;

namespace ToolsPack.NLog
{
    /// <summary>
    /// DefaultJsonSerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
    /// </summary>
    public class MicrosoftJsonSerializer : N.IJsonConverter
    {
        private static readonly JsonSerializerOptions DefaultJsonSerializerOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            MaxDepth = 10,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        private readonly JsonSerializerOptions _options;

        public MicrosoftJsonSerializer()
        {
            _options = DefaultJsonSerializerOptions;
        }

        public MicrosoftJsonSerializer(JsonSerializerOptions options)
        {
            _options = options;
        }

        /// <summary>Serialization of an object into JSON format.</summary>
        /// <param name="value">The object to serialize to JSON.</param>
        /// <param name="builder">Output destination.</param>
        /// <returns>Serialize succeeded (true/false)</returns>
        public bool SerializeObject(object value, StringBuilder builder)
        {
            try
            {
                builder.Append(JsonSerializer.Serialize(value, _options));
            }
            catch (Exception e)
            {
                N.Common.InternalLogger.Error(e, "Error when custom JSON serialization");
                return false;
            }
            return true;
        }
    }
}
