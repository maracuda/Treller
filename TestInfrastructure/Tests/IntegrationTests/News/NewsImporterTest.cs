using NUnit.Framework;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Reporters;

namespace SKBKontur.Treller.Tests.Tests.IntegrationTests.News
{
    public class NewsImporterTest : IntegrationTest
    {
        private TaskManagerReporter taskManagerReporter;

        public override void SetUp()
        {
            base.SetUp();

            taskManagerReporter = container.Get<TaskManagerReporter>();
        }

        [Test]
        public void TestImportCard()
        {
            taskManagerReporter.TryToMakeReport("XiPNHjuM");
        }
    }
}