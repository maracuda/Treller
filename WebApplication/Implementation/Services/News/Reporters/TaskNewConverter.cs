using System;
using System.Collections.Generic;
using SKBKontur.Infrastructure.Common;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Content;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Content.Sources;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Reporters
{
    public class TaskNewConverter : ITaskNewConverter
    {
        private readonly IDateTimeFactory dateTimeFactory;
        private readonly ITextNewParser[] textNewParsers;
        private readonly IContentParser contentParser;
        private readonly IContentSourceRepository contentSourceRepository;

        public TaskNewConverter(
            IDateTimeFactory dateTimeFactory,
            ITextNewParser[] textNewParsers,
            IContentParser contentParser,
            IContentSourceRepository contentSourceRepository)
        {
            this.dateTimeFactory = dateTimeFactory;
            this.textNewParsers = textNewParsers;
            this.contentParser = contentParser;
            this.contentSourceRepository = contentSourceRepository;
        }

        public List<TaskNew> Convert(string cardId, string cardName, string cardDesc, DateTime? cardDueDate)
        {
            var result = new List<TaskNew>();
            var contentSource = contentSourceRepository.FindOrRegister(cardId);
            var content = contentParser.Parse(contentSource.Id, cardName, cardDesc, cardDueDate);
            foreach (var textNewParser in textNewParsers)
            {
                var parseResult = textNewParser.TryParse(cardDesc);
                if (parseResult.HasValue && !string.IsNullOrWhiteSpace(parseResult.Value))
                {
                    result.Add(new TaskNew
                    {
                        TaskId = cardId,
                        Content = content,
                        Text = parseResult.Value,
                        DeliveryChannel = textNewParser.PublishStrategy,
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
                result.AddRange(Convert(cardInfo.Id, cardInfo.Name, cardInfo.Desc, cardInfo.Due));
            }
            return result;
        }
    }
}