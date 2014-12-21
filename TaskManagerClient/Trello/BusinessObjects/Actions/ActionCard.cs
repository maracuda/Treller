namespace SKBKontur.TaskManagerClient.Trello.BusinessObjects.Actions
{
    public class ActionCard
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ShortLink { get; set; }
        public bool Closed { get; set; }
        public double Pos { get; set; }
    }
}