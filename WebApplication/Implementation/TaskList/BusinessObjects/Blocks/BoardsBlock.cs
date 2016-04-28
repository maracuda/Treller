using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.TaskManagerClient.Repository.BusinessObjects;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Abstractions;

namespace SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.Blocks
{
    public class BoardsBlock : BaseBlock
    {
        public Board[] Boards { get; set; }
        public ReleasedBranch[] BranchesMergedToReleaseCandidate { get; set; }
    }
}