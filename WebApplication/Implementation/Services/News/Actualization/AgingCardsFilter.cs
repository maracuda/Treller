using System.Collections.Generic;
using Antlr.Runtime.Misc;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Domain.Builders;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Domain.Models;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Actualization
{
    public class AgingCardsFilter : IAgingCardsFilter
    {
        private readonly IAgingBoardCardBuilder agingBoardCardBuilder;
        private readonly IDateTimeFactory dateTimeFactory;

        public AgingCardsFilter(
            IAgingBoardCardBuilder agingBoardCardBuilder,
            IDateTimeFactory dateTimeFactory)
        {
            this.agingBoardCardBuilder = agingBoardCardBuilder;
            this.dateTimeFactory = dateTimeFactory;
        }

        public TaskNew[] FilterAging(IEnumerable<TaskNew> taskNews)
        {
            return Filter(taskNews, agingCardModel => agingCardModel.IsGrowedOld(dateTimeFactory.UtcNow));
        }

        public TaskNew[] FilterFresh(IEnumerable<TaskNew> taskNews)
        {
            return Filter(taskNews, agingCardModel => !agingCardModel.IsGrowedOld(dateTimeFactory.UtcNow));
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