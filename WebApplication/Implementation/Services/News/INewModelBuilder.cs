namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public interface INewsModelBuilder
    {
        NewsViewModel BuildViewModel();
        CardNewsModel[] BuildNewsModel();
    }
}