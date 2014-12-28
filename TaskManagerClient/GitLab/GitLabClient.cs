using System.Collections.Generic;
using SKBKontur.TaskManagerClient.Abstractions;

namespace SKBKontur.TaskManagerClient.GitLab
{
    public class GitLabClient
    {
        private readonly IHttpRequester httpClient;
        private readonly Dictionary<string, string> credentialParameters;

        public GitLabClient(IHttpRequester httpClient, IGitLabCredentialService gitLabCredentialService)
        {
            this.httpClient = httpClient;
            var credentials = gitLabCredentialService.GetCredentials();
            credentialParameters = new Dictionary<string, string>
                                   {
                                       {"private_token", credentials.PrivateToken},
                                       {"sudo", credentials.UserName}
                                   };
        }
    }
}