using System;
using Infrastructure.Sugar;

namespace OperationalService.Operations
{
    public interface IRegularOperation
    {
        string Name { get; }
        OperationState State { get; }
        Maybe<Exception> Run();
    }
}