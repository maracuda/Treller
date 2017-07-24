namespace TaskManagerClient.Trello.BusinessObjects.Cards
{
    public class Checklist
    {
        public Checklist()
        {
            CheckItems = new CheckItem[0];
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string IdBoard { get; set; }
        public string IdCard { get; set; }
        public double Pos { get; set; }
        public CheckItem[] CheckItems { get; set; }
    }
}