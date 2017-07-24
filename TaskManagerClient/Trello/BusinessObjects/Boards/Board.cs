namespace TaskManagerClient.Trello.BusinessObjects.Boards
{
    public class Board
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public bool Closed { get; set; }
        public string IdOrganization { get; set; }
        public string Url { get; set; }
    }
}