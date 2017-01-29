using System;
using SKBKontur.Treller.OperationalService.Operations;

namespace SKBKontur.Treller.OperationalService
{
    public interface IOperationalService : IDisposable
    {
        void Register(IRegularOperation operation, ScheduleParams scheduleParams);
    }
}