using System;

namespace SKBKontur.Treller.Logger
{
    public class LogService : ILogService
    {
        public event EventHandler<ErrorEventArgs> OnError;
        public void RegisterError(string message)
        {
            OnError?.Invoke(this, new ErrorEventArgs {Message = message});
        }

        public void RegisterError(string message, Exception e)
        {
            OnError?.Invoke(this, new ErrorEventArgs {Message = message, Exception = e});
        }
    }
}