using System;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Infrastructure.Sugar;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations
{
    public class ScheduledRegularOperation : IRegularOperation
    {
        private static readonly object operationLock = new object();
        private readonly IDateTimeFactory dateTimeFactory;

        public string Name { get; }
        public OperationState State { get; private set; }
        public TimeSpan RunPeriod { get; }
        private readonly TimeSpan minTimeToRun;
        private readonly TimeSpan maxTimeToRun;
        private readonly Action action;

        public ScheduledRegularOperation(IDateTimeFactory dateTimeFactory, string name, TimeSpan runPeriod, TimeSpan minTimeToRun, TimeSpan maxTimeToRun, Action action)
        {
            Name = name;
            RunPeriod = runPeriod;
            this.dateTimeFactory = dateTimeFactory;
            this.minTimeToRun = minTimeToRun;
            this.maxTimeToRun = maxTimeToRun;
            this.action = action;
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
                    var now = dateTimeFactory.Now;
                    var nowDate = now.Date;
                    if (now > nowDate.Add(minTimeToRun) && now < nowDate.Add(maxTimeToRun))
                    {
                        action.Invoke();
                    }
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