using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.NewsFeed
{
    public interface IOutdatedNewsFilter
    {
        TaskNew[] FilterOutdated(IEnumerable<TaskNew> taskNews);
        TaskNew[] FilterActual(IEnumerable<TaskNew> taskNews);
    }
}