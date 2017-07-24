using System;
using System.Collections.Concurrent;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository.Hierarchy;

namespace Logger.Log4Net
{
    public class Log4NetLoggerFactory : ILoggerFactory
    {
        private static readonly object configLock = new object();
        private static bool isConfigured;
        private readonly ILogService logService;
        private readonly ConcurrentDictionary<Type, ILogger> loggersMap = new ConcurrentDictionary<Type, ILogger>();

        public Log4NetLoggerFactory(ILogService logService)
        {
            this.logService = logService;

        }

        public ILogger Get<T>()
        {
            if (!isConfigured)
            {
                lock (configLock)
                {
                    if (!isConfigured)
                    {
                        Configure();
                        isConfigured = true;
                    }
                }
            }

            return loggersMap.GetOrAdd(typeof(T), type => new Log4NetLogger(LogManager.GetLogger(typeof(T)), logService));
        }

        private static void Configure()
        {
            var hierarchy = (Hierarchy)LogManager.GetRepository();

            var patternLayout = new PatternLayout
            {
                ConversionPattern = "%date [%thread] %-5level %logger - %message%newline"
            };
            patternLayout.ActivateOptions();

            var roller = new RollingFileAppender
            {
                AppendToFile = false,
                File = @"..\..\TrellerLogs\log.txt",
                Layout = patternLayout,
                MaxSizeRollBackups = 5,
                MaximumFileSize = "1GB",
                RollingStyle = RollingFileAppender.RollingMode.Size,
                StaticLogFileName = true
            };
            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);

            var memory = new MemoryAppender();
            memory.ActivateOptions();
            hierarchy.Root.AddAppender(memory);

            hierarchy.Root.Level = Level.Info;
            hierarchy.Configured = true;
        }
    }
}