using SKBKontur.TaskManagerClient.CredentialServiceAbstractions;
using SKBKontur.TaskManagerClient.Wiki.BusinessObjects;

namespace SKBKontur.TaskManagerClient.Wiki
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