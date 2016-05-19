namespace SKBKontur.Treller.WebApplication.Implementation.Services.Notifications
{
    public interface INotificationService
    {
        void SendMessage(string recipientEmail, string messageTitle, string messageBody, bool inHtmlStyle, string replyTo = null);
    }
}