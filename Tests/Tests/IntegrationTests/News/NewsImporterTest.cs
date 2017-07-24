using WebApplication.Implementation.Services.News.NewsFeed;
using WebApplication.Implementation.Services.News.Reporters;
using Xunit;

namespace Tests.Tests.IntegrationTests.News
{
    public class NewsImporterTest : IntegrationTest
    {
        private TaskManagerReporter taskManagerReporter;

        public NewsImporterTest() : base()
        {
            taskManagerReporter = container.Get<TaskManagerReporter>();
        }

        [Fact]
        public void TestImportCard()
        {
            var zzz = taskManagerReporter.TryToMakeReport("XiPNHjuM");
            container.Get<INewsFeed>().AddNews(zzz.Value);

            var x = 1;
        }
    }
}