using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.ViewModels;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Blocks
{
    public class CardAvatarBlock : BaseTaskDetalizationBlock
    {
        public UserAvatarViewModel[] SuggestedNotActiveAvatars { get; set; }
        public UserAvatarViewModel[] SuggestedActiveAvatars { get; set; }
    }
}