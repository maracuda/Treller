namespace MessageBroker
{
    public interface IEmailMessageProducer
    {
        void Publish(EmailMessage message);
    }
}