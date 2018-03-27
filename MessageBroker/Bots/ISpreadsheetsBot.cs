using MessageBroker.Messages;

namespace MessageBroker.Bots
{
    public interface ISpreadsheetsBot
    {
        void Publish(Report report);
    }
}