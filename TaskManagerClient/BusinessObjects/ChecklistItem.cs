namespace SKBKontur.TaskManagerClient.BusinessObjects
{
    public class ChecklistItem
    {
        public string Id { get; set; }
        public bool IsChecked { get; set; }
        public string Description { get; set; }
        public double Position { get; set; }
    }
}