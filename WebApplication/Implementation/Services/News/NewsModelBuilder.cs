using SKBKontur.Treller.WebApplication.Implementation.Services.News.Search;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public class NewsModelBuilder : INewsModelBuilder
    {
        private readonly ITaskNewIndex taskNewIndex;

        public NewsModelBuilder(ITaskNewIndex taskNewIndex)
        {
            this.taskNewIndex = taskNewIndex;
        }

        public NewsViewModel BuildViewModel()
        {
            return new NewsViewModel
            {
                TaskNews = taskNewIndex.SelectCurrentNews()
            };
        }
    }
}