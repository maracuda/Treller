using System;
using OperationalService.Operations;

namespace OperationalService
{
    public interface IOperationalService : IDisposable
    {
        void Register(IRegularOperation operation, ScheduleParams scheduleParams);
    }
}