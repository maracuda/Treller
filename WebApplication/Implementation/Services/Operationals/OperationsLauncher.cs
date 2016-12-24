using System;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Treller.Logger;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.OperationsLog;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Scheduler;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals
{
    public class OperationsLauncher : IOperationsLauncher
    {
        private readonly IDateTimeFactory dateTimeFactory;
        private readonly IScheduler scheduler;
        private readonly IOperationsLog operationsLog;
        private readonly ILoggerFactory loggerFactory;

        public OperationsLauncher(
            IDateTimeFactory dateTimeFactory,
            IScheduler scheduler,
            IOperationsLog operationsLog,
            ILoggerFactory loggerFactory)
        {
            this.dateTimeFactory = dateTimeFactory;
            this.scheduler = scheduler;
            this.operationsLog = operationsLog;
            this.loggerFactory = loggerFactory;
        }

        public void SafeLaunch(IRegularOperation operation)
        {
            try
            {
                var beginDateTime = dateTimeFactory.Now;
                if (scheduler.IsItTimeToLaunch(operation.Name, beginDateTime))
                {
                    var operationResult = operation.Run();
                    if (operationResult.HasValue)
                    {
                        loggerFactory.Get<OperationsLauncher>().LogError($"Operation with name {operation.Name} failed", operationResult.Value);
                    }
                    var endDateTime = dateTimeFactory.Now;
                    operationsLog.Append(operation.Name, beginDateTime, endDateTime, !operationResult.HasValue);
                }
            }
            catch (Exception e)
            {
                loggerFactory.Get<OperationsLauncher>().LogError($"Fail to launch operation {operation.Name}", e);
            }
        }
    }
}