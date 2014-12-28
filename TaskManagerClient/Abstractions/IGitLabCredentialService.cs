using SKBKontur.TaskManagerClient.GitLab.BusinessObjects;

namespace SKBKontur.TaskManagerClient.Abstractions
{
    public interface IGitLabCredentialService
    {
        GitLabCredential GetCredentials();
    }
}