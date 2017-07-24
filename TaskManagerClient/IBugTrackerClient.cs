using SKBKontur.TaskManagerClient.BusinessObjects.BugTracker;
using SKBKontur.TaskManagerClient.Youtrack.BusinessObjects;

namespace SKBKontur.TaskManagerClient
{
    public interface IBugTrackerClient
    {
        Issue[] GetFiltered(string filter);
        int GetFilteredCount(string filter);
        Issue[] GetSprintInfo(string sprintName);
        string GetIssueUrl();
        string GetSprintUrl();
        string GetStrintUrlEndWord();
        string GetBaseUrl();

        BugTrackerIssueAttachment[] GetAttachments(string issueId);
        BugTrackerIssueComment[] GetComments(string issueId);

        void DeleteAttachment(string issueId, string attachmentId);
        void DeleteComment(string issueId, string commentId, bool permanently);
    }
}