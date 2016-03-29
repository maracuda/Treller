namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public interface INewsService
    {
        void Refresh();
        NewsViewModel GetNews();
        void DeleteCard(string cardId);
        void RestoreCard(string cardId);
        void SendTechnicalNews();
        void SendNews();
        void UpdateEmail(string technicalEmail, string releaseEmail);
        void UpdateEmailToBattleValues();
        bool IsAnyNewsExists();
    }
}