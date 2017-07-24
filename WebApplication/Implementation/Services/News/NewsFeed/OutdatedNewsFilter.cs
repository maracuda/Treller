using System.Collections.Generic;
using Antlr.Runtime.Misc;
using Infrastructure.Common;
using WebApplication.Implementation.Services.News.Domain.Builders;
using WebApplication.Implementation.Services.News.Domain.Models;

namespace WebApplication.Implementation.Services.News.NewsFeed
{
    public class OutdatedNewsFilter : IOutdatedNewsFilter
    {
        private readonly IOutdatedBoardCardBuilder outdatedBoardCardBuilder;
        private readonly IDateTimeFactory dateTimeFactory;

        public OutdatedNewsFilter(
            IOutdatedBoardCardBuilder outdatedBoardCardBuilder,
            IDateTimeFactory dateTimeFactory)
        {
            this.outdatedBoardCardBuilder = outdatedBoardCardBuilder;
            this.dateTimeFactory = dateTimeFactory;
        }

        public TaskNew[] FilterOutdated(IEnumerable<TaskNew> taskNews)
        {
            return Filter(taskNews, outdatedNew => outdatedNew.IsOutdated(dateTimeFactory.UtcNow));
        }

        public TaskNew[] FilterActual(IEnumerable<TaskNew> taskNews)
        {
            return Filter(taskNews, agingCardModel => !agingCardModel.IsOutdated(dateTimeFactory.UtcNow));
        }

        private TaskNew[] Filter(IEnumerable<TaskNew> taskNews, Func<OutdatedBoardCardModel, bool> selector)
        {
            var result = new List<TaskNew>();
            foreach (var taskNew in taskNews)
            {
                var maybeModel = outdatedBoardCardBuilder.TryBuildModel(taskNew.TaskId);
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