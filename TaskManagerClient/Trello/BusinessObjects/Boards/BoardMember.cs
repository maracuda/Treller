using SKBKontur.TaskManagerClient.Trello.BusinessObjects.Actions;

namespace SKBKontur.TaskManagerClient.Trello.BusinessObjects.Boards
{
    public class BoardMember : ActionMember
    {
        public string Bio { get; set; }
        public string Url { get; set; }
    }
}