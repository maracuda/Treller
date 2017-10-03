using System.IO;
using ProcessStats.Dev;
using Xunit;

namespace Tests.Tests.IntegrationTests.ProcessStats
{
    public class ReportBuilderTest : IntegrationTest
    {
        private readonly IStatsReportBuilder statsReportBuilder;

        public ReportBuilderTest()
        {
            statsReportBuilder = container.Get<IStatsReportBuilder>();
        }

        [Fact]
        public void BuildReportForBillingDeliveryBoard()
        {
            var reportsModels = statsReportBuilder.BuildForBillingDelivery();
            foreach (var reportModel in reportsModels)
            {
                var reportPath = $"{reportModel.Name}.csv";
                if (File.Exists(reportPath))
                {
                    File.Delete(reportPath);
                }
                File.WriteAllBytes(reportPath, reportModel.Content);
            }
        }

        [Fact]
        public void BuildReportForDirectionBoards()
        {
            var doneLists = new [] {KnownLists.DiscountsDone, KnownLists.MarketDone, KnownLists.MotocycleDone, KnownLists.PortalAuthDone};
            foreach (var doneList in doneLists)
            {
                var reportModel = statsReportBuilder.BuildForDirection(doneList);
                var reportPath = $"{reportModel.Name}.csv";
                if (File.Exists(reportPath))
                {
                    File.Delete(reportPath);
                }
                File.WriteAllBytes(reportPath, reportModel.Content);
            }
        }
    }
}