using System;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Treller.Storage;

namespace SKBKontur.Treller.OperationalService.Operations
{
    public class RegularOperationsFactory : IRegularOperationsFactory
    {
        private readonly IDateTimeFactory dateTimeFactory;
        private readonly IKeyValueStorage keyValueStorage;

        public RegularOperationsFactory(
            IDateTimeFactory dateTimeFactory,
            IKeyValueStorage keyValueStorage
        )
        {
            this.dateTimeFactory = dateTimeFactory;
            this.keyValueStorage = keyValueStorage;
        }

        public IRegularOperation Create(string name, Action action)
        {
            return new SimpleOperation(name, action);
        }

        public IRegularOperation Create(string name, Func<long, long> enumeration, Func<long> defaultTimetampFunc)
        {
            return new EnumerationOperation(keyValueStorage, name, enumeration, defaultTimetampFunc);
        }
    }
}