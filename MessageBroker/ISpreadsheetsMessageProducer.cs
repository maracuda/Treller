namespace MessageBroker
{
    public interface ISpreadsheetsMessageProducer
    {
        void Append(string spreadsheetId, string sheetName, DataRow dataRow);
        void Rewrite(string spreadsheetId, string sheetName, DataRow[] dataRows);
    }
}