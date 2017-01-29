using System;
using SKBKontur.Infrastructure.Sugar;

namespace SKBKontur.Treller.OperationalService.Operations
{
    public interface IRegularOperation
    {
        string Name { get; }
        OperationState State { get; }
        Maybe<Exception> Run();
    }
}