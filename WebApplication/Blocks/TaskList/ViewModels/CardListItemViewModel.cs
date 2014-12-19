using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.ViewModels;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskList.ViewModels
{
    public class CardListItemViewModel
    {
        public string CardId { get; set; }
        public string CardName { get; set; }
        public CardLabel[] Labels { get; set; }
        public UserAvatarViewModel[] Avatars { get; set; }
        public string StageInfo { get; set; }
        public string CardUrl { get; set; }
        public CardState State { get; set; }
    }
}