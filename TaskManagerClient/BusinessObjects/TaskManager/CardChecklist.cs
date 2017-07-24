using System.Linq;
using TaskManagerClient.Trello.BusinessObjects.Cards;

namespace TaskManagerClient.BusinessObjects.TaskManager
{
    public class CardChecklist
    {
        public string Id { get; set; }
        public string CardId { get; set; }
        public string Name { get; set; }
        public double Position { get; set; }
        public ChecklistItem[] Items { get; set; }

        public static CardChecklist ConvertFrom(Checklist list)
        {
            return new CardChecklist
            {
                Id = list.Id,
                Name = list.Name,
                CardId = list.IdCard,
                Position = list.Pos,
                Items = list.CheckItems.Select(i => new ChecklistItem
                {
                    Id = i.Id,
                    Description = i.Name,
                    Position = i.Pos,
                    IsChecked = i.IsChecked,
                }).ToArray()
            };
        }
    }
}