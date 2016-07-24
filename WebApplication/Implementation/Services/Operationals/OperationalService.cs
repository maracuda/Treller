using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Timers;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Scheduler;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals
{
    public class OperationalService : IOperationalService
    {
        private readonly IErrorService errorService;
        private readonly IOperationsLauncher operationsLauncher;
        private readonly IScheduler scheduler;
        private readonly ConcurrentDictionary<Timer, IRegularOperation> operationsIndex = new ConcurrentDictionary<Timer, IRegularOperation>();


        public OperationalService(
            IErrorService errorService,
            IOperationsLauncher operationsLauncher,
            IScheduler scheduler)
        {
            this.errorService = errorService;
            this.operationsLauncher = operationsLauncher;
            this.scheduler = scheduler;
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

        public void Register(IRegularOperation operation, ScheduleParams scheduleParams)
        {
            if (operationsIndex.Values.Any(x => x.Name.Equals(operation.Name)))
                return;

            scheduler.Register(operation.Name, scheduleParams);
            var timer = new Timer(scheduleParams.PollingPeriod.TotalMilliseconds) {Enabled = true};
            operationsIndex.AddOrUpdate(timer, t => operation, (t, n) => operation);
            timer.Elapsed += Excute;
        }

        private void Excute(object sender, ElapsedEventArgs elapsedEventArg)
        {
            var timer = sender as Timer;
            if (timer == null || !operationsIndex.ContainsKey(timer))
            {
                errorService.SendError("Fail to find action to run operation", new Exception());
                return;
            }

            operationsLauncher.SafeLaunch(operationsIndex[timer]);
        }
    }
}