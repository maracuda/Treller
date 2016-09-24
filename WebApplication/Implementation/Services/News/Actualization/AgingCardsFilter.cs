using System.Collections.Generic;
using Antlr.Runtime.Misc;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Domain.Builders;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Domain.Models;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Actualization
{
    public class AgingCardsFilter : IAgingCardsFilter
    {
        private readonly IAgingBoardCardBuilder agingBoardCardBuilder;

        public AgingCardsFilter(IAgingBoardCardBuilder agingBoardCardBuilder)
        {
            this.agingBoardCardBuilder = agingBoardCardBuilder;
        }

        public TaskNew[] FilterAging(IEnumerable<TaskNew> taskNews)
        {
            return Filter(taskNews, agingCardModel => agingCardModel.IsGrowedOld());
        }

        public TaskNew[] FilterFresh(IEnumerable<TaskNew> taskNews)
        {
            return Filter(taskNews, agingCardModel => !agingCardModel.IsGrowedOld());
        }

        private TaskNew[] Filter(IEnumerable<TaskNew> taskNews, Func<AgingBoardCardModel, bool> selector)
        {
            var result = new List<TaskNew>();
            foreach (var taskNew in taskNews)
            {
                var maybeModel = agingBoardCardBuilder.TryBuildModel(taskNew.TaskId);
                if (maybeModel.HasValue)
                {
                    if (selector(maybeModel.Value))
                    {
                        result.Add(taskNew);
                    }
                }
            }
            return result.ToArray();
        }
    }
}