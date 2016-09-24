using System.Linq;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Storage;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Actualization
{
    public class NewsActualizator : INewsActualizator
    {
        private readonly ITaskNewStorage taskNewStorage;
        private readonly IAgingCardsFilter agingCardsFilter;

        public NewsActualizator(
            ITaskNewStorage taskNewStorage,
            IAgingCardsFilter agingCardsFilter)
        {
            this.taskNewStorage = taskNewStorage;
            this.agingCardsFilter = agingCardsFilter;
        }

        public void ActualizeAll(int batchSize = 30)
        {
            long currentTimestamp = 0;
            TaskNew[] newsBatch;
            do
            {
                newsBatch = taskNewStorage.Enumerate(currentTimestamp, batchSize);
                var uselessTaskNews = agingCardsFilter.FilterAging(newsBatch);
                taskNewStorage.Delete(uselessTaskNews);
                currentTimestamp = newsBatch.Max(x => x.TimeStamp);
            } while (newsBatch.Length > 0);
        }
    }
}