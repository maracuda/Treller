using System;
using System.Linq;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Serialization;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.NewsFeed
{
    public class TaskNewConverter : ITaskNewConverter
    {
        private readonly IJsonSerializer jsonSerializer;

        public TaskNewConverter(IJsonSerializer jsonSerializer)
        {
            this.jsonSerializer = jsonSerializer;
        }

        public TaskNewModel Build(TaskNew taskNew)
        {
            try
            {
                return new TaskNewModel
                {
                    TaskId = taskNew.TaskId,
                    Content = Build(taskNew.Content),
                    //TODO: remove null check after we can gurantee that storage has no null reports.
                    Reports = taskNew.Reports.Where(x => x != null).Select(Build).ToArray()
                };
            }
            catch (Exception e)
            {
                throw new Exception($"Fail to convert model for object {jsonSerializer.Serialize(taskNew)}.", e);
            }
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