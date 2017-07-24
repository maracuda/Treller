using System;
using System.Linq;
using Infrastructure.Common;
using Storage;

namespace WebApplication.Implementation.Services.Releases
{
    public class CommentsStorage : ICommentsStorage
    {
        private readonly ICollectionsStorageRepository collectionsStorageRepository;
        private readonly IDateTimeFactory dateTimeFactory;

        public CommentsStorage(
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