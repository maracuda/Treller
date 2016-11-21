using SKBKontur.Infrastructure.Sugar;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Storage
{
    public interface ITaskNewStorage
    {
        Maybe<TaskNew> Find(string taskId);
        TaskNew[] ReadAll();
        TaskNew[] Enumerate(long fromTimestampExclusive, int batchSize);
        void Create(TaskNew taskNew);
        void Update(TaskNew changedTaskNew);
        void Delete(params TaskNew[] uselessTaskNews);
    }
}