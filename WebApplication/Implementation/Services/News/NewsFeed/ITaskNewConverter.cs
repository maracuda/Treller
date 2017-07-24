namespace WebApplication.Implementation.Services.News.NewsFeed
{
    public interface ITaskNewConverter
    {
        TaskNewModel Build(TaskNew taskNew);
    }
}