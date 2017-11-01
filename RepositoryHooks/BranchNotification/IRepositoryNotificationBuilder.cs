using System.Collections.Generic;
using MessageBroker;

namespace RepositoryHooks.BranchNotification
{
    public interface IRepositoryNotificationBuilder
    {
        EmailMessage Build(string commiterEmail, IEnumerable<string> oldBranches);
    }
}