using System;
using SKBKontur.Infrastructure.Sugar;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations
{
    public class SimpleOperation : IRegularOperation
    {
        private static readonly object operationLock = new object();

        private readonly Action action;
        public string Name { get; }
        public OperationState State { get; private set; }

        public SimpleOperation(string name, Action action)
        {
            this.action = action;
            State = OperationState.Idle;
            Name = name;
        }

        public Maybe<Exception> Run()
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