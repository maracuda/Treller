using System;
using SKBKontur.TaskManagerClient.BusinessObjects;

namespace SKBKontur.TaskManagerClient.Trello.BusinessObjects.Actions
{
    public class Action
    {
        public string Id { get; set; }
        public string IdMemberCreator { get; set; }
        public DateTime Date { get; set; }
        public ActionMember MemberCreator { get; set; }
        public string Type { get; set; }
        public ActionData Data { get; set; }
        public ActionMember Member { get; set; }

        public ActionType ActionType
        {
            get
            {
                ActionType actionType;
                return string.IsNullOrEmpty(Type) || !Enum.TryParse(Type, true, out actionType) ? ActionType.Unknown : actionType;
            }
        }
    }
}