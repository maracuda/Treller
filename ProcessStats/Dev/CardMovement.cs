using System;

namespace ProcessStats.Dev
{
    public class CardMovement
    {
        private CardMovement() { }

        public string FromListId { get; private set; }
        public string FromListName { get; private set; }
        public string ToListId { get; private set; }
        public string ToListName { get; private set; }
        public DateTime Date { get; private set; }

        public static CardMovement Create(string fromListId, string fromListName, string toListId, string toListName, DateTime date)
        {
            return new CardMovement
            {
                FromListId = fromListId,
                FromListName = fromListName,
                ToListId = toListId,
                ToListName = toListName,
                Date = date
            };
        }
    }
}