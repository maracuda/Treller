using System;
using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Services.News
{
    public class NewsViewModel
    {
        public Dictionary<DateTime, NewsModel> TechnicalNews { get; set; }
        public Dictionary<DateTime, NewsModel> News { get; set; }
    }
}