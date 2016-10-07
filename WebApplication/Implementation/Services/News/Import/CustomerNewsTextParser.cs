using SKBKontur.Infrastructure.Sugar;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Import
{
    public class CustomerNewsTextParser : ITextNewParser
    {
        private readonly SubstringParser commonParser;
        private readonly SubstringParser customParser;

        public CustomerNewsTextParser()
        {
            commonParser = new SubstringParser("**Новости**:", "**");
            customParser = new SubstringParser("**Новости**:", "---");
        }
        
        public NewDeliveryChannelType DeliveryChannelType => NewDeliveryChannelType.Customer;
        
        public Maybe<string> TryParse(string cardDescription)
        {
            var commonResult = commonParser.TryParse(cardDescription);
            var customResult = customParser.TryParse(cardDescription);

            if (customResult.HasValue && customResult.HasValue && customResult.Value.Length < commonResult.Value.Length)
                return customResult;
            return commonResult;
        }
    }
}