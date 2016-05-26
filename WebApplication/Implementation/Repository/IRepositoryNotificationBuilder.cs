using System.Collections.Generic;
using SKBKontur.TaskManagerClient.Notifications;

namespace SKBKontur.Treller.WebApplication.Implementation.Repository
{
    public interface IRepositoryNotificationBuilder
    {
        Notification BuildForOldBranch(string commiterEmail, IEnumerable<string> oldBranches);
        Notification BuildForReleasedBranch(string commiterEmail, IEnumerable<string> releasedBranches);
    }
}