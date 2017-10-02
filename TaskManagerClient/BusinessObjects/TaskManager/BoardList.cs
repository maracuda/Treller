using System.Linq;

namespace TaskManagerClient.BusinessObjects.TaskManager
{
    public class BoardList
    {
        public BoardList(string id, string boardId, string name, BoardListCardInfo[] cardInfos = null)
        {
            Id = id;
            BoardId = boardId;
            Name = name;
            Cards = cardInfos;
        }

        public string Id { get; set; }
        public string BoardId { get; set; }
        public string Name { get; set; }
        public BoardListCardInfo[] Cards { get; set; }

        public static BoardList ConvertFrom(Trello.BusinessObjects.Boards.BoardList x)
        {
            if (x == null)
                return null;

            var cardInfos = x.Cards.Select(y => new BoardListCardInfo
            {
                Id = y.Id,
                Name = y.Name,
                Desc = y.Desc,
                Due = y.Due
            }).ToArray();
            return new BoardList(x.Id, x.IdBoard, x.Name, cardInfos);
        }


    }
}