using MessageBroker.Messages;

namespace MessageBroker.Bots
{
    public interface IEmailBot : IBot
    {
        void Publish(Email email);
    }
}