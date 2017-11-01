using System.Collections.Generic;

namespace MessageBroker
{
    //TODO: move to messagebroker (extend message broker to process different types of messages)
    public interface ISpreadsheetsMessageProducer
    {
        void Publish(string spreadsheetId, int sheetId, IEnumerable<object> rowData);
    }
}