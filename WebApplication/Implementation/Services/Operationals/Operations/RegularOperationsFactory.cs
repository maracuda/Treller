using System;
using SKBKontur.Infrastructure.Common;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations
{
    public class RegularOperationsFactory : IRegularOperationsFactory
    {
        private readonly IDateTimeFactory dateTimeFactory;

        public RegularOperationsFactory(IDateTimeFactory dateTimeFactory)
        {
            this.dateTimeFactory = dateTimeFactory;
        }

        public IRegularOperation Create(string name, TimeSpan runPeriod, Action action)
        {
            return new SimpleOperation(name, runPeriod, action);
        }

        public IRegularOperation Create(string name, TimeSpan runPeriod, TimeSpan minTimeToRun, TimeSpan maxTimeToRun, Action action)
        {
            return new ScheduledRegularOperation(dateTimeFactory, name, runPeriod, minTimeToRun, maxTimeToRun, action);
        }
    }
}