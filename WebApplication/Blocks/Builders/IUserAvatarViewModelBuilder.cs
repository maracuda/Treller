using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.ViewModels;

namespace SKBKontur.Treller.WebApplication.Blocks.Builders
{
    public interface IUserAvatarViewModelBuilder
    {
        UserAvatarViewModel Build(User user);
    }
}