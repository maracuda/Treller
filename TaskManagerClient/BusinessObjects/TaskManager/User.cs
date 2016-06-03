using SKBKontur.TaskManagerClient.Trello.BusinessObjects.Actions;
using SKBKontur.TaskManagerClient.Trello.BusinessObjects.Boards;

namespace SKBKontur.TaskManagerClient.BusinessObjects.TaskManager
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string AvatarHash { get; set; }
        public string AvatarSrc => !string.IsNullOrEmpty(AvatarHash) ? $"https://trello-avatars.s3.amazonaws.com/{AvatarHash}/30.png" : null;

        public string Initials { get; set; }
        public string UserUrl { get; set; }

        public static User ConvertFrom(ActionMember actionMember)
        {
            var boardMember = actionMember as BoardMember;

            return new User
            {
                Id = actionMember.Id,
                AvatarHash = actionMember.AvatarHash,
                FullName = actionMember.FullName,
                Name = actionMember.Username,
                Initials = actionMember.Initials,
                UserUrl = boardMember?.Url
            };
        }
    }
}