using NUnit.Framework;
using SKBKontur.Treller.WebApplication.Implementation.Services.News;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.News
{
    public class NewsServiceTest : IntegrationTest
    {
        private INewsService newsService;

        public override void SetUp()
        {
            base.SetUp();

            newsService = container.Get<INewsService>();
        }

        [Test]
        public void TestRefresh()
        {
            newsService.Refresh();
        }
    }
}