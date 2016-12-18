using System;

namespace SKBKontur.Treller.Serialization
{
    public interface IJsonSerializer
    {
        string Serialize<T>(T obj);
        T Deserialize<T>(string str);
        object Deserialize(Type objType, string str);
    }
}