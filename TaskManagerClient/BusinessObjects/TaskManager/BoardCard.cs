using System;
using System.Linq;
using SKBKontur.TaskManagerClient.Trello.BusinessObjects.Cards;

namespace SKBKontur.TaskManagerClient.BusinessObjects.TaskManager
{
    public class BoardCard
    {
        public string Id { get; set; }
        public string BoardId { get; set; }
        public string BoardListId { get;  set; }
        public string Name { get; set; }
        public double Position { get; set; }
        public string Description { get; set; }
        public CardLabel[] Labels { get; set; }
        public DateTime LastActivity { get; set; }
        public string[] CheckListIds { get; set; }
        public string[] UserIds { get; set; }
        public string Url { get; set; }
        public DateTime? DueDate { get; set; }
        public bool IsArchived { get; set; }

        public static BoardCard ConvertFrom(Card card)
        {
            CardLabelColor result;
            return new BoardCard
            {
                Id = card.Id,
                Url = card.Url,
                DueDate = card.Due,
                BoardId = card.IdBoard,
                Name = card.Name,
                Position = card.Pos,
                BoardListId = card.IdList,
                Description = card.Desc,
                Labels = card.Labels.Select(cardLabel =>
                    new CardLabel
                    {
                        Name = cardLabel.Name,
                        Color = Enum.TryParse(cardLabel.Color, true, out result) ? result : CardLabelColor.Undefined
                    }).ToArray(),
                LastActivity = card.DateLastActivity,
                UserIds = card.IdMembers.ToArray(),
                CheckListIds = card.IdCheckLists,
                IsArchived = card.Closed
            };
        }
    }
}