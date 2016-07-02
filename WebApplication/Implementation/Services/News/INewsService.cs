namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public interface INewsService
    {
        //TODO list:
        //Integrate NewsType: tech or public notification
        //Introduce proper domain model without information about any cards etc
        //Decompose service layer from ui interpretation
        void Refresh();
        CardNewsModel[] GetAllNews();
        void DeleteCard(string cardId);
        void RestoreCard(string cardId);
        void SendNews();
    }
}