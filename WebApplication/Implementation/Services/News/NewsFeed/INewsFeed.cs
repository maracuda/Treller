using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.NewsFeed
{
    public interface INewsFeed
    {
        void AddNews(IEnumerable<TaskNew> news);
        void Refresh(int batchSize = 30);
    }
}