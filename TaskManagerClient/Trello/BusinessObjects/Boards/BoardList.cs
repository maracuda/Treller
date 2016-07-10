namespace SKBKontur.TaskManagerClient.Trello.BusinessObjects.Boards
{
    public class BoardList
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Closed { get; set; }
        public string IdBoard { get; set; }
        public double Pos { get; set; }
        public BoardListCardInfo[] Cards { get; set; }
    }
}