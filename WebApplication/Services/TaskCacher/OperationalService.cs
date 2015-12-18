using System;
using System.Timers;
using SKBKontur.Treller.WebApplication.Storages;
using SKBKontur.BlocksMapping.Blocks;
using SKBKontur.Treller.WebApplication.Blocks;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.Blocks;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.ViewModels;
using SKBKontur.Treller.WebApplication.Services.News;

namespace SKBKontur.Treller.WebApplication.Services.TaskCacher
{
    public class OperationalService : IOperationalService, IDisposable
    {
        private readonly ITaskCacher taskCacher;
        private readonly INewsService newsService;
        private readonly ICachedFileStorage cachedFileStorage;
        private readonly IBlocksBuilder blocksBuilder;
        private bool isTimerInProgress;
        private readonly Timer timer;
        private DateTime lastUpdateUtc;
        private const string TimestampFileName = "TrellerCacheCurrentTimestamp.json";

        public OperationalService(ITaskCacher taskCacher, INewsService newsService, ICachedFileStorage cachedFileStorage, IBlocksBuilder blocksBuilder)
        {
            this.taskCacher = taskCacher;
            this.blocksBuilder = blocksBuilder;
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
                if (taskCacher.TryActualize(lastUpdateUtc))
                {
                    lastUpdateUtc = time;
                    cachedFileStorage.Write(TimestampFileName, lastUpdateUtc.Ticks);
                }
                else
                {
                    var warmedBlocks = blocksBuilder.BuildBlocks(ContextKeys.TasksKey, new[] { typeof(BoardsBlock), typeof(CardListBlock) }, new CardListEnterModel { BoardIds = new string[0], ShowMode = ShowMode.All }).Result;
                }
                newsService.Refresh();

                var now = DateTime.Now;
                if (((now.Hour >= 17 && now.Minute > 20 && now.Hour < 18) || (now.Hour >= 9 && now.Hour < 10)) && newsService.IsAnyNewsExists())
                {
                    newsService.SendNews();
                    newsService.SendTechnicalNews();
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

        public void Actualize()
        {
            TimerOnElapsed(null, null);
        }
    }
}