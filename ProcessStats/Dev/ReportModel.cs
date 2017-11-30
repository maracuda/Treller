using System.Collections.Generic;

namespace ProcessStats.Dev
{
    public class ReportModel
    {
        public ReportModel(string name, IEnumerable<ReportRow> reportRows)
        {
            Name = name;
            Rows = reportRows;
        }

        public string Name { get; }

        public IEnumerable<ReportRow> Rows { get; }
    }
}