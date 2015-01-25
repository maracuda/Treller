using SKBKontur.TaskManagerClient.Youtrack.BusinessObjects;

namespace SKBKontur.TaskManagerClient.Abstractions
{
    public interface IYouTrackCredentialService
    {
        YouTrackCredential GetYouTrackCredentials();
    }
}