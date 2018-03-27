using MessageBroker.Messages;

namespace MessageBroker.Bots
{
    public interface IEmailBot
    {
        void Publish(Email email);
    }
}