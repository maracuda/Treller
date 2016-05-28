namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public class NewsModelBuilder : INewsModelBuilder
    {
        private readonly INewsService newsSerivce;

        public NewsModelBuilder(INewsService newsSerivce)
        {
            this.newsSerivce = newsSerivce;
        }

        public NewsViewModel Build()
        {
            var result = newsSerivce.GetNews();
            result.Settings = newsSerivce.NewsSettings;
            return result;
        }
    }
}