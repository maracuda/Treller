using System.Linq;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Storage;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Search
{
    public class TaskNewIndex : ITaskNewIndex
    {
        private readonly ITaskNewStorage taskNewStorage;

        public TaskNewIndex(ITaskNewStorage taskNewStorage)
        {
            this.taskNewStorage = taskNewStorage;
        }

        public TaskNewModel[] SelectCurrentNews()
        {
            return taskNewStorage.ReadAll()
                .Select(x => new TaskNewModel
                {
                    BoardId = x.BoardId,
                    DeliveryChannel = x.DeliveryChannel,
                    DoNotDeliverUntil = x.DoNotDeliverUntil,
                    State = TaskNewState.Imported,
                    TaskId = x.TaskId,
                    Text = x.Text,
                    TimeStamp = x.TimeStamp,
                    Title = x.Title
                })
                .ToArray();
        }
    }
}