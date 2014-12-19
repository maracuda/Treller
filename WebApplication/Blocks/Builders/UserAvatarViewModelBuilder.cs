using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.ViewModels;

namespace SKBKontur.Treller.WebApplication.Blocks.Builders
{
    public class UserAvatarViewModelBuilder : IUserAvatarViewModelBuilder
    {
        public UserAvatarViewModel Build(User user)
        {
            return new UserAvatarViewModel
                       {
                           UserUrl = user.UserUrl,
                           Initials = user.Initials,
                           UserName = user.Name,
                           UserFullName = user.FullName,
                           Base64Image = user.AvatarInfo
                       };
        }
    }
}