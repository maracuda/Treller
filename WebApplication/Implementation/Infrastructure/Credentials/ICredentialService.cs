using TaskManagerClient.CredentialServiceAbstractions;
using TaskManagerClient.Repository.Clients.GitLab;

namespace WebApplication.Implementation.Infrastructure.Credentials
{
    public interface ICredentialService : ITrelloUserCredentialService, IGitLabCredentialService, IYouTrackCredentialService, IWikiCredentialService
    {
        DomainCredentials MessageBrokerCredentials { get; }
    }
}