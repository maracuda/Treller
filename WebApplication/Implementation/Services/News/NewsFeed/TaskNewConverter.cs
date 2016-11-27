namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.NewsFeed
{
    public class TaskNewConverter : ITaskNewConverter
    {
        public TaskNewModel Build(TaskNew taskNew)
        {
            return new TaskNewModel
            {
                DoNotDeliverUntil = taskNew.Content.DeadLine,
                TaskId = taskNew.TaskId,
                Content = taskNew.Content,
                Reports = taskNew.Reports
            };
        }
    }
}