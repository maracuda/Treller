using System.Collections.Generic;

namespace ProcessStats.Dev
{
    public interface IStatsReportBuilder
    {
        ReportModel[] BuildForBillingDelivery();
        IEnumerable<ReportModel> BuildForDirections();
    }
}