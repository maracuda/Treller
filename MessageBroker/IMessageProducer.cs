namespace SKBKontur.Treller.MessageBroker
{
    public interface IMessageProducer
    {
        void Publish(Message message);
    }
}