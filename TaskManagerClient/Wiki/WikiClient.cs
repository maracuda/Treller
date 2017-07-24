using TaskManagerClient.CredentialServiceAbstractions;
using TaskManagerClient.Wiki.BusinessObjects;

namespace TaskManagerClient.Wiki
{
    public class WikiClient : IWikiClient
    {
        private readonly WikiCredential wikiCredentials;

        public WikiClient(IWikiCredentialService wikiCredentialService)
        {
            wikiCredentials = wikiCredentialService.GetWikiCredentials();
        }

        public string GetBaseUrl()
        {
            return wikiCredentials.DefaultUrl;
        }
    }
}