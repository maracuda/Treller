using System;
using System.Timers;
using SKBKontur.Treller.WebApplication.Services.News;

namespace SKBKontur.Treller.WebApplication.Services.TaskCacher
{
    public class OperationalService : IOperationalService, IDisposable
    {
        private readonly ITaskCacher taskCacher;
        private readonly INewsService newsService;
        private bool isTimerInProgress;
        private readonly Timer timer;
        private DateTime lastUpdateUtc = DateTime.UtcNow.AddDays(-2);

        public OperationalService(ITaskCacher taskCacher, INewsService newsService)
        {
            this.taskCacher = taskCacher;
            this.newsService = newsService;
            timer = new Timer(60000);
            timer.Elapsed += TimerOnElapsed;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (isTimerInProgress)
            {
                return;
            }

            isTimerInProgress = true;

            try
            {
                if (taskCacher.TryActualize(lastUpdateUtc) && newsService.TryRefresh(lastUpdateUtc))
                {
                    lastUpdateUtc = elapsedEventArgs.SignalTime;
                }
            }
            finally
            {
                isTimerInProgress = false;
            }
        }

        public void Dispose()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
        }

        public void Start()
        {
            timer.Start();
        }
    }
}