using System;
using SKBKontur.Infrastructure.Sugar;
using SKBKontur.Treller.Storage;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations
{
    public class EnumerationOperation : IRegularOperation
    {
        private static readonly object operationLock = new object();
        private readonly IKeyValueStorage keyValueStorage;

        private readonly Func<long, long> enumration;
        private readonly Func<long> defaultTimestampFunc;
        private long? timestamp = null;
        public string Name { get; }
        public OperationState State { get; private set; }

        public EnumerationOperation(IKeyValueStorage keyValueStorage, string name, Func<long, long> enumration, Func<long> defaultTimestampFunc)
        {
            this.keyValueStorage = keyValueStorage;
            Name = name;
            State = OperationState.Idle;
            this.enumration = enumration;
            this.defaultTimestampFunc = defaultTimestampFunc;
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
                    if (!timestamp.HasValue)
                    {
                        timestamp = keyValueStorage.Find<long>($"{Name}Timestamp.json");
                        if (timestamp.Value == 0)
                            timestamp = defaultTimestampFunc.Invoke();
                    }
                    timestamp = enumration.Invoke(timestamp.Value);
                    keyValueStorage.Write($"{Name}Timestamp.json", timestamp.Value);
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