using System;

namespace SKBKontur.Treller.WebApplication.Services.Notifications
{
    public interface INotificationService
    {
        void SendErrorReport(string errorHeader, Exception ex);
        void SendMessage(string recipientEmail, string messageHeader, string messageBody, bool inHtmlStyle);
    }
}