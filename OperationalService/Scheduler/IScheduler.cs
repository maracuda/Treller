using System;

namespace OperationalService.Scheduler
{
    public interface IScheduler
    {
        void Register(string operationName, ScheduleParams scheduleParams);
        bool IsItTimeToLaunch(string operationName, DateTime now);
    }
}