using System;
using System.Collections.Generic;
using System.Linq;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Reporters;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Migration
{
    public class TaskNewReportsMigrator
    {
        private readonly ICollectionsStorage collectionsStorage;
        private readonly IErrorService errorService;
        private readonly IReportFactory reportFactory;
        private readonly IDateTimeFactory dateTimeFactory;

        public TaskNewReportsMigrator(
            ICollectionsStorage collectionsStorage,
            IErrorService errorService,
            IReportFactory reportFactory,
            IDateTimeFactory dateTimeFactory)
        {
            this.collectionsStorage = collectionsStorage;
            this.errorService = errorService;
            this.reportFactory = reportFactory;
            this.dateTimeFactory = dateTimeFactory;
        }

        public void Run()
        {
            var existentTaskNews = collectionsStorage.GetAll<TaskNew>();
            var migratedTaskNews = new List<TaskNew>();
            var taskIds = existentTaskNews.Select(t => t.TaskId).Distinct().ToArray();

            var totalTaskIds = taskIds.Length;
            var migratedTaskIds = 0;

            foreach (var taskId in taskIds)
            {
                try
                {
                    var relatedTaskNews = existentTaskNews.Where(t => t.TaskId == taskId).ToArray();
                    var firstTaskNew = relatedTaskNews.First();
                    var reports = new List<Report>();
                    foreach (var relatedTaskNew in relatedTaskNews)
                    {
                        Report report = null;
                        switch (relatedTaskNew.DeliveryChannel)
                        {
                            case PublishStrategy.Customer:
                                report = reportFactory.CreateCutomerReport(relatedTaskNew.Content, taskId);
                                break;
                            case PublishStrategy.Support:
                                report = reportFactory.CreateSupportReport(relatedTaskNew.Content, taskId);
                                break;
                            case PublishStrategy.Team:
                                report = reportFactory.CreateTeamReport(relatedTaskNew.Content, taskId);
                                break;
                            default:
                                report = null;
                                break;
                        }

                        if (report != null)
                        {
                            if (relatedTaskNew.DeliverDateTime.HasValue)
                            {
                                report.PublishDate = relatedTaskNew.DeliverDateTime;
                            }
                            reports.Add(report);
                        }
                    }
                    var taskNew = new TaskNew
                    {
                        TaskId = taskId,
                        Content = firstTaskNew.Content,
                        Reports = reports.ToArray(),
                        DeliveryChannel = firstTaskNew.DeliveryChannel,
                        TimeStamp = dateTimeFactory.UtcTicks
                    };
                    migratedTaskNews.Add(taskNew);
                    migratedTaskIds++;
                }
                catch (Exception)
                {
                    
                }
            }

            collectionsStorage.Delete<TaskNew>();
            collectionsStorage.Put(migratedTaskNews.ToArray());

            errorService.SendError($"TaskNewReportsMigrator finished migration. Total tasks ids to migrate {totalTaskIds}. Migrated tasks {migratedTaskIds}.");
        }
    }
}