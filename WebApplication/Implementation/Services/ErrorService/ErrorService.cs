using System;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Storages;
using SKBKontur.Treller.WebApplication.Implementation.Services.Notifications;

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
            var body = $"{title}{Environment.NewLine}{ex}";
            notificationService.SendMessage(ErrorRecipientEmail, title, body, false);
        }

        public void ChangeErrorRecipientEmail(string email)
        {
            ErrorRecipientEmail = email;
            cachedFileStorage.Write(NotificationFileName, email);
        }
    }
}