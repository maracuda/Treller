using System;
using System.ComponentModel;

namespace TaskManagerClient.Trello.BusinessObjects.Actions
{
    public class ActionData
    {
        public ActionBoard Board { get; set; }
        public ActionCard Card { get; set; }
        public ActionAttachment Attachment { get; set; }
        public ActionChecklist Checklist { get; set; }
        public ActionOrganization Organization { get; set; }
        public string IdMember { get; set; }
        public OldUpdatedValue Old { get; set; }
        [Description("Comment")]
        public string Text { get; set; }
        public DateTime? DateLastEdited { get; set; }
        public ActionCard CardSource { get; set; }
        public ActionChecklistItem CheckItem { get; set; }
        public ActionList List { get; set; }
        public ActionBoard BoardTarget { get; set; }
        public ActionBoard BoardSource { get; set; }
        public ActionList ListBefore { get; set; }
        public ActionList ListAfter { get; set; }
    }
}