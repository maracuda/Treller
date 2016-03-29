using SKBKontur.TaskManagerClient.GitLab.BusinessObjects;

namespace SKBKontur.TaskManagerClient.CredentialServiceAbstractions
{
    public interface IGitLabCredentialService
    {
        GitLabCredential GetGitLabCredentials();
    }
}