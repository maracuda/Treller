namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Publisher
{
    public interface IPublisher
    {
        void Publish(string taskId, PublishStrategy deliveryChannel);
    }
}