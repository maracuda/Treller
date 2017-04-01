using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Releases
{
    public class ReleasesPageViewModel
    {
        public Release[] Releases { get; set; }
        public Dictionary<string, string> Urls { get; set; }
    }
}