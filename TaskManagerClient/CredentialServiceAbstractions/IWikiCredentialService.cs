using TaskManagerClient.Wiki.BusinessObjects;

namespace TaskManagerClient.CredentialServiceAbstractions
{
    public interface IWikiCredentialService
    {
        WikiCredential GetWikiCredentials();
    }
}