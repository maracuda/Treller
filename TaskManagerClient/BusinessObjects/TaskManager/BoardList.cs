using System.Linq;

namespace SKBKontur.TaskManagerClient.BusinessObjects.TaskManager
{
    public class BoardList
    {
        public string Id { get; set; }
        public string BoardId { get; set; }
        public string Name { get; set; }
        public double Position { get; set; }
        public BoardListCardInfo[] Cards { get; set; }

        public static BoardList ConvertFrom(Trello.BusinessObjects.Boards.BoardList x)
        {
            return new BoardList
            {
                Id = x.Id,
                BoardId = x.IdBoard,
                Name = x.Name,
                Position = x.Pos,
                Cards = x.Cards.Select(y => new BoardListCardInfo
                {
                    Id = y.Id,
                    Name = y.Name,
                    Desc = y.Desc,
                    Due = y.Due
                }).ToArray()
            };
        }
    }
}