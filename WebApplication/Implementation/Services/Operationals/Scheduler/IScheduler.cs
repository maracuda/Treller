using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Scheduler
{
    public interface IScheduler
    {
        void Register(string operationName, ScheduleParams scheduleParams);
        bool IsItTimeToLaunch(string operationName, DateTime now);
    }
}