using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.ViewModels;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.TaskManager
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
                           Base64Image = user.AvatarHash,
                           AvatarSrc = user.AvatarSrc
                       };
        }
    }
}