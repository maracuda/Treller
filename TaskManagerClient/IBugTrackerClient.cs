using TaskManagerClient.BusinessObjects.BugTracker;
using TaskManagerClient.Youtrack.BusinessObjects;

namespace TaskManagerClient
{
    public interface IBugTrackerClient
    {
        Issue[] GetFiltered(string filter);
        int GetFilteredCount(string filter);

        BugTrackerIssueAttachment[] GetAttachments(string issueId);
        BugTrackerIssueComment[] GetComments(string issueId);

        void DeleteAttachment(string issueId, string attachmentId);
        void DeleteComment(string issueId, string commentId, bool permanently);
    }
}