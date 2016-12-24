using System;

namespace SKBKontur.Treller.Logger
{
    public class ErrorEventArgs : EventArgs
    {
        public string Message { get; set; }
        public Exception Exception { get; set; }
    }
}