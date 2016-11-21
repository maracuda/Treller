using System;
using SKBKontur.TaskManagerClient;
using SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Content.Parsing;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Content.Sources;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Content
{
    public class OnlineContentManager : IContentManager
    {
        private readonly IContentSourceRepository contentSourceRepository;
        private readonly ITaskManagerClient taskManagerClient;
        private readonly IContentParser contentParser;
        private readonly IContentRepository contentRepository;
        private readonly IErrorService errorService;

        public OnlineContentManager(
            IContentSourceRepository contentSourceRepository,
            ITaskManagerClient taskManagerClient,
            IContentParser contentParser,
            IContentRepository contentRepository,
            IErrorService errorService)
        {
            this.contentSourceRepository = contentSourceRepository;
            this.taskManagerClient = taskManagerClient;
            this.contentParser = contentParser;
            this.contentRepository = contentRepository;
            this.errorService = errorService;
        }

        public void RefreshContent()
        {
            foreach (var contentSource in contentSourceRepository.SelectActual())
            {
                try
                {
                    var cardInfo = taskManagerClient.GetCard(contentSource.ExternalId);
                    var content = contentParser.Parse(contentSource.Id, cardInfo.Name, cardInfo.Description, cardInfo.DueDate);
                    contentRepository.CreateOrUpdate(content);
                }
                catch (Exception e)
                {
                    errorService.SendError($"Fail to refresh content for card {contentSource.ExternalId}.", e);
                }
            }
        }
    }
}