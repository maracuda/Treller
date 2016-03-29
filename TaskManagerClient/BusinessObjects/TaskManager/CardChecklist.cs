namespace SKBKontur.TaskManagerClient.BusinessObjects.TaskManager
{
    public class CardChecklist
    {
        public string Id { get; set; }
        public string CardId { get; set; }
        public string Name { get; set; }
        public double Position { get; set; }
        public ChecklistItem[] Items { get; set; }
    }
}