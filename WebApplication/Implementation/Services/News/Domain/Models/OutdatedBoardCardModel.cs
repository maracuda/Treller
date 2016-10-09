using System;
using SKBKontur.Treller.WebApplication.Implementation.Services.BoardsService;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Domain.Models
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