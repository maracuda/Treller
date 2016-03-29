using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.Treller.WebApplication.Implementation.TaskDetalization.BusinessObjects.ViewModels;

namespace SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.ViewModels
{
    public class CardListItemViewModel
    {
        public string CardId { get; set; }
        public string CardName { get; set; }
        public string AnalyticLink { get; set; }
        public CardLabel[] Labels { get; set; }
        public UserAvatarViewModel[] Avatars { get; set; }
        public CardStageInfoViewModel StageInfo { get; set; }
        public BugsInfoViewModel Bugs { get; set; }
        public string CardUrl { get; set; }
        public bool IsNewCard { get; set; }
        public string BranchName { get; set; }
        public bool IsInCandidateRelease { get; set; }
    }
}