using System;
using System.Collections.Concurrent;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.OperationsLog;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Scheduler
{
    public class Scheduler : IScheduler
    {
        private readonly IOperationsLog operationsLog;
        private readonly ConcurrentDictionary<string, ScheduleParams> paramsIndex;

        public Scheduler(
            IOperationsLog operationsLog)
        {
            this.operationsLog = operationsLog;
            paramsIndex = new ConcurrentDictionary<string, ScheduleParams>();
        }

        public void Register(string operationName, ScheduleParams scheduleParams)
        {
            paramsIndex.AddOrUpdate(operationName, scheduleParams, (opName, sParams) => scheduleParams);
        }

        public bool IsItTimeToLaunch(string operationName, DateTime now)
        {
            if (!paramsIndex.ContainsKey(operationName))
                throw new Exception($"Schedule params for operation {operationName} was not found.");

            var scheduleParams = paramsIndex[operationName];
            switch (scheduleParams.Mode)
            {
                case ScheduleMode.Anytime:
                    return true;
                case ScheduleMode.EveryDay:
                {
                    if (!scheduleParams.At.HasValue)
                        throw new ArgumentException($"Schedule params for mode {scheduleParams.Mode} should define field At.");
                    if (now < now.Date.Add(scheduleParams.At.Value))
                        return false;
                    var launchesNumber = operationsLog.CountLanches(operationName, now.Date, now.Date.AddDays(1));
                    return launchesNumber == 0;
                }
                default:
                    throw new NotSupportedException($"Mode {scheduleParams.Mode} does not supported.");
            }
        }
    }
}