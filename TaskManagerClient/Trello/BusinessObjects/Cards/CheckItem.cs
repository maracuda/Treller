using System;

namespace SKBKontur.TaskManagerClient.Trello.BusinessObjects.Cards
{
    public class CheckItem
    {
        public string Id { get; set; }
        public string State { get; set; }
        public string Name { get; set; }
        public double Pos { get; set; }
        public bool IsChecked { get { return string.Equals(State, "complete", StringComparison.OrdinalIgnoreCase); } }
    }
}