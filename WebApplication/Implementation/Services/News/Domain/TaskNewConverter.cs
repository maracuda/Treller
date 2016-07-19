using SKBKontur.Treller.WebApplication.Implementation.Services.News.Search;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Domain
{
    public class TaskNewConverter : ITaskNewConverter
    {
        public TaskNewModel Build(TaskNew taskNew)
        {
            return new TaskNewModel
            {
                BoardId = taskNew.BoardId,
                DeliveryChannel = taskNew.DeliveryChannel,
                DoNotDeliverUntil = taskNew.DoNotDeliverUntil,
                State = TaskNewState.Imported,
                TaskId = taskNew.TaskId,
                Text = taskNew.Text,
                TimeStamp = taskNew.TimeStamp,
                Title = taskNew.Title
            };
        }

        public TaskNew Project(TaskNewModel taskNewModel)
        {
            return new TaskNew
            {
                BoardId = taskNewModel.BoardId,
                TaskId = taskNewModel.BoardId,
                DeliveryChannel = taskNewModel.DeliveryChannel,
                DoNotDeliverUntil = taskNewModel.DoNotDeliverUntil,
                Text = taskNewModel.Text,
                TimeStamp = taskNewModel.TimeStamp,
                Title = taskNewModel.Title
            };
        }
    }
}