namespace SKBKontur.Treller.Logger
{
    public interface ILoggerFactory
    {
        ILogger Get<T>();
    }
}