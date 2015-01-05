using System;
using System.Collections.Generic;
using System.Linq;
using SKBKontur.BlocksMapping.BlockExtenssions;
using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.ViewModels;
using SKBKontur.Treller.WebApplication.Services.Settings;

namespace SKBKontur.Treller.WebApplication.Blocks.Builders
{
    public class CardStageInfoBuilder : ICardStageInfoBuilder
    {
        private readonly ICardStateBuilder cardStateBuilder;
        private readonly IChecklistParrotsBuilder checklistParrotsBuilder;
        private readonly IBugsBuilder bugsBuilder;

        public CardStageInfoBuilder(ICardStateBuilder cardStateBuilder, IChecklistParrotsBuilder checklistParrotsBuilder, IBugsBuilder bugsBuilder)
        {
            this.cardStateBuilder = cardStateBuilder;
            this.checklistParrotsBuilder = checklistParrotsBuilder;
            this.bugsBuilder = bugsBuilder;
        }

        public CardStageInfoViewModel Build(BoardCard card, CardAction[] actions, CardChecklist[] checklists, BoardSettings boardSetting, BoardList[] boardLists)
        {
            var state = cardStateBuilder.GetState(card.BoardListId, boardSetting, boardLists);

            DateTime? beginDate = null;
            DateTime? endDate = null;
            var cardChecklists = new HashSet<string>();
            foreach (var action in actions)
            {
                if (action.ToListId == card.BoardListId)
                {
                    beginDate = action.Date;
                }

                if (action.FromListId == card.BoardListId)
                {
                    endDate = action.Date;
                }

                if (beginDate.HasValue && !string.IsNullOrEmpty(action.CreatedCheckListId))
                {
                    cardChecklists.Add(action.CreatedCheckListId);
                }
            }
            beginDate = beginDate ?? (actions.Length > 0 ? actions[0].Date : (DateTime?) null);
            var lists = checklists.ToDictionary(x => x.Id);
            var resultLists = cardChecklists.Select(x => lists.SafeGet(x)).Where(x => x != null).ToArray();
            
            var totalDays = (int)(beginDate != null ? (DateTime.Now.Date - beginDate.Value.Date).TotalDays : 0);
            var parrotsInfo = checklistParrotsBuilder.Build(resultLists, totalDays, beginDate, endDate);
            var bugsInfo = bugsBuilder.Build(resultLists);

            return new CardStageInfoViewModel
                       {
                           State = state,
                           StageParrots = parrotsInfo,
                           Bugs = bugsInfo,
                       };
        }
    }
}