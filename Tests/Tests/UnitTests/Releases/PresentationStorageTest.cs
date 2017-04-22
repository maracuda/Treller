using System;
using Rhino.Mocks;
using SKBKontur.Treller.Storage;
using SKBKontur.Treller.WebApplication.Implementation.Services.Releases;
using Xunit;

namespace SKBKontur.Treller.Tests.Tests.UnitTests.Releases
{
    public class PresentationStorageTest : UnitTest
    {
        private readonly ICollectionsStorageRepository collectionsStorageRepository;
        private PresentationStorage presentationStorage;
        private readonly ICollectionsStorage<Presentation> collectionsStorage;

        public PresentationStorageTest()
        {
            collectionsStorageRepository = mockRepository.Create<ICollectionsStorageRepository>();
            collectionsStorage = mockRepository.Create<ICollectionsStorage<Presentation>>();
        }

        [Fact]
        public void PresentationStorageReturnsObjectsInDescendingOrder()
        {
            var oldPresentation = new Presentation
            {
                CreateDate = new DateTime(2010, 1, 1)
            };
            var youngPresentation = new Presentation
            {
                CreateDate = new DateTime(2016, 1, 1)
            };

            using (mockRepository.Record())
            {
                collectionsStorageRepository.Stub(x => x.Get<Presentation>()).Return(collectionsStorage);
                collectionsStorage.Expect(x => x.GetAll()).Return(new[] {youngPresentation, oldPresentation});
            }

            presentationStorage = new PresentationStorage(collectionsStorageRepository);
            var actuals = presentationStorage.FetchAll();
            Assert.Equal(2, actuals.Length);
            Assert.Equal(youngPresentation, actuals[0]);
            Assert.Equal(oldPresentation, actuals[1]);
        }
    }
}