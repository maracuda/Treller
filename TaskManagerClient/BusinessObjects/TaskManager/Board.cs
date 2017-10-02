namespace TaskManagerClient.BusinessObjects.TaskManager
{
    public class Board
    {
        public Board(string id, string name, bool isClosed = false)
        {
            Id = id;
            Name = name;
            IsClosed = isClosed;
        }

        public string Id { get; }
        public string Name { get; }
        public bool IsClosed { get; }

        public static Board ConvertFrom(Trello.BusinessObjects.Boards.Board trelloBoard)
        {
            return new Board(trelloBoard.Id, trelloBoard.Name, trelloBoard.Closed);
        }

        protected bool Equals(Board other)
        {
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Board) obj);
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }
    }
}