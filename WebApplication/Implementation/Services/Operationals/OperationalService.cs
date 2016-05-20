using System;
using System.Timers;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;
using SKBKontur.Treller.WebApplication.Implementation.Services.News;
using SKBKontur.Treller.WebApplication.Implementation.Services.TaskCacher;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals
{
    public class OperationalService : IOperationalService
    {
        private readonly ITaskCacher taskCacher;
        private readonly INewsService newsService;
        private readonly IErrorService errorService;
        private static bool _isTimerInProgress;
        private readonly Timer timer;
        private readonly DateTime lastUpdateUtc;
        private const string TimestampFileName = "TrellerCacheCurrentTimestamp.json";
        private DateTime lastNewsError = DateTime.MinValue;

        public OperationalService(ITaskCacher taskCacher,
                                  INewsService newsService,
                                  ICachedFileStorage cachedFileStorage,
                                  IErrorService errorService)
        {
            this.taskCacher = taskCacher;
            this.errorService = errorService;
            this.newsService = newsService;

            var timestamp = cachedFileStorage.Find<long>(TimestampFileName);
            lastUpdateUtc = timestamp == 0 ? DateTime.UtcNow.AddDays(-2) : new DateTime(timestamp);

            timer = new Timer(60000);
            timer.Elapsed += TimerOnElapsed;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (_isTimerInProgress)
            {
                return;
            }

            _isTimerInProgress = true;

            try
            {
                try
                {
                    taskCacher.TryActualize(lastUpdateUtc);
                }
                catch (Exception ex)
                {
                    errorService.SendError("Актуализатор кэша не смог отработать!", ex);
                    return;
                }

                var now = DateTime.Now;
                if (((now.Hour >= 18 && now.Minute > 20 && now.Hour < 19) || (now.Hour >= 12 && now.Hour < 13)) && newsService.IsAnyNewsExists())
                {
                    try
                    {
                        newsService.SendNews();
                        newsService.SendTechnicalNews();
                    }
                    catch (Exception ex)
                    {
                        if ((DateTime.Now - lastNewsError).TotalDays > 1)
                        {
                            errorService.SendError("Не смог отправить новости!", ex);
                        }
                        lastNewsError = DateTime.Now;
                    }
                }
            }
            finally
            {
                _isTimerInProgress = false;
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