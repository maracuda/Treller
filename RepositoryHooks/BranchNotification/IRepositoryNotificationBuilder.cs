using System.Collections.Generic;
using MessageBroker;

namespace RepositoryHooks.BranchNotification
{
    public interface IRepositoryNotificationBuilder
    {
        Message Build(string commiterEmail, IEnumerable<string> mergedBranches, IEnumerable<string> oldBranches);
    }
}