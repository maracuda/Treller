using System;
using Action = SKBKontur.TaskManagerClient.Trello.BusinessObjects.Actions.Action;

namespace SKBKontur.TaskManagerClient.BusinessObjects.TaskManager
{
    public class CardAction
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public string CardId { get; set; }
        public User Initiator { get; set; }
        public User AddedUser { get; set; }
        public string Comment { get; set; }
        public string BoardId { get; set; }
        public string ListId { get; set; }
        public string FromListId { get; set; }
        public string ToListId { get; set; }
        public string CreatedCheckListId { get; set; }
        public ActionType Type { get; set; }

        public static CardAction ConvertFrom(Action action)
        {
            return (action.Data.Card == null || action.ActionType == ActionType.Unknown || action.Data.Board == null || action.ActionType > ActionType.RemoveMemberFromBoard)
                    && action.ActionType != ActionType.AddMemberToBoard && action.ActionType != ActionType.RemoveMemberFromBoard
                       ? null
                       : new CardAction
                       {
                           Id = action.Id,
                           Date = action.Date,
                           Initiator = User.ConvertFrom(action.MemberCreator),
                           Type = action.ActionType,
                           BoardId = action.Data.Board == null ? string.Empty : action.Data.Board.Id,
                           CardId = action.Data.Card == null ? string.Empty : action.Data.Card.Id,
                           Comment = action.Data.Text,
                           AddedUser = action.Member != null ? User.ConvertFrom(action.Member) : null,
                           ListId = action.Data.List?.Id,
                           FromListId = action.Data.ListBefore?.Id,
                           ToListId = action.Data.ListAfter?.Id,
                           CreatedCheckListId = action.Data.Checklist?.Id
                       };
        }
    }
}