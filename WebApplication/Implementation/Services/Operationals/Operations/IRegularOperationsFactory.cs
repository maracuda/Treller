using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations
{
    public interface IRegularOperationsFactory
    {
        IRegularOperation Create(string name, TimeSpan runPeriod, Action action);
        IRegularOperation Create(string name, TimeSpan runPeriod, TimeSpan minTimeToRun, TimeSpan maxTimeToRun, Action action);
    }
}