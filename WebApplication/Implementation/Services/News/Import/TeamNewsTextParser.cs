using SKBKontur.Infrastructure.Sugar;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Import
{
    public class TeamNewsTextParser : ITextNewParser
    {
        private readonly SubstringParser motivationParser;
        private readonly SubstringParser analyticsParser;

        public TeamNewsTextParser()
        {
            motivationParser = new SubstringParser("**Мотивация**:", "**");
            analyticsParser = new SubstringParser("**Аналитика**:", "**");
        }
        
        public NewDeliveryChannelType DeliveryChannelType => NewDeliveryChannelType.Team;

        public Maybe<string> TryParse(string cardDescription)
        {
            var motivationResult = motivationParser.TryParse(cardDescription);
            var analyticsResult = analyticsParser.TryParse(cardDescription);
            var result = $"Всем доброго времени суток.\r\nКомадна Биллинга только что доставила огненный релиз на боевые.\r\n";
            if (motivationResult.HasValue)
                result += $"Немного о задаче: {motivationResult.Value}\r\n";
            if (analyticsResult.HasValue)
                result += $"Подробнее читайте здесь: {analyticsResult.Value}";
            return result;
        }
    }
}