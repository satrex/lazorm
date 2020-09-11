using System;
using Newtonsoft;

namespace Lazorm
{
    /// <summary>
    /// Write connectionstring entry into appsettings.json
    /// </summary>
    public static class JsonSettingWriter
    {

        /// <summary>
        /// Write connectionstring entry into appsettings.json
        /// </summary>
        /// <param name="key">entry key name</param>
        /// <param name="value">entry value connectionstring</param>
        /// <param name="appSettingsJsonFilePath">(optional) appsetting.json file path</param>
        public static void SetAppSettingValue(string key, string value, string appSettingsJsonFilePath = null)
        {
            if (appSettingsJsonFilePath == null)
            {
                appSettingsJsonFilePath = System.IO.Path.Combine(System.Environment.CurrentDirectory, "appsettings.json");
            }
            if (!System.IO.File.Exists(appSettingsJsonFilePath))
            {
                var source = System.IO.Path.Combine(System.AppContext.BaseDirectory, "appsettings.json.template");
                System.IO.File.Copy(source, appSettingsJsonFilePath);
            }

            var json = System.IO.File.ReadAllText(appSettingsJsonFilePath);
            dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);

            jsonObj["ConnectionStrings"][key] = value;


            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);

            System.IO.File.WriteAllText(appSettingsJsonFilePath, output);
        }

    }
}
