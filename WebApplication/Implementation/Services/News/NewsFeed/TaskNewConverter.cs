namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.NewsFeed
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
                State = TaskNewState.Reported,
                TaskId = taskNew.TaskId,
                Text = taskNew.Text,
                TimeStamp = taskNew.TimeStamp,
                Title = taskNew.Title
            };
        }
    }
}