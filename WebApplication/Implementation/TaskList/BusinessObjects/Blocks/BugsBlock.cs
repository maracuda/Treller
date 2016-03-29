using SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.ViewModels;

namespace SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.Blocks
{
    public class BugsBlock : BaseCardListBlock
    {
        public BugsCountLinkInfoViewModel BattleUnassigned { get; set; }
        public BugsCountLinkInfoViewModel BattleAssigned { get; set; }
        public BugsCountLinkInfoViewModel BillyCurrent { get; set; }
        public BugsCountLinkInfoViewModel BillyAll { get; set; }
        public BugsCountLinkInfoViewModel CsCurrent { get; set; }
        public BugsCountLinkInfoViewModel BillyNotVerified { get; set; }
    }
}