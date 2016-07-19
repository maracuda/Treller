using System.Collections.Generic;
using SKBKontur.Infrastructure.Common;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Import
{
    public class TaskNewConverter : ITaskNewConverter
    {
        private readonly IDateTimeFactory dateTimeFactory;
        private readonly ITextNewParser[] textNewParsers;

        public TaskNewConverter(
            IDateTimeFactory dateTimeFactory,
            ITextNewParser[] textNewParsers)
        {
            this.dateTimeFactory = dateTimeFactory;
            this.textNewParsers = textNewParsers;
        }

        public List<TaskNew> Convert(string boardId, BoardListCardInfo cardInfo)
        {
            var result = new List<TaskNew>();
            foreach (var textNewParser in textNewParsers)
            {
                var parseResult = textNewParser.TryParse(cardInfo.Desc);
                if (parseResult.HasValue && !string.IsNullOrWhiteSpace(parseResult.Value))
                {
                    result.Add(new TaskNew
                    {
                        BoardId = boardId,
                        TaskId = cardInfo.Id,
                        Title = cardInfo.Name,
                        Text = parseResult.Value,
                        DeliveryChannel = textNewParser.DeliveryChannelType,
                        DoNotDeliverUntil = cardInfo.Due,
                        TimeStamp = dateTimeFactory.UtcTicks
                    });
                }
            }
            return result;
        }
    }
}