using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.ErrorService
{
    public interface IErrorService
    {
        void SendError(string title, Exception ex);
        void SendError(string title);
    }
}