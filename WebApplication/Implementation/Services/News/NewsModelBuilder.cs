namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public class NewsModelBuilder : INewsModelBuilder
    {
        private readonly INewsService newsSerivce;
        private readonly INewsSettingsService newsSettingsService;

        public NewsModelBuilder(
            INewsService newsSerivce,
            INewsSettingsService newsSettingsService)
        {
            this.newsSerivce = newsSerivce;
            this.newsSettingsService = newsSettingsService;
        }

        public NewsViewModel Build()
        {
            var result = newsSerivce.GetNews();
            result.Settings = newsSettingsService.GetOrRead();
            return result;
        }
    }
}