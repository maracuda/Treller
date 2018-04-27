using System;
using log4net;

namespace Logger.Log4Net
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog log;

        public Log4NetLogger(
            ILog log)
        {
            this.log = log;
        }

        public void LogError(string message)
        {
            log.Error(message);
        }

        public void LogError(string message, Exception e)
        {
            log.Error(message, e);
        }

        public void LogInfo(string message)
        {
            log.Info(message);
        }

        public void LogInfo(string message, Exception e)
        {
            log.Info(message, e);
        }
    }
}