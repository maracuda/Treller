using System;
using System.Linq;
using SKBKontur.Treller.Storage;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Releases
{
    public class PresentationStorage : IPresentationStorage
    {
        private readonly ICollectionsStorage<Presentation> collectionsStorage;

        public PresentationStorage(
            ICollectionsStorageRepository collectionsStorageRepository)
        {
            collectionsStorage = collectionsStorageRepository.Get<Presentation>();
        }

        public Presentation[] FetchAll()
        {
            return collectionsStorage.GetAll().OrderBy(p => p.CreateDate).ToArray();
        }

        public void Append(Guid id, DateTime createDate, string title, string description)
        {
            var presentation = new Presentation
            {
                Id = id,
                CreateDate = createDate,
                Title = title,
                Description = description
            };
            collectionsStorage.Append(presentation);
        }
    }
}