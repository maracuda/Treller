using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations
{
    public interface IRegularOperationsFactory
    {
        RegularOperation Create(string name, TimeSpan runPeriod, Action action);
        RegularOperation Create(string name, TimeSpan runPeriod, TimeSpan minTimeToRun, TimeSpan maxTimeToRun, Action action);
    }
}