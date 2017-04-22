using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Releases
{
    public interface IPresentationStorage
    {
        void Append(Guid id, DateTime createDate, string title, string description);
        Presentation[] FetchAll();
    }
}