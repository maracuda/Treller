using System.Net;

namespace ViskeyTube.Wiki
{
    public interface IWikiClient
    {
        string GetPage();
    }

    public class WikiClient : IWikiClient
    {
        public string GetPage()
        {
            using (var client = new WebClient())
            {
                var s = client.DownloadString("00");
                s = client.DownloadString("http://wiki.skbkontur.ru/rest/api/content?type=blogpost&spaceKey=TST&title=Команда");
                return s;
            }
        }
    }
}