using System;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.OperationsLog;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Scheduler;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals
{
    public class OperationsLauncher : IOperationsLauncher
    {
        private readonly IErrorService errorService;
        private readonly IDateTimeFactory dateTimeFactory;
        private readonly IScheduler scheduler;
        private readonly IOperationsLog operationsLog;

        public OperationsLauncher(
            IErrorService errorService,
            IDateTimeFactory dateTimeFactory,
            IScheduler scheduler,
            IOperationsLog operationsLog)
        {
            this.errorService = errorService;
            this.dateTimeFactory = dateTimeFactory;
            this.scheduler = scheduler;
            this.operationsLog = operationsLog;
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
                        errorService.SendError($"Operation with name {operation.Name} failed", operationResult.Value);
                    }
                    var endDateTime = dateTimeFactory.Now;
                    operationsLog.Append(operation.Name, beginDateTime, endDateTime, !operationResult.HasValue);
                }
            }
            catch (Exception e)
            {
                errorService.SendError($"Fail to launch operation {operation.Name}", e);
            }
        }
    }
}