using System;
using log4net;

namespace SKBKontur.Treller.Logger.Log4Net
{
    public class Log4NetLogger : ILogger
    {
        private readonly ILog log;
        private readonly ILogService logService;

        public Log4NetLogger(
            ILog log,
            ILogService logService)
        {
            this.log = log;
            this.logService = logService;
        }

        public void LogError(string message)
        {
            log.Error(message);
            logService.RegisterError(message);
        }

        public void LogError(string message, Exception e)
        {
            log.Error(message, e);
            logService.RegisterError(message, e);
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