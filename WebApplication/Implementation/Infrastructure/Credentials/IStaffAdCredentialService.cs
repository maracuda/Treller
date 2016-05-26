using SKBKontur.TaskManagerClient.CredentialServiceAbstractions;

namespace SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Credentials
{
    public interface IStaffAdCredentialService
    {
        DomainCredentials GetStaffCredentials();
    }
}