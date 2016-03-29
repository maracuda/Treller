using System;
using System.Collections.Generic;
using SKBKontur.TaskManagerClient.BusinessObjects.TaskManager;
using SKBKontur.Treller.WebApplication.Implementation.Statistics;

namespace SKBKontur.Treller.WebApplication.Blocks.TaskDetalization.Models
{
    public class CardActionStateInfo
    {
        public CardActionStateInfo(CardState state, DateTime beginDate, User initiator)
        {
            BeginDate = beginDate;
            State = state;
            StateInitiator = initiator;
            NewStateUsers = new LinkedList<User>();
            StateComments = new LinkedList<string>();
            CheckListIds = new LinkedList<string>();
        }

        public CardState State { get; private set; }
        public User StateInitiator { get; private set; }
        public DateTime BeginDate { get; private set; }
        public DateTime? EndDate { get; set; }

        public TimeSpan StatePeriod
        {
            get
            {
                return EndDate.HasValue ? BeginDate.CalculatePeriod(EndDate.Value) : new TimeSpan(1000, 0, 0, 0);
            }
        }

        public LinkedList<User> NewStateUsers { get; set; }
        public LinkedList<string> StateComments { get; set; }
        public LinkedList<string> CheckListIds { get; set; }
    }
}