using SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.ViewModels;

namespace SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.Blocks
{
    public class CardListBlock : BaseCardListBlock
    {
        public CardStateOverallViewModel[] OverallStateCards { get; set; }
    }
}