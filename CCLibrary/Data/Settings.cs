using System;
using System.Linq;
using System.Configuration;
using CCLibrary.Exceptions;

namespace CCLibrary.Data
{
    public static class Settings
    {
        public static T Get<T>(string key, T defaultValue = default)
        {
            try
            {
                string? value = ConfigurationManager.AppSettings[key];
                if (value is null)
                    return defaultValue;
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }

        public static void Set<T>(string key, T value)
        {
            try
            {
                Configuration? config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                KeyValueConfigurationCollection? settings = config.AppSettings.Settings;
                string? sValue = Convert.ToString(value);
                if (settings.AllKeys.Contains(key))
                    settings[key].Value = sValue;
                else
                    settings.Add(key, sValue);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                throw new SettingsException(ex.Message);
            }
        }
    }
}
