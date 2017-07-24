using Infrastructure.Sugar;

namespace WebApplication.Implementation.Services.News.Storage
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