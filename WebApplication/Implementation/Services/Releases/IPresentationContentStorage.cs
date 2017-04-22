using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Releases
{
    public interface IPresentationContentStorage
    {
        PresentationContent Fetch(Guid presentationId);
        void Create(Guid presentationId, string type, byte[] bytes);
    }
}