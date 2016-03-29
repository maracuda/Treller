namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public class NewsViewModel
    {
        public string ReleaseEmail { get; set; }
        public string TechnicalEmail { get; set; }

        public NewsModel TechnicalNewsToPublish { get; set; }
        public NewsModel NewsToPublish { get; set; }
        public CardNewsModel[] NotActualCards { get; set; }
        public CardNewsModel[] CardsWihoutNews { get; set; }
        public CardNewsModel[] ActualCards { get; set; }
    }
}