using System.Collections.Generic;
using SKBKontur.TaskManagerClient.BusinessObjects;

namespace SKBKontur.TaskManagerClient.Trello.BusinessObjects.Boards
{
    public class Board
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public bool Closed { get; set; }
        public string IdOrganization { get; set; }
        public string Url { get; set; }
        public Dictionary<CardLabelColor, string> LabelNames { get; set; }
    }
}