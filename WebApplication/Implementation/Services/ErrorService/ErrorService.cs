using System;
using SKBKontur.TaskManagerClient.Notifications;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService
{
    public class ErrorService : IErrorService
    {
        private const string NotificationFileName = "NotificationEmail";

        private readonly INotificationService notificationService;
        private readonly ICachedFileStorage cachedFileStorage;

        public ErrorService(
            INotificationService notificationService,
            ICachedFileStorage cachedFileStorage)
        {
            this.notificationService = notificationService;
            this.cachedFileStorage = cachedFileStorage;

            ErrorRecipientEmail = cachedFileStorage.Find<string>(NotificationFileName) ?? "hvorost@skbkontur.ru";
        }

        public string ErrorRecipientEmail { get; private set; }
        public void SendError(string title, Exception ex)
        {
            var notification = new Notification
            {
                Title = title,
                Body = $"{title}{Environment.NewLine}{ex}",
                IsHtml = false,
                Recipient = ErrorRecipientEmail,
            };
            notificationService.Send(notification);
        }

        public void ChangeErrorRecipientEmail(string email)
        {
            ErrorRecipientEmail = email;
            cachedFileStorage.Write(NotificationFileName, email);
        }
    }
}