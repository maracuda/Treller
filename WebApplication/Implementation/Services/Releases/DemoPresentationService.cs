using System;
using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Releases
{
    public class DemoPresentationService : IDemoPresentationsService
    {
        private readonly IPresentationStorage presentationStorage;
        private readonly ICommentsStorage commentsStorage;
        private readonly IPresentationContentStorage presentationContentStorage;

        public DemoPresentationService(
            IPresentationStorage presentationStorage,
            ICommentsStorage commentsStorage,
            IPresentationContentStorage presentationContentStorage)
        {
            this.presentationStorage = presentationStorage;
            this.commentsStorage = commentsStorage;
            this.presentationContentStorage = presentationContentStorage;
        }

        public IEnumerable<PresentationModel> FetchPresentations(int count)
        {
            var presentations = presentationStorage.FetchAll();
            foreach (var presentation in presentations)
            {
                yield return new PresentationModel
                {
                    PresentationId = presentation.Id,
                    CreateDate = presentation.CreateDate,
                    Title = presentation.Title,
                    Content = presentation.Description,
                    Comments = FetchComments(presentation.Id)
                };
            }
        }

        public Comment AppendComment(Guid presnetationId, string name, string text)
        {
            return commentsStorage.Append(presnetationId, name, text);
        }

        public PresentationContent DownloadPresentationContent(Guid presentationId)
        {
            return presentationContentStorage.Fetch(presentationId);
        }

        private Comment[] FetchComments(Guid presentationId)
        {
            return commentsStorage.Fetch(presentationId);
        }
    }
}