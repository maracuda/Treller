using MessageBroker.Messages;

namespace MessageBroker.Bots
{
    public interface ISpreadsheetsBot : IBot
    {
        void Publish(Report report);
    }
}