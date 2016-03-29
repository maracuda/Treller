using System;
using System.Collections.Generic;
using System.Linq;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models;
using SKBKontur.Treller.WebApplication.Implementation.Services.Settings;

namespace SKBKontur.Treller.WebApplication.Blocks.Builders
{
    public class CardStateInfoBuilder : ICardStateInfoBuilder
    {
        private readonly ICardStateBuilder cardStateBuilder;

        public CardStateInfoBuilder(ICardStateBuilder cardStateBuilder)
        {
            this.cardStateBuilder = cardStateBuilder;
        }

        public CardStateInfo Build(CardAction[] actions, Dictionary<string, BoardSettings> boardSettings, Dictionary<string, BoardList[]> boardLists)
        {
            var currentState = CardState.Unknown;
            var firstAction = actions.OrderBy(x => x.Date).FirstOrDefault() ?? new CardAction { Date = new DateTime(2014, 1, 1) };
            var states = new Dictionary<CardState, CardActionStateInfo>(8) { { CardState.Unknown, new CardActionStateInfo(CardState.Unknown, firstAction.Date, firstAction.Initiator ) } };

            foreach (var action in actions.OrderBy(x => x.Date))
            {
                BoardSettings setting;
                if (!boardSettings.TryGetValue(action.BoardId, out setting))
                {
                    if (currentState >= CardState.Develop && currentState <= CardState.Testing)
                    {
                        states[currentState].EndDate = action.Date;
                        currentState = CardState.Unknown;
                    }

                    continue;
                }

                if (action.ToListId != null || action.ListId != null)
                {
                    var newState = boardLists.ContainsKey(action.BoardId) ? cardStateBuilder.GetState(action.ToListId ?? action.ListId, setting, boardLists[action.BoardId]) : CardState.BeforeDevelop;
                    if (newState != currentState)
                    {
                        if (states.ContainsKey(currentState))
                        {
                            states[currentState].EndDate = action.Date;
                        }
                        if (!states.ContainsKey(newState))
                        {
                            states.Add(newState, new CardActionStateInfo(newState, action.Date, action.Initiator));
                        }
                        states[newState].EndDate = null;
                        currentState = newState;
                    }
                }

                if (currentState == CardState.Unknown)
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(action.Comment))
                {
                    states[currentState].StateComments.AddLast(action.Comment);
                }

                if (action.AddedUser != null)
                {
                    states[currentState].NewStateUsers.AddLast(action.AddedUser);
                }

                if (!string.IsNullOrEmpty(action.CreatedCheckListId))
                {
                    states[currentState].CheckListIds.AddLast(action.CreatedCheckListId);
                }
            }

            return new CardStateInfo(states, currentState);
        }
    }
}