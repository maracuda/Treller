namespace MessageBroker.Messages
{
    public class Report : Message
    {
        public Report()
        {
            DataRows = new DataRow[0];
        }

        public string SpreadsheetId { get; set; }
        public string SheetName { get; set; }
        public ReportType Type { get; set; }
        public DataRow[] DataRows { get; set; }

        public override string ToString()
        {
            return $"{SpreadsheetId}, {SheetName}, {Type}, {DataRows.Length} rows.";
        }
    }
}