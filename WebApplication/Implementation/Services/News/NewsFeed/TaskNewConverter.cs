namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.NewsFeed
{
    public class TaskNewConverter : ITaskNewConverter
    {
        public TaskNewModel Build(TaskNew taskNew)
        {
            var text = $"{taskNew.Content.Motivation}\r\n{taskNew.Content.Analytics}\r\n{taskNew.Content.Branch}\r\n{taskNew.Content.PubicInfo}\r\n{taskNew.Content.TechInfo}";

            return new TaskNewModel
            {
                DoNotDeliverUntil = taskNew.Content.DeadLine,
                State = TaskNewState.Reported,
                TaskId = taskNew.TaskId,
                Text = text,
                TimeStamp = taskNew.TimeStamp,
                Title = taskNew.Content.Title
            };
        }
    }
}