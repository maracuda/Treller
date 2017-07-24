using System;

namespace ProcessStats.Dev
{
    public class CardMovement
    {
        private CardMovement() { }

        public DevelopingProcessStage From { get; private set; }
        public DevelopingProcessStage To { get; private set; }
        public DateTime Date { get; private set; }

        public static CardMovement Create(DevelopingProcessStage from, DevelopingProcessStage to, DateTime date)
        {
            return new CardMovement
            {
                From = from,
                To = to,
                Date = date
            };
        }
    }
}