using System;

namespace TaskManagerClient.Trello.BusinessObjects.Cards
{
    public class Card
    {
        public string Id { get; set; }
        public int IdShort { get; set; }
        public string Name { get; set; }
        public double Pos { get; set; }
        public string ShortUrl { get; set; }
        public string Url { get; set; }
        public CardOverall Badges { get; set; }
        public bool Closed { get; set; }
        public DateTime DateLastActivity { get; set; }
        public string Desc { get; set; }
        public DateTime? Due { get; set; }
        public string IdBoard { get; set; }
        public string IdList { get; set; }
        public string[] IdMembers { get; set; }
        public string[] IdCheckLists { get; set; }
        public CardLabel[] Labels { get; set; }
    }
}

