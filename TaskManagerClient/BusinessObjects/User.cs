namespace SKBKontur.TaskManagerClient.BusinessObjects
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string AvatarHash { get; set; }
        public string AvatarSrc 
        {
            get
            {
                return !string.IsNullOrEmpty(AvatarHash)
                           ? string.Format("https://trello-avatars.s3.amazonaws.com/{0}/30.png", AvatarHash)
                           : null;
            }
        }
        public string Initials { get; set; }
        public string UserUrl { get; set; }
    }
}