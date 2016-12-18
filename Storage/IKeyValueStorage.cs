namespace SKBKontur.Treller.Storage
{
    public interface IKeyValueStorage
    {
        T Find<T>(string key);
        void Write<T>(string key, T value);
    }
}