namespace SKBKontur.Treller.MessageBroker
{
    public interface INotificationService
    {
        void Send(Notification notification);
    }
}