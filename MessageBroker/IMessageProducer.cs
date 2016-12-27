namespace SKBKontur.Treller.MessageBroker
{
    public interface IMessageProducer
    {
        void Publish(Message message);

        //TODO: remove this after fix problem with contaier configuration
        void Configure(string login,
            string password,
            string domain,
            string smtpHost,
            int smtpPort);
    }
}