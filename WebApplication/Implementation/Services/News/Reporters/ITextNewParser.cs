using SKBKontur.Infrastructure.Sugar;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Reporters
{
    public interface ITextNewParser
    {
        NewDeliveryChannelType DeliveryChannelType { get; }
        Maybe<string> TryParse(string cardDescription);
    }
}