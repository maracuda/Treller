using System;
using System.Timers;
using SKBKontur.Treller.WebApplication.Services.News;
using SKBKontur.Treller.WebApplication.Storages;

namespace SKBKontur.Treller.WebApplication.Services.TaskCacher
{
    public class OperationalService : IOperationalService, IDisposable
    {
        private readonly ITaskCacher taskCacher;
        private readonly INewsService newsService;
        private readonly ICachedFileStorage cachedFileStorage;
        private bool isTimerInProgress;
        private readonly Timer timer;
        private DateTime lastUpdateUtc;
        private const string TimestampFileName = "TrellerCacheCurrentTimestamp.json";

        public OperationalService(ITaskCacher taskCacher, INewsService newsService, ICachedFileStorage cachedFileStorage)
        {
            this.taskCacher = taskCacher;
            this.newsService = newsService;
            this.cachedFileStorage = cachedFileStorage;

            var timestamp = cachedFileStorage.Find<long>(TimestampFileName);
            lastUpdateUtc = timestamp == 0 ? DateTime.UtcNow.AddDays(-2) : new DateTime(timestamp);

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
                var time = DateTime.UtcNow;
                if (taskCacher.TryActualize(lastUpdateUtc) && newsService.TryRefresh(lastUpdateUtc))
                {
                    lastUpdateUtc = time;
                    cachedFileStorage.Write(TimestampFileName, lastUpdateUtc.Ticks);
                }

//                if (DateTime.Now.Hour > 18 && DateTime.Now.Hour < 19)
//                {
//                    newsService.SendNews(DateTime.Now.Date);
//                }
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

        public void Actualize()
        {
            TimerOnElapsed(null, null);
        }
    }
}