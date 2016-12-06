using System;
using System.Collections.Generic;
using SKBKontur.Infrastructure.Common;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Content.Parsing;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Content.Sources;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Reporters
{
    public class TaskNewConverter : ITaskNewConverter
    {
        private readonly IDateTimeFactory dateTimeFactory;
        private readonly IContentParser contentParser;
        private readonly IContentSourceRepository contentSourceRepository;
        private readonly IReportFactory reportFactory;

        public TaskNewConverter(
            IDateTimeFactory dateTimeFactory,
            IContentParser contentParser,
            IContentSourceRepository contentSourceRepository,
            IReportFactory reportFactory)
        {
            this.dateTimeFactory = dateTimeFactory;
            this.contentParser = contentParser;
            this.contentSourceRepository = contentSourceRepository;
            this.reportFactory = reportFactory;
        }

        public List<TaskNew> Convert(string cardId, string cardName, string cardDesc, DateTime? cardDueDate)
        {
            var contentSource = contentSourceRepository.FindOrRegister(cardId);
            var content = contentParser.Parse(contentSource.Id, cardName, cardDesc, cardDueDate);
            var reports = BuildReports(content, cardId);

            var taskNew = new TaskNew
            {
                TaskId = cardId,
                Content = content,
                Reports = reports,
                TimeStamp = dateTimeFactory.UtcTicks
            };
            return new List<TaskNew> {taskNew};
        }

        public List<TaskNew> Convert(BoardList boardList)
        {
            var result = new List<TaskNew>();
            foreach (var cardInfo in boardList.Cards)
            {
                result.AddRange(Convert(cardInfo.Id, cardInfo.Name, cardInfo.Desc, cardInfo.Due));
            }
            return result;
        }

        private Report[] BuildReports(Content.Content content, string taskId)
        {
            var reports = new List<Report>();
            if (!string.IsNullOrEmpty(content.PubicInfo))
            {
                reports.Add(reportFactory.CreateCutomerReport(content, taskId));
            }
            if (!string.IsNullOrEmpty(content.TechInfo))
            {
                reports.Add(reportFactory.CreateSupportReport(content, taskId));
            }
            reports.Add(reportFactory.CreateTeamReport(content, taskId));
            return reports.ToArray();
        }
    }
}