using System.Collections.Generic;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskList.Blocks
{
    public class CardListBlock : BaseCardListBlock
    {
        public Dictionary<CardState, CardStateOverallViewModel> OverallStateCards { get; set; }
    }
}