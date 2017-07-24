using System.Collections.Concurrent;
using System.Linq;
using System.Timers;
using Logger;
using OperationalService.Launcher;
using OperationalService.Operations;
using OperationalService.Scheduler;

namespace OperationalService
{
    public class OperationalService : IOperationalService
    {
        private readonly IOperationsLauncher operationsLauncher;
        private readonly IScheduler scheduler;
        private readonly ILoggerFactory loggerFactory;
        private readonly ConcurrentDictionary<Timer, IRegularOperation> operationsIndex = new ConcurrentDictionary<Timer, IRegularOperation>();

        public OperationalService(IOperationsLauncher operationsLauncher,
            IScheduler scheduler,
            ILoggerFactory loggerFactory)
        {
            this.operationsLauncher = operationsLauncher;
            this.scheduler = scheduler;
            this.loggerFactory = loggerFactory;
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
                loggerFactory.Get<OperationalService>().LogError("Fail to find action to run operation");
                return;
            }

            operationsLauncher.SafeLaunch(operationsIndex[timer]);
        }
    }
}