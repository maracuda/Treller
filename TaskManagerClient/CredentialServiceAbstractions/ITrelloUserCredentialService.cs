using SKBKontur.TaskManagerClient.Trello.BusinessObjects;

namespace SKBKontur.TaskManagerClient.CredentialServiceAbstractions
{
    public interface ITrelloUserCredentialService
    {
        TrelloCredential GetCredentials();
    }
}