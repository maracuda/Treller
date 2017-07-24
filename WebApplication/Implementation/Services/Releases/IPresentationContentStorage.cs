using System;

namespace WebApplication.Implementation.Services.Releases
{
    public interface IPresentationContentStorage
    {
        PresentationContent Fetch(Guid presentationId);
        void Create(Guid presentationId, string mimeType, byte[] bytes);
    }
}