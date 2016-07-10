using SKBKontur.Infrastructure.Sugar;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Import
{
    public class CustomerNewsTextParser : ITextNewParser
    {
        private readonly SubstringParser substringParser;

        public CustomerNewsTextParser()
        {
            substringParser = new SubstringParser("**Новости**:", "\n\n");
        }
        
        public NewDeliveryChannelType DeliveryChannelType => NewDeliveryChannelType.Customer;
        
        public Maybe<string> TryParse(string cardDescription)
        {
            return substringParser.TryParse(cardDescription);
        }
    }
}