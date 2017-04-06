using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Releases
{
    public interface IDemoPresentationsService
    {
        PresentationModel[] FetchPresentations(int count);
        Comment AppendComment(Guid presnetationId, string name, string text);
        PresentationContent DownloadPresentationContent(Guid presentationId);
        Comment[] FetchComments(Guid presentationId);
    }
}