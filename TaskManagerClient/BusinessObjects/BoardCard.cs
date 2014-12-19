using System;

namespace SKBKontur.TaskManagerClient.BusinessObjects
{
    public class BoardCard
    {
        public string Id { get; set; }
        public string BoardId { get; set; }
        public string BoardListId { get; set; }
        public string Name { get; set; }
        public double Position { get; set; }
        public string Description { get; set; }
        public CardLabel[] Labels { get; set; }
        public DateTime LastActivity { get; set; }
        public string[] UserIds { get; set; }
        public CardChecklist[] CheckLists { get; set; }
        public string Url { get; set; }
        public DateTime? DueDate { get; set; }
    }
}