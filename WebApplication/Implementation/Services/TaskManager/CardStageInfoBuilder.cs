using System;
using System.Collections.Generic;
using System.Linq;
using SKBKontur.BlocksMapping.BlockExtenssions;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.Treller.WebApplication.Implementation.TaskList.BusinessObjects.ViewModels;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.TaskManager
{
    public class CardStageInfoBuilder : ICardStageInfoBuilder
    {
        private readonly ICardStateBuilder cardStateBuilder;
        private readonly IChecklistParrotsBuilder checklistParrotsBuilder;

        public CardStageInfoBuilder(ICardStateBuilder cardStateBuilder, IChecklistParrotsBuilder checklistParrotsBuilder)
        {
            this.cardStateBuilder = cardStateBuilder;
            this.checklistParrotsBuilder = checklistParrotsBuilder;
        }

        public CardStageInfoViewModel Build(BoardCard card, CardAction[] actions, CardChecklist[] checklists, BoardList[] boardLists)
        {
            var state = cardStateBuilder.GetState(card.BoardListId, boardLists);

            DateTime? beginDate = null;
            DateTime? endDate = null;
            var cardChecklists = new HashSet<string>();
            foreach (var action in actions.OrderBy(x => x.Date))
            {
                if (!beginDate.HasValue && (action.ToList?.Id == card.BoardListId || action.List?.Id == card.BoardListId))
                {
                    beginDate = action.Date;
                }

                if (action.FromList?.Id == card.BoardListId)
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

            return new CardStageInfoViewModel
                       {
                           State = state,
                           StageParrots = parrotsInfo
                       };
        }
    }
}