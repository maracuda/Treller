using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Timers;
using SKBKontur.Infrastructure.Sugar;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals
{
    public class OperationalService2 : IOperationalService2
    {
        private readonly IErrorService errorService;
        private readonly ConcurrentDictionary<Timer, RegularOperation> operationsIndex = new ConcurrentDictionary<Timer, RegularOperation>();


        public OperationalService2(IErrorService errorService)
        {
            this.errorService = errorService;
        }

        public void Dispose()
        {
            foreach (var timerNamePair in operationsIndex)
            {
                var timer = timerNamePair.Key;
                RegularOperation operation;
                operationsIndex.TryRemove(timer, out operation);
                timer.Dispose();
            }
        }

        public void Register(RegularOperation operation)
        {
            if (operationsIndex.Values.Any(x => x.Name.Equals(operation.Name)))
                return;

            var timer = new Timer(operation.RunPeriod.TotalMilliseconds) {Enabled = true};
            operationsIndex.AddOrUpdate(timer, t => operation, (t, n) => operation);
            timer.Elapsed += SafeExcute;
        }

        private void SafeExcute(object sender, ElapsedEventArgs elapsedEventArg)
        {
            try
            {
                var operation = FindOperation(sender as Timer);
                if (operation.HasNoValue)
                {
                    errorService.SendError("Fail to find action to run regular process", new Exception());
                    return;
                }

                var operationResult = operation.Value.Run();
                if (operationResult.HasValue)
                {
                    errorService.SendError($"Oparation with name {operation.Value.Name} failed", operationResult.Value);
                }
            }
            catch (Exception e)
            {
                errorService.SendError("Fail to run regular process", e);
            }
        }

        private Maybe<RegularOperation> FindOperation(Timer timer)
        {
            if (timer == null || !operationsIndex.ContainsKey(timer))
                return null;
            return operationsIndex[timer];
        }
    }
}