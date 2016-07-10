using SKBKontur.Infrastructure.Sugar;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Import
{
    public class TeamNewsTextParser : ITextNewParser
    {
        private readonly SubstringParser substringParser;

        public TeamNewsTextParser()
        {
            substringParser = new SubstringParser("**Мотивация**:", "\n\n");
        }
        
        public NewDeliveryChannelType DeliveryChannelType => NewDeliveryChannelType.Team;

        public Maybe<string> TryParse(string cardDescription)
        {
            return substringParser.TryParse(cardDescription);
        }
    }
}