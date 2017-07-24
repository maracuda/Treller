using System;
using Newtonsoft.Json;

namespace Serialization
{
    public class JsonSerializer : IJsonSerializer
    {
        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T Deserialize<T>(string str)
        {
            return (T) Deserialize(typeof(T), str);
        }

        public object Deserialize(Type objType, string str)
        {
            return JsonConvert.DeserializeObject(str, objType);
        }
    }
}