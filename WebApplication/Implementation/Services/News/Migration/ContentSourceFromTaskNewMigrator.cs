using SKBKontur.Treller.WebApplication.Implementation.Services.News.Content.Sources;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.NewsFeed;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Migration
{
    public class ContentSourceFromTaskNewMigrator
    {
        private readonly INewsFeed newsFeed;
        private readonly IContentSourceRepository contentSourceRepository;

        public ContentSourceFromTaskNewMigrator(
            INewsFeed newsFeed,
            IContentSourceRepository contentSourceRepository)
        {
            this.newsFeed = newsFeed;
            this.contentSourceRepository = contentSourceRepository;
        }

        public void Migrate()
        {
            var models = newsFeed.SelectAll();
            foreach (var taskNewModel in models)
            {
                contentSourceRepository.FindOrRegister(taskNewModel.TaskId);
            }
        }
    }
}