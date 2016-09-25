namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Sender
{
    public interface IMagazine
    {
        void Publish(TaskNew taskNew);
        void Publish(string taskId);
    }
}