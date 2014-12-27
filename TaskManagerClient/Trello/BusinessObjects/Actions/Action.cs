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
        public ActionType Type { get; set; }
        public ActionData Data { get; set; }
        public ActionMember Member { get; set; }
    }
}