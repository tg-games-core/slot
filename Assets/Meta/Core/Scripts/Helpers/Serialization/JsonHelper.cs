using System;
using Newtonsoft.Json;

namespace Core
{
    public static class JsonHelper
    {
        public static T Convert<T>(string jsonData)
        {
            T data = default;
            jsonData = jsonData.Replace("\\", "");

            try
            {
                data = JsonConvert.DeserializeObject<T>(jsonData);
            }
            catch
            {
                DebugSafe.LogException(new Exception($"Can't convert string to {typeof(T)}"));
            }
            
            return data;
        }
        
        public static string Convert<T>(T data, bool prettyPrint = false)
        {
            var formatting = prettyPrint ? Formatting.Indented : Formatting.None;
            return JsonConvert.SerializeObject(data, formatting);
        }
    }
}