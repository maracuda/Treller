using System.Collections.Generic;
using SKBKontur.Treller.MessageBroker;

namespace SKBKontur.Treller.RepositoryHooks.BranchNotification
{
    public interface IRepositoryNotificationBuilder
    {
        Message BuildForOldBranch(string commiterEmail, IEnumerable<string> oldBranches);
        Message BuildForReleasedBranch(string commiterEmail, IEnumerable<string> releasedBranches);
    }
}