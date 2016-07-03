using System;
using SKBKontur.Infrastructure.Sugar;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations
{
    public class EnumerationOperation : IRegularOperation
    {
        public string Name { get; }
        public OperationState State { get; }
        public TimeSpan RunPeriod { get; }

        public Maybe<Exception> Run()
        {
            throw new NotImplementedException();
        }
    }
}