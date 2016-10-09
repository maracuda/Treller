using SKBKontur.Infrastructure.Sugar;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Reporters
{
    public class SupportNewsTextParser : ITextNewParser
    {
        private readonly SubstringParser substringParser;

        public SupportNewsTextParser()
        {
            substringParser = new SubstringParser("**Технические новости**:", "**");
        }

        public NewDeliveryChannelType DeliveryChannelType => NewDeliveryChannelType.Support;

        public Maybe<string> TryParse(string cardDescription)
        {
            return substringParser.TryParse(cardDescription);
        }
    }
}