using NUnit.Framework;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Actualization;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.News
{
    public class NewsActualizatorTest : IntegrationTest
    {
        private INewsActualizator newsActualizator;

        public override void SetUp()
        {
            base.SetUp();

            newsActualizator = container.Get<INewsActualizator>();
        }

        [Test]
        public void TestActualizeAll()
        {
            newsActualizator.ActualizeAll(1000);
        }
    }
}