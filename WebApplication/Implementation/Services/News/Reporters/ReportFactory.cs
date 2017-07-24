using System;

namespace WebApplication.Implementation.Services.News.Reporters
{
    public class ReportFactory : IReportFactory
    {
        public Report CreateCutomerReport(Content.Content content, string taskId)
        {
            return Create(taskId, PublishStrategy.Customer, content.Title, content.DeadLine, content.PubicInfo);
        }

        public Report CreateSupportReport(Content.Content content, string taskId)
        {
            return Create(taskId, PublishStrategy.Support, content.Title, content.DeadLine, content.TechInfo);
        }

        public Report CreateTeamReport(Content.Content content, string taskId)
        {
            var message = $"Комадна Биллинга только что доставила огненный релиз на боевые.\r\n";
            if (!string.IsNullOrEmpty(content.Motivation))
                message += $"Немного о задаче: {content.Motivation}\r\n";
            if (!string.IsNullOrEmpty(content.Analytics))
                message += $"Подробнее читайте здесь: {content.Analytics}";
            return Create(taskId, PublishStrategy.Team, content.Title, content.DeadLine, message);
        }

        private static Report Create(string taskId, PublishStrategy publishStrategy, string title, DateTime? doNotDeliverUntil, string message)
        {
            return new Report
            {
                TaskId = taskId,
                PublishStrategy = publishStrategy,
                Title = title,
                DoNotDeliverUntil = doNotDeliverUntil,
                Message = message
            };
        }
    }
}