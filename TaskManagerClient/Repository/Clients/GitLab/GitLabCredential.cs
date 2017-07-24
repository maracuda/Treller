namespace TaskManagerClient.Repository.Clients.GitLab
{
    public class GitLabCredential
    {
        public string PrivateToken { get; set; }
        public string UserName { get; set; }
        public string DefaultUrl { get; set; }
    }
}