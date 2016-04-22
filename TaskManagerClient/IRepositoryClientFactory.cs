namespace SKBKontur.TaskManagerClient
{
    public interface IRepositoryClientFactory
    {
        IRepositoryClient CreateGitLabClient(string repoId);
    }
}