using SKBKontur.TaskManagerClient.CredentialServiceAbstractions;

namespace SKBKontur.TaskManagerClient.Notifications
{
    public interface INotificationCredentialsService
    {
        DomainCredentials GetDeliveryCredentials();
    }
}