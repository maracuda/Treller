using System.Collections.Generic;

namespace WebApplication.Implementation.Services.Releases
{
    public class DemoPresentationPageViewModel
    {
        public PresentationModel[] Releases { get; set; }
        public Dictionary<string, string> Urls { get; set; }
    }
}