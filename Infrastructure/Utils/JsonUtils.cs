using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Utils
{
    public class JsonUtils
    {
        public static T convertJsonToNetObject<T>(String json)
        {
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, jsSettings);
        }
        public static T convertJsonToNetObject<T>(String json, String cultureInfo)
        {
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            jsSettings.Culture = new System.Globalization.CultureInfo(cultureInfo);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, jsSettings);
        }

        public static T convertJsonToNetObject<T>(String json, bool typing)
        {
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            jsSettings.TypeNameHandling = TypeNameHandling.Objects;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, jsSettings);
        }
        public static T convertJsonToNetObject<T>(String json, String cultureInfo, bool typing)
        {
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            jsSettings.Culture = new System.Globalization.CultureInfo(cultureInfo);
            jsSettings.TypeNameHandling = TypeNameHandling.Objects;
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json, jsSettings);
        }

        public static string Json<T>(T obj)
        {
            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, Formatting.None, jsSettings);
        }
        public static string createJsonMessage(bool success, string message)
        {
            StringBuilder json = new StringBuilder();
            json.Append("{");
            json.Append("\"success\":\"" + success + "\",");
            if (message != null)
            {
                if (message.Contains("'"))
                {
                    throw new Exception("Message no puede contener comillas simples.");
                }
                json.Append("\"message\":\"" + message + "\"");
            }
            else
            {
                json = json.Remove(json.Length - 1, 1);
            }
            json.Append("}");
            return json.ToString();
        }

        public static string convertNetObjectToJson(string objRoot, Object obj)
        {
            StringBuilder json = new StringBuilder();
            json.Append("{");
            json.Append("\"" + objRoot + "\":");
            json.Append(JsonConvert.SerializeObject(obj));//, converters));
            json.Append("}");
            return json.ToString();
        }
        public static string convertNetObjectToJson(Object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
        }
        public static string convertNetObjectToJson(Object obj, bool typing)
        {
            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects, ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }
        public static string convertNetObjectToJson(bool success, string objRoot, Object obj)
        {
            return convertNetObjectToJson(success, null, objRoot, obj);
        }

        public static string convertNetObjectToJson(bool success, string message, string objRoot, Object obj)
        {
            StringBuilder json = new StringBuilder();
            json.Append("{");
            json.Append("\"success\":\"" + success + "\",");
            if (message != null)
            {
                if (message.Contains("'"))
                {
                    throw new Exception("Message no puede contener comillas simples.");
                }
                json.Append("\"message\":\"" + message + "\",");
            }
            json.Append("\"" + objRoot + "\":");

            json.Append(JsonConvert.SerializeObject(obj));//, converters));
            json.Append("}");
            return json.ToString();
        }
        /// <summary>
        /// obtiene de un json el valor de la propiedad que se pasa como parámetro.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string getValueInJson(string key, string json)
        {
            string value = null;
            Dictionary<string, string> diccionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            if (diccionary == null) return null;

            if (diccionary.ContainsKey(key))
            {
                value = diccionary[key];
            }

            return value;

        }
        /// <summary>
        /// Método que Regresa un array de tipo generico pasando como parametro un json con ese formato
        /// Colocando como raiz del json el "data"
        /// </summary>
        /// <typeparam name="T">clase de tipo genrica</typeparam>
        /// <param name="json">string en formato json</param>
        /// <returns>Array generico</returns>
        public static T[] GetGenericArrayByJSON<T>(string json) where T : class
        {

            var template = new { data = new T[] { } };

            T[] g = JsonConvert.DeserializeAnonymousType(json, template).data;

            return g;
        }
        /// <summary>
        /// Método que Regresa un objeto de tipo generico pasando como parametro un json con ese formato
        /// Colocando como raiz del json el "data"
        /// </summary>
        /// <typeparam name="T">clase de tipo generica</typeparam>
        /// <param name="json">string en formato json</param>
        /// <returns>objeto generico</returns>
        public static T GetGenericObjectByJSON<T>(string json) where T : new()
        {
            var template = new { data = new T() };

            T g = JsonConvert.DeserializeAnonymousType(json, template).data;

            return g;
        }
        public static dynamic Grid<T>(IList<T> data, params string[] props)
        {
            var jsonData = data.Select(item => new
            {
                cell = SetProperties(item, props).Select(x => x.Value).ToArray()
            });
            return jsonData;
        }
        private static Dictionary<string, string> SetProperties(object current, params string[] props)
        {
            var result = new Dictionary<string, string>();
            int countEmpty = 0;
            bool empty = false;
            const String KEY = "empty_";
            foreach (var str in props)
            {
                Object obj = null;
                String value = String.Empty;
                String final_key = String.Empty;
                empty = String.IsNullOrEmpty(str);

                if (!empty)
                {
                    obj = current.GetType().GetProperty(str).GetValue(current, null);
                    value = obj != null ? obj.ToString() : String.Empty;
                    final_key = str;
                }
                else
                {
                    final_key = String.Format("{0}{1}", KEY, (countEmpty++));
                }
                result.Add(final_key, value);
            }
            return result;
        }
    }
}
