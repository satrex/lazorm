using System;
using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            Console.WriteLine("\n{0} started.", System.Reflection.MethodInfo.GetCurrentMethod().Name);

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
            // dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);
            JToken token =  JObject.Parse(json);
            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(token.ToString());
            const string CONSTR = "ConnectionStrings";

            if(!jsonObj.ContainsKey(CONSTR))
            {
                Console.WriteLine("{0} node does not exist", CONSTR);
                jsonObj.Add(CONSTR, new JObject());
                System.Threading.Thread.Sleep(200);
                jsonObj[CONSTR].Add(key, value);
                Console.WriteLine("{0} node added.", CONSTR);
            }
            else
            {
                Console.WriteLine("{0} node exists", CONSTR);
                jsonObj[CONSTR][key] = value;
            }

            string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);

            System.IO.File.WriteAllText(appSettingsJsonFilePath, output);
            Console.WriteLine("connection is written in config file {0} ", appSettingsJsonFilePath);
        }

    }
}
