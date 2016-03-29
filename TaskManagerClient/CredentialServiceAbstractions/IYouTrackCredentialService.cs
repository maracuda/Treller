using SKBKontur.TaskManagerClient.Youtrack.BusinessObjects;

namespace SKBKontur.TaskManagerClient.CredentialServiceAbstractions
{
    public interface IYouTrackCredentialService
    {
        YouTrackCredential GetYouTrackCredentials();
    }
}