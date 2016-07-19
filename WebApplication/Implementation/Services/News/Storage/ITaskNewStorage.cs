using SKBKontur.Infrastructure.Sugar;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Storage
{
    public interface ITaskNewStorage
    {
        Maybe<TaskNew[]> Find(string taskId);
        TaskNew[] ReadAll();
        void Create(TaskNew taskNew);
        void Update(TaskNew changedTaskNew, string diffInfo);
        void Delete(params TaskNew[] uselessTaskNews);
    }
}