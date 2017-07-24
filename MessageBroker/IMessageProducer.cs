namespace MessageBroker
{
    public interface IMessageProducer
    {
        void Publish(Message message);
    }
}