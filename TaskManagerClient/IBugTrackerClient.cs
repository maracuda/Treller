using SKBKontur.TaskManagerClient.Youtrack.BusinessObjects;

namespace SKBKontur.TaskManagerClient
{
    public interface IBugTrackerClient
    {
        Issue[] GetFiltered(string filter);
        Issue[] GetSprintInfo(string sprintName);
        string GetIssueUrl();
        string GetSprintUrl();
        string GetBaseUrl();
    }
}