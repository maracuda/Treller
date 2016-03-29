namespace SKBKontur.Treller.WebApplication.Services.News
{
    public class NewsModel
    {
        public string NewsText { get; set; }
        public string NewsHeader { get; set; }

        public CardNewsModel[] Cards { get; set; }
    }
}