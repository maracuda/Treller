namespace ProcessStats.SpreadsheetProducer
{
    //TODO: move to messagebroker (extend message broker to process different types of messages)
    public interface ISpreadsheetProducer
    {
        void Publish(string spreadsheetId, int sheetId, string sheetName, string[] rowData);
    }
}