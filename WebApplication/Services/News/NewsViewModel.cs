namespace SKBKontur.Treller.WebApplication.Services.News
{
    public class NewsViewModel
    {
        public string ReleaseEmail { get; set; }
        public string TechnicalEmail { get; set; }

        public NewsModel TechnicalNewsToPublish { get; set; }
        public NewsModel NewsToPublish { get; set; }
        public NewCardNewsModel[] NotActualCards { get; set; }
        public NewCardNewsModel[] CardsWihoutNews { get; set; }
        public NewCardNewsModel[] ActualCards { get; set; }
    }
}