using System.Collections.Generic;
using MessageBroker;

namespace RepositoryHooks.BranchNotification
{
    public interface IRepositoryMessageBuilder
    {
        EmailMessage CreateOldBranchesMessage(string commiterEmail, IEnumerable<string> oldBranchNames);
        EmailMessage CreateBranchDeletedMessage(string commiterEmail, string deletedBranchName);
    }
}