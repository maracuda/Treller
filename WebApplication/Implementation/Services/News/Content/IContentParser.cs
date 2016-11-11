using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Content
{
    public interface IContentParser
    {
        Content Parse(Guid sourceId, string title, string desc, DateTime? deadLine);
    }
}