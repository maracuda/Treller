using SKBKontur.TaskManagerClient.BusinessObjects;

namespace SKBKontur.TaskManagerClient.Abstractions
{
    public interface ITrelloUserCredentialService
    {
        TrelloCredential GetCredentials();
    }
}