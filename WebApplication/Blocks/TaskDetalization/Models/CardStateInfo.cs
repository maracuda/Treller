using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models
{
    public class CardStateInfo
    {
        public CardStateInfo(Dictionary<CardState, CardActionStateInfo> states, CardState currentState)
        {
            States = states;
            CurrentState = currentState;
        }

        public Dictionary<CardState, CardActionStateInfo> States { get; private set; }
        public CardState CurrentState { get; private set; }
    }
}