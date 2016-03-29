namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public class NewsEmail
    {
        public string TechnicalEmail { get; set; }
        public string ReleaseEmail { get; set; }

        public static NewsEmail Default()
        {
            return new NewsEmail { ReleaseEmail = "maylo@skbkontur.ru", TechnicalEmail = "maylo@skbkontur.ru" };
        }

        public string GetEmail(bool technical)
        {
            return technical ? TechnicalEmail : ReleaseEmail;
        }
    }
}