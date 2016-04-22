using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.Treller.WebApplication.Implementation.Services.Repository;

namespace SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.Blocks
{
    public class BoardsBlock : BaseCardListBlock
    {
        public Board[] Boards { get; set; }
        public RepoBranchModel[] BranchesModelInCandidateRelease { get; set; }
    }
}