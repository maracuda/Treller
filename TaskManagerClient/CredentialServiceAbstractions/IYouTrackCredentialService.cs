using TaskManagerClient.Youtrack.BusinessObjects;

namespace TaskManagerClient.CredentialServiceAbstractions
{
    public interface IYouTrackCredentialService
    {
        YouTrackCredential GetYouTrackCredentials();
    }
}