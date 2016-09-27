using System;
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

        public List<TaskNew> Convert(string boardId, string cardId, string cardName, string cardDesc, DateTime? cardDueDate)
        {
            var result = new List<TaskNew>();
            foreach (var textNewParser in textNewParsers)
            {
                var parseResult = textNewParser.TryParse(cardDesc);
                if (parseResult.HasValue && !string.IsNullOrWhiteSpace(parseResult.Value))
                {
                    result.Add(new TaskNew
                    {
                        BoardId = boardId,
                        TaskId = cardId,
                        Title = cardName,
                        Text = parseResult.Value,
                        DeliveryChannel = textNewParser.DeliveryChannelType,
                        DoNotDeliverUntil = cardDueDate,
                        TimeStamp = dateTimeFactory.UtcTicks
                    });
                }
            }
            return result;
        }

        public List<TaskNew> Convert(BoardList boardList)
        {
            var result = new List<TaskNew>();
            foreach (var cardInfo in boardList.Cards)
            {
                result.AddRange(Convert(boardList.BoardId, cardInfo.Id, cardInfo.Name, cardInfo.Desc, cardInfo.Due));
            }
            return result;
        }
    }
}