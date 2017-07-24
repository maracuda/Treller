using System;

namespace Logger
{
    public interface ILogService
    {
        event EventHandler<ErrorEventArgs> OnError;
        void RegisterError(string message);
        void RegisterError(string message, Exception e);
    }
}