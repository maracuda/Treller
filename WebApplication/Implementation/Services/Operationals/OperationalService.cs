using System;
using System.Timers;
using SKBKontur.BlocksMapping.Blocks;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Abstractions;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;
using SKBKontur.Treller.WebApplication.Implementation.Services.Digest;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;
using SKBKontur.Treller.WebApplication.Implementation.Services.News;
using SKBKontur.Treller.WebApplication.Implementation.Services.TaskCacher;
using SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.Blocks;
using SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.ViewModels;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals
{
    public class OperationalService : IOperationalService, IDisposable
    {
        private readonly ITaskCacher taskCacher;
        private readonly INewsService newsService;
        private readonly ICachedFileStorage cachedFileStorage;
        private readonly IBlocksBuilder blocksBuilder;
        private readonly IDigestService digestService;
        private readonly IErrorService errorService;
        private static bool _isTimerInProgress;
        private readonly Timer timer;
        private DateTime lastUpdateUtc;
        private const string TimestampFileName = "TrellerCacheCurrentTimestamp.json";
        private DateTime lastError = DateTime.MinValue;
        private DateTime lastNewsError = DateTime.MinValue;

        public OperationalService(ITaskCacher taskCacher,
                                  INewsService newsService,
                                  ICachedFileStorage cachedFileStorage,
                                  IBlocksBuilder blocksBuilder,
                                  IDigestService digestService,
                                  IErrorService errorService)
        {
            this.taskCacher = taskCacher;
            this.blocksBuilder = blocksBuilder;
            this.digestService = digestService;
            this.errorService = errorService;
            this.newsService = newsService;
            this.cachedFileStorage = cachedFileStorage;

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
                var time = DateTime.UtcNow;

                var actualizeResult = false;
                try
                {
                    actualizeResult = taskCacher.TryActualize(lastUpdateUtc);
                }
                catch (Exception ex)
                {
                    errorService.SendError("Актуализатор кэша не смог отработать!", ex);
                    return;
                }

                if (actualizeResult)
                {
                    lastUpdateUtc = time;
                    cachedFileStorage.Write(TimestampFileName, lastUpdateUtc.Ticks);
                }
                else
                {
                    var warmedBlocks = blocksBuilder.BuildBlocks(ContextKeys.TasksKey, new[] { typeof(BoardsBlock), typeof(CardListBlock) }, new CardListEnterModel { BoardIds = new string[0], ShowMode = ShowMode.All }).Result;
                }

                try
                {
                    newsService.Refresh();
                }
                catch (Exception ex)
                {
                    errorService.SendError("Проблема в обновлении данных для новостей", ex);
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

                try
                {
                    digestService.SendAllToDigest();
                }
                catch (Exception ex)
                {
                    if ((DateTime.Now - lastError).TotalDays > 1)
                    {
                        errorService.SendError("Не смог отправить в дайджест!", ex);
                    }
                    lastError = DateTime.Now;
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