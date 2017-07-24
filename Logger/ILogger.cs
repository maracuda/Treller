using System;

namespace Logger
{
    public interface ILogger
    {
        void LogError(string message);
        void LogError(string message, Exception e);
        void LogInfo(string message);
        void LogInfo(string message, Exception e);
    }
}