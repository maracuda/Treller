using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.ViewModels;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.TaskManager
{
    public interface IUserAvatarViewModelBuilder
    {
        UserAvatarViewModel Build(User user);
    }
}