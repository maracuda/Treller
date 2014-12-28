using SKBKontur.TaskManagerClient.Youtrack;

namespace SKBKontur.TaskManagerClient
{
    public interface IBugTrackerClient
    {
        Issue[] GetFiltered(string filter);
        string GetIssueUrl();
        string GetSprintUrl();
    }
}