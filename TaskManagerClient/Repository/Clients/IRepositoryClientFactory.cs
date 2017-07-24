namespace TaskManagerClient.Repository.Clients
{
    public interface IRepositoryClientFactory
    {
        IRepositoryClient CreateGitLabClient(string repoId);
    }
}