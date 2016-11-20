namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.NewsFeed
{
    public class TaskNewConverter : ITaskNewConverter
    {
        public TaskNewModel Build(TaskNew taskNew)
        {
            return new TaskNewModel
            {
                DoNotDeliverUntil = taskNew.Content.DeadLine,
                State = TaskNewState.Reported,
                TaskId = taskNew.TaskId,
                Text = taskNew.GetContentText(),
                TimeStamp = taskNew.TimeStamp,
                Title = taskNew.Content.Title
            };
        }
    }
}