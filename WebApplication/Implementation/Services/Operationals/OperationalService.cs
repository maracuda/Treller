using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Timers;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals
{
    public class OperationalService : IOperationalService
    {
        private readonly IErrorService errorService;
        private readonly ConcurrentDictionary<Timer, IRegularOperation> operationsIndex = new ConcurrentDictionary<Timer, IRegularOperation>();


        public OperationalService(IErrorService errorService)
        {
            this.errorService = errorService;
        }

        public void Dispose()
        {
            foreach (var timerNamePair in operationsIndex)
            {
                var timer = timerNamePair.Key;
                IRegularOperation operation;
                operationsIndex.TryRemove(timer, out operation);
                timer.Dispose();
            }
        }

        public void Register(IRegularOperation operation)
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
                var timer = sender as Timer;
                if (timer == null || !operationsIndex.ContainsKey(timer))
                {
                    errorService.SendError("Fail to find action to run regular process", new Exception());
                    return;
                }

                var operation = operationsIndex[timer];
                var operationResult = operation.Run();
                if (operationResult.HasValue)
                {
                    errorService.SendError($"Operation with name {operation.Name} failed", operationResult.Value);
                }
            }
            catch (Exception e)
            {
                errorService.SendError("Fail to run regular process", e);
            }
        }
    }
}