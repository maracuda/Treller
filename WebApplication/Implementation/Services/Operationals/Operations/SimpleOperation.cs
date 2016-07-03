using System;
using SKBKontur.Infrastructure.Sugar;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations
{
    public class SimpleOperation : RegularOperation
    {
        private static readonly object operationLock = new object();

        public SimpleOperation(string name, TimeSpan runPeriod, Action action) : base(name, runPeriod, action)
        {
        }

        public override Maybe<Exception> Run()
        {
            try
            {
                if (State != OperationState.Idle)
                    return null;

                lock (operationLock)
                {
                    if (State != OperationState.Idle)
                        return null;

                    State = OperationState.Running;
                    action.Invoke();
                    return null;
                }
            }
            catch (Exception e)
            {
                return e;
            }
            finally
            {
                State = OperationState.Idle;
            }
        }
    }
}