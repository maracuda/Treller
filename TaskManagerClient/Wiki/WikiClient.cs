namespace SKBKontur.TaskManagerClient.Wiki
{
    public class WikiClient : IWikiClient
    {
        public string GetBaseUrl()
        {
            return "https://wiki.skbkontur.ru";
        }
    }
}