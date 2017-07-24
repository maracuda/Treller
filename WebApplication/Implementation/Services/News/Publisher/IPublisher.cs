namespace WebApplication.Implementation.Services.News.Publisher
{
    public interface IPublisher
    {
        void Publish(string taskId, PublishStrategy publishStrategy);
    }
}