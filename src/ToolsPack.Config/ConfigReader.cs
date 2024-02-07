using System;
using System.Configuration;
using System.Globalization;

namespace ToolsPack.Config
{
    /// <summary>
    /// Read the app.config file from ConfigurationManager.AppSettings
    /// </summary>
    public static class ConfigReader
    {
        public static T Read<T>(string key, T defaultValue = default(T))
        {
            string value = null;
            try
            {
                value = ConfigurationManager.AppSettings.Get(key);
                if (value == null)
                {
                    return defaultValue;
                }
                return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException($"Failed to read config '{key}' = '{value}' - {ex.Message}", ex);
            }
        }
    }
}
