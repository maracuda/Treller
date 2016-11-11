using System;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Reporters;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Content
{
    public class ContentParser : IContentParser
    {
        private readonly ITokenParserFactory tokenParserFactory;
        private readonly IDateTimeFactory dateTimeFactory;

        public ContentParser(
            ITokenParserFactory tokenParserFactory,
            IDateTimeFactory dateTimeFactory)
        {
            this.tokenParserFactory = tokenParserFactory;
            this.dateTimeFactory = dateTimeFactory;
        }

        public Content Parse(Guid sourceId, string title, string desc, DateTime? deadLine)
        {
            return new Content
            {
                SourceId = sourceId,
                Title = title,
                Motivation = tokenParserFactory.GetMotivationParser().TryParse(desc, string.Empty),
                Analytics = tokenParserFactory.GetAnalyticsParser().TryParse(desc, string.Empty),
                Branch = tokenParserFactory.GetBranchParser().TryParse(desc, string.Empty),
                PubicInfo = tokenParserFactory.GetPublicInfoParser().TryParse(desc, string.Empty),
                TechInfo = tokenParserFactory.GetPublicInfoParser().TryParse(desc, string.Empty),
                DeadLine = deadLine,
                Timestamp = dateTimeFactory.Ticks
            };
        }
    }
}