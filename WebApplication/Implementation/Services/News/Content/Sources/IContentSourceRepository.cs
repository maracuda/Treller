using System;

namespace WebApplication.Implementation.Services.News.Content.Sources
{
    public interface IContentSourceRepository
    {
        bool Contains(string externalId);
        ContentSource FindOrRegister(string externalId);
        void Block(Guid sourceId);
        ContentSource[] SelectActual();
    }
}