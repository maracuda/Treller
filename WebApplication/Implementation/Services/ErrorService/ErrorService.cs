using System;
using SKBKontur.TaskManagerClient.Notifications;
using SKBKontur.Treller.Storage;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService
{
    public class ErrorService : IErrorService
    {
        private const string NotificationFileName = "NotificationEmail";

        private readonly INotificationService notificationService;
        private readonly IKeyValueStorage keyValueStorage;

        public ErrorService(
            INotificationService notificationService,
            IKeyValueStorage keyValueStorage)
        {
            this.notificationService = notificationService;
            this.keyValueStorage = keyValueStorage;

            ErrorRecipientEmail = keyValueStorage.Find<string>(NotificationFileName) ?? "hvorost@skbkontur.ru";
        }

        public string ErrorRecipientEmail { get; private set; }
        public void SendError(string title, Exception ex)
        {
            SendError(title, $"{title}{Environment.NewLine}{ex}");
        }

        public void SendError(string title)
        {
            SendError(title, title);
        }

        private void SendError(string title, string body)
        {
            var notification = new Notification
            {
                Title = title,
                Body = body,
                Recipient = ErrorRecipientEmail,
            };
            notificationService.Send(notification);
        }

        public void ChangeErrorRecipientEmail(string email)
        {
            ErrorRecipientEmail = email;
            keyValueStorage.Write(NotificationFileName, email);
        }
    }
}