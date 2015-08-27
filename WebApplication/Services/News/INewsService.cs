using System;

namespace SKBKontur.Treller.WebApplication.Services.News
{
    public interface INewsService
    {
        NewsViewModel GetAllNews();
        bool TryRefresh(DateTime timestamp);
    }
}