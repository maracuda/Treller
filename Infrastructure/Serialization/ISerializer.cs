namespace SKBKontur.Infrastructure.Serialization
{
    public interface ISerializer
    {
        byte[] Serialize<T>(string serializeType, T data);
        T Deserialize<T>(string serializeType, byte[] data);
        string Stringify<T>(string serializeType, T data);
    }
}