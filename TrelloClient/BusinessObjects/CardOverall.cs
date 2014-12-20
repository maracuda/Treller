using System;

namespace SKBKontur.Treller.TrelloClient.BusinessObjects
{
    public class CardOverall
    {
        public int Attachments { get; set; }

        public int CheckItems { get; set; }

        public int CheckItemsChecked { get; set; }

        public int Comments { get; set; }

        public bool Description { get; set; }

        public DateTime? Due { get; set; }

        public string FogBugz { get; set; }

        public int Votes { get; set; }
    }
}