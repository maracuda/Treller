using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService
{
    public interface IErrorService
    {
        string ErrorRecipientEmail { get; }
        void SendError(string title, Exception ex);
        void ChangeErrorRecipientEmail(string email);
    }
}