using System;

namespace TaskManagerClient.Trello.BusinessObjects.Boards
{
    public class BoardListCardInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? Due { get; set; }
        public string Desc { get; set; }
    }
}