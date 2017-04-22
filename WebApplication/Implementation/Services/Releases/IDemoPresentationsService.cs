using System;
using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Releases
{
    public interface IDemoPresentationsService
    {
        IEnumerable<PresentationModel> FetchPresentations(int count);
        Comment AppendComment(Guid presnetationId, string name, string text);
        PresentationContent DownloadPresentationContent(Guid presentationId);
    }
}