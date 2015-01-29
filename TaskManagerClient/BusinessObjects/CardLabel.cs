namespace SKBKontur.TaskManagerClient.BusinessObjects
{
    public class CardLabel
    {
        public string Name { get; set; }
        public string ColorText { get { return Color.ToString().ToLower(); } }
        public CardLabelColor Color { get; set; }
    }
}