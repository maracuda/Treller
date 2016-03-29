using SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.ViewModels;

namespace SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.Blocks
{
    public class CardAvatarBlock : BaseTaskDetalizationBlock
    {
        public UserAvatarViewModel[] SuggestedNotActiveAvatars { get; set; }
        public UserAvatarViewModel[] SuggestedActiveAvatars { get; set; }
    }
}