using TaskManagerClient.Trello.BusinessObjects.Actions;

namespace TaskManagerClient.Trello.BusinessObjects.Boards
{
    public class BoardMember : ActionMember
    {
        public string Bio { get; set; }
        public string Url { get; set; }
    }
}