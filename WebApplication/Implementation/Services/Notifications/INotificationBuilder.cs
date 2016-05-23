using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Notifications
{
    public interface INotificationBuilder
    {
        Notification BuildForOldBranchNotification(string commiterEmail, IEnumerable<string> oldBranches);
    }
}