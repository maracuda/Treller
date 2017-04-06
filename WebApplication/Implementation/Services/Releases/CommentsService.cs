using System;
using System.Linq;
using SKBKontur.Infrastructure.Common;
using SKBKontur.Treller.Storage;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Releases
{
    public class CommentsService : ICommentsService
    {
        private readonly ICollectionsStorageRepository collectionsStorageRepository;
        private readonly IDateTimeFactory dateTimeFactory;

        public CommentsService(
            ICollectionsStorageRepository collectionsStorageRepository,
            IDateTimeFactory dateTimeFactory)
        {
            this.collectionsStorageRepository = collectionsStorageRepository;
            this.dateTimeFactory = dateTimeFactory;
        }

        public Comment Append(Guid presentationId, string name, string text)
        {
            var comment = new Comment
            {
                PresentationId = presentationId,
                CommentId = Guid.NewGuid(),
                CreateDate = dateTimeFactory.Now,
                Name = name,
                Text = text
            };
            GetStorage(presentationId).Append(comment);
            return comment;
        }

        public Comment[] Fetch(Guid presentationId, int page = 1, int count = 100)
        {
            return GetStorage(presentationId).GetAll().Reverse().ToArray();
        }

        private ICollectionsStorage<Comment> GetStorage(Guid presentationId)
        {
            return collectionsStorageRepository.Get<Comment>($"Comments_{presentationId}");
        }
    }
}