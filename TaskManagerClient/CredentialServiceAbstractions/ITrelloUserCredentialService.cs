using TaskManagerClient.Trello.BusinessObjects;

namespace TaskManagerClient.CredentialServiceAbstractions
{
    public interface ITrelloUserCredentialService
    {
        TrelloCredential GetCredentials();
    }
}