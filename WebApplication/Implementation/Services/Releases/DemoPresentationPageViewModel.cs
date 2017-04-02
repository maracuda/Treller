using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Releases
{
    public class DemoPresentationPageViewModel
    {
        public PresentationModel[] Releases { get; set; }
        public Dictionary<string, string> Urls { get; set; }
    }
}