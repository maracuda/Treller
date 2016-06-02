namespace SKBKontur.TaskManagerClient.BusinessObjects.TaskManager
{
    public class Board
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string OrganizationId { get; set; }
        public string Url { get; set; }
        public bool IsClosed { get; set; }

        public static Board ConvertFrom(Trello.BusinessObjects.Boards.Board trelloBoard)
        {
            return new Board
            {
                Id = trelloBoard.Id,
                Name = trelloBoard.Name,
                Url = trelloBoard.Url,
                OrganizationId = trelloBoard.IdOrganization,
                IsClosed = trelloBoard.Closed
            };
        }
    }
}