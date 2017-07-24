using System;
using WebApplication.Implementation.Services.BoardsService;

namespace WebApplication.Implementation.Services.News.Domain.Models
{
    public class OutdatedBoardCardModel
    {
        public string CardId { get; set; }
        public string BoardListName { private get; set; }
        public DateTime LastActivity { private get; set; }
        public TimeSpan ExpirationPeriod { private get; set; }
        public bool IsArchived { private get; set; }

        public bool IsOutdated(DateTime at)
        {
            if (IsArchived)
                return true;
            return KanbanBoardTemplate.ReleasedListName.Equals(BoardListName) && LastActivity.Add(ExpirationPeriod) <= at;
        }
    }
}