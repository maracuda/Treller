using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.Treller.WebApplication.Blocks.Builders;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskList.Blocks
{
    public class BoardsBlock : BaseCardListBlock
    {
        public Board[] Boards { get; set; }
        public SimpleRepoBranch[] BranchesInCandidateRelease { get; set; }
    }
}