namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public interface INewsSettingsService
    {
        NewsSettings GetOrRead();
        void Reset();
        void Update(string techMailingList, string publicMailingList);
    }
}