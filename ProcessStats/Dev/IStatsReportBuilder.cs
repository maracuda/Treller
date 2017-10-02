using TaskManagerClient.BusinessObjects.TaskManager;

namespace ProcessStats.Dev
{
    public interface IStatsReportBuilder
    {
        ReportModel[] BuildForBillingDelivery();
        ReportModel BuildForDirection(BoardList doneList);
    }
}