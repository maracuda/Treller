using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Actualization
{
    public interface IAgingCardsFilter
    {
        TaskNew[] FilterAging(IEnumerable<TaskNew> taskNews);
        TaskNew[] FilterFresh(IEnumerable<TaskNew> taskNews);
    }
}