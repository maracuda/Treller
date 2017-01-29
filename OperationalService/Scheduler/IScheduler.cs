using System;

namespace SKBKontur.Treller.OperationalService.Scheduler
{
    public interface IScheduler
    {
        void Register(string operationName, ScheduleParams scheduleParams);
        bool IsItTimeToLaunch(string operationName, DateTime now);
    }
}