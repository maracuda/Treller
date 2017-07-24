namespace Logger
{
    public interface ILoggerFactory
    {
        ILogger Get<T>();
    }
}