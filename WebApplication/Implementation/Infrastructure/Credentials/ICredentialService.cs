using SKBKontur.TaskManagerClient.CredentialServiceAbstractions;
using SKBKontur.TaskManagerClient.Repository.Clients.GitLab;

namespace SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Credentials
{
    public interface ICredentialService : ITrelloUserCredentialService, IGitLabCredentialService, IYouTrackCredentialService, IWikiCredentialService
    {
        DomainCredentials MessageBrokerCredentials { get; }
    }
}