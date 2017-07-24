namespace Storage
{
    public interface IKeyValueStorage
    {
        T Read<T>(string key);
        T Find<T>(string key);
        void Write<T>(string key, T value);
    }
}