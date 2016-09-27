using NUnit.Framework;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Import;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.News
{
    public class NewsImporterTest : IntegrationTest
    {
        private NewsImporter newsImporter;

        public override void SetUp()
        {
            base.SetUp();

            newsImporter = container.Get<NewsImporter>();
        }

        [Test]
        public void TestImportCard()
        {
            newsImporter.Import("wtq5pHOg");
        }
    }
}