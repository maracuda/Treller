using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Abstractions;
using SKBKontur.Treller.WebApplication.Implementation.Services.Repository;

namespace SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.Blocks
{
    public class BoardsBlock : BaseBlock
    {
        public Board[] Boards { get; set; }
        public RepoBranchModel[] BranchesMergedToReleaseCandidate { get; set; }
    }
}