using Newtonsoft.Json;
using System;
using System.Text;
using N = NLog;

namespace ToolsPack.NLog
{
    public class JsonNetSerializer : N.IJsonConverter
    {
        private static readonly JsonSerializerSettings DefaultJsonSerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ReferenceLoopHandling = ReferenceLoopHandling.Ignore };

        private readonly JsonSerializerSettings _settings;

        public JsonNetSerializer()
        {
            _settings = DefaultJsonSerializerSettings;
        }

        public JsonNetSerializer(JsonSerializerSettings settings)
        {
            _settings = settings;
        }

        /// <summary>Serialization of an object into JSON format.</summary>
        /// <param name="value">The object to serialize to JSON.</param>
        /// <param name="builder">Output destination.</param>
        /// <returns>Serialize succeeded (true/false)</returns>
        public bool SerializeObject(object value, StringBuilder builder)
        {
            try
            {
                var jsonSerializer = JsonSerializer.CreateDefault(_settings);
                var sw = new System.IO.StringWriter(builder, System.Globalization.CultureInfo.InvariantCulture);
                using (var jsonWriter = new JsonTextWriter(sw))
                {
                    jsonWriter.Formatting = jsonSerializer.Formatting;
                    jsonSerializer.Serialize(jsonWriter, value, null);
                }
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
