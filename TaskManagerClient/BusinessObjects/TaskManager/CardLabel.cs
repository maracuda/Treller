namespace TaskManagerClient.BusinessObjects.TaskManager
{
    public class CardLabel
    {
        public string Name { get; set; }
        public CardLabelColor Color { get; set; }

        protected bool Equals(CardLabel other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CardLabel) obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }
    }
}