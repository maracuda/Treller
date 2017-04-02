using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Releases
{
    public interface IPresentationContentService
    {
        PresentationContent Fetch(Guid presentationId);
        void Create(Guid presentationId, string type, byte[] bytes);
    }
}