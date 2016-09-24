using System;
using SKBKontur.Treller.WebApplication.Implementation.Services.BoardsService;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Domain.Models
{
    public class AgingBoardCardModel
    {
        public string CardId { get; set; }
        public string BoardListName { private get; set; }
        public DateTime LastActivity { private get; set; }
        public DateTime ExpirationTime { private get; set; }
        public bool IsArchived { private get; set; }

        public bool IsGrowedOld()
        {
            if (IsArchived)
                return true;
            return KanbanBoardTemplate.ReleasedListName.Equals(BoardListName) && LastActivity < ExpirationTime;
        }
    }
}