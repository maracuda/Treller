using System;
using System.Collections.Concurrent;
using System.Timers;
using SKBKontur.Infrastructure.Sugar;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals
{
    public interface IOperationalService2 : IDisposable
    {
        void RegisterRegularProccess(string name, Action action, TimeSpan period);
    }

    public class OperationalService2 : IOperationalService2
    {
        private readonly IErrorService errorService;
        private readonly ConcurrentDictionary<Timer, string> namesIndex = new ConcurrentDictionary<Timer, string>();
        private readonly ConcurrentDictionary<string, Action> actionsIndex = new ConcurrentDictionary<string, Action>();

        public OperationalService2(IErrorService errorService)
        {
            this.errorService = errorService;
        }

        public void Dispose()
        {
            foreach (var timerNamePair in namesIndex)
            {
                var timer = timerNamePair.Key;
                string name;
                namesIndex.TryRemove(timer, out name);
                timer.Dispose();
            }
        }

        public void RegisterRegularProccess(string name, Action action, TimeSpan period)
        {
            if (actionsIndex.ContainsKey(name))
                return;

            var timer = new Timer(period.TotalMilliseconds) { Enabled = true };
            namesIndex.AddOrUpdate(timer, t => name, (t, n) => name);
            actionsIndex.AddOrUpdate(name, n => action, (n, a) => action);
            timer.Elapsed += SafeExcute;
        }

        private void SafeExcute(object sender, ElapsedEventArgs elapsedEventArg)
        {
            try
            {
                var name = GetName(sender as Timer);
                if (name.HasNoValue)
                {
                    errorService.SendError("Fail to find action to run regular process", new Exception());
                    return;
                }

                actionsIndex[name.Value].Invoke();
            }
            catch (Exception e)
            {
                errorService.SendError("Fail to run regular process", e);
            }
        }

        private Maybe<string> GetName(Timer timer)
        {
            if (timer == null || !namesIndex.ContainsKey(timer))
                return null;
            return namesIndex[timer];
        }
    }
}