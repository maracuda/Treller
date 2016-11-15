namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.NewsFeed
{
    public class TaskNewConverter : ITaskNewConverter
    {
        public TaskNewModel Build(TaskNew taskNew)
        {
            return new TaskNewModel
            {
                DeliveryChannel = taskNew.DeliveryChannel,
                DoNotDeliverUntil = taskNew.DoNotDeliverUntil,
                State = TaskNewState.Reported,
                TaskId = taskNew.TaskId,
                Text = taskNew.GetContentText(),
                TimeStamp = taskNew.TimeStamp,
                Title = taskNew.GetContentTitle()
            };
        }
    }
}