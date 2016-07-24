using System;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Scheduler;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals
{
    public interface IOperationalService : IDisposable
    {
        void Register(IRegularOperation operation, ScheduleParams scheduleParams);
    }
}