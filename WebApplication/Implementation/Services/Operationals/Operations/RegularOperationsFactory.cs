using System;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations
{
    public class RegularOperationsFactory : IRegularOperationsFactory
    {
        private readonly IDateTimeFactory dateTimeFactory;
        private readonly ICachedFileStorage cachedFileStorage;

        public RegularOperationsFactory(
            IDateTimeFactory dateTimeFactory,
            ICachedFileStorage cachedFileStorage
        )
        {
            this.dateTimeFactory = dateTimeFactory;
            this.cachedFileStorage = cachedFileStorage;
        }

        public IRegularOperation Create(string name, TimeSpan runPeriod, Action action)
        {
            return new SimpleOperation(name, runPeriod, action);
        }

        public IRegularOperation Create(string name, TimeSpan runPeriod, TimeSpan minTimeToRun, TimeSpan maxTimeToRun, Action action)
        {
            return new ScheduledRegularOperation(dateTimeFactory, name, runPeriod, minTimeToRun, maxTimeToRun, action);
        }

        public IRegularOperation Create(string name, TimeSpan runPeriod, Func<long, long> enumeration, Func<long> defaultTimetampFunc)
        {
            return new EnumerationOperation(cachedFileStorage, name, runPeriod, enumeration, defaultTimetampFunc);
        }
    }
}