using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Releases
{
    public class ReleasesPageViewModel
    {
        public Release[] Releases { get; set; }
        public Dictionary<string, string> Urls { get; set; }
    }
}