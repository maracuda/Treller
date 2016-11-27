using System.Linq;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.NewsFeed
{
    public class TaskNewConverter : ITaskNewConverter
    {
        public TaskNewModel Build(TaskNew taskNew)
        {
            return new TaskNewModel
            {
                TaskId = taskNew.TaskId,
                Content = Build(taskNew.Content),
                Reports = taskNew.Reports.Select(Build).ToArray()
            };
        }

        private static ReportModel Build(Report report)
        {
            return new ReportModel
            {
                Title = report.Title,
                Message = report.Message,
                PublishStrategy = report.PublishStrategy,
                DoNotDeliverUntil = report.DoNotDeliverUntil,
                PublishDate = report.PublishDate
            };
        }

        private static ContentModel Build(Content.Content content)
        {
            return new ContentModel
            {
                Title = content.Title,
                Analytics = content.Analytics,
                Branch = content.Branch,
                DeadLine = content.DeadLine,
                Motivation = content.Motivation,
                PubicInfo = content.PubicInfo,
                TechInfo = content.TechInfo
            };
        }
    }
}