using System;
using SKBKontur.Infrastructure.Sugar;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations
{
    public abstract class RegularOperation
    {
        protected readonly Action action;
        public string Name { get; private set; }
        public TimeSpan RunPeriod { get; private set; }

        protected RegularOperation(string name, TimeSpan runPeriod, Action action)
        {
            this.action = action;
            Name = name;
            RunPeriod = runPeriod;
        }

        public abstract Maybe<Exception> Run();
    }
}