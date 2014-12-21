using System;
using System.Collections.Generic;
using System.Linq;
using SKBKontur.TaskManagerClient.BusinessObjects;
using SKBKontur.Treller.WebApplication.Blocks.TaskList.ViewModels;
using SKBKontur.Treller.WebApplication.Services.Settings;
using SKBKontur.Billy.Core.BlocksMapping.BlockExtenssions;

namespace SKBKontur.Treller.WebApplication.Blocks.Builders
{
    public class CardStageInfoBuilder : ICardStageInfoBuilder
    {
        private readonly ICardStateBuilder cardStateBuilder;

        public CardStageInfoBuilder(ICardStateBuilder cardStateBuilder)
        {
            this.cardStateBuilder = cardStateBuilder;
        }

        public CardStageInfoViewModel Build(BoardCard card, CardAction[] actions, CardChecklist[] checklists, BoardSettings boardSetting, BoardList[] boardLists)
        {
            var state = cardStateBuilder.GetState(card.BoardListId, boardSetting, boardLists);

            DateTime? lastListChangedDate = null;
            var cardChecklists = new HashSet<string>();
            foreach (var action in actions)
            {
                if (action.ToListId == card.BoardListId)
                {
                    lastListChangedDate = action.Date;
                }
                if (lastListChangedDate.HasValue && !string.IsNullOrEmpty(action.CreatedCheckListId))
                {
                    cardChecklists.Add(action.CreatedCheckListId);
                }
            }
            var lists = checklists.ToDictionary(x => x.Id);
            var resultLists = cardChecklists.Select(x => lists.SafeGet(x)).Where(x => x != null).ToArray();
            
            var totalDays = (int)(lastListChangedDate != null ? (DateTime.Now.Date - lastListChangedDate.Value.Date).TotalDays : 0);

            return new CardStageInfoViewModel
                       {
                           DaysInState = totalDays,
                           State = state,
                           CurrentStateParrots = resultLists.Sum(l => l.Items.Count(i => i.IsChecked)),
                           TotalStateParrots = resultLists.Sum(l => l.Items.Length),
                           TotalCardParrots = checklists.Sum(l => l.Items.Length),
                           CurrentCardParrots = checklists.Sum(l => l.Items.Count(i => i.IsChecked)),
                       };
        }
    }
}