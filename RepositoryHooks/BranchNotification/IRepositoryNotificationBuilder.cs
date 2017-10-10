using System.Collections.Generic;
using MessageBroker;

namespace RepositoryHooks.BranchNotification
{
    public interface IRepositoryNotificationBuilder
    {
        Message BuildForOldBranch(string commiterEmail, IEnumerable<string> oldBranches);
        Message BuildForReleasedBranch(string commiterEmail, IEnumerable<string> releasedBranches);

        Message Build(string commiterEmail, IEnumerable<string> mergedBranches, IEnumerable<string> oldBranches);
    }
}