namespace SKBKontur.Treller.WebApplication.Implementation.Services.News
{
    public class NewsEmail
    {
        public string TechnicalEmail { get; set; }
        public string ReleaseEmail { get; set; }

        public string GetEmail(bool technical)
        {
            return technical ? TechnicalEmail : ReleaseEmail;
        }
    }
}