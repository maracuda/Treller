using System;

namespace SKBKontur.Treller.WebApplication.Controllers.RoundDance
{
    public class DirectionPeriod
    {
        public Direction Direction { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; private set; }

        public void SetNextPeriod(DirectionPeriod nextPeriod)
        {
            EndDate = nextPeriod.BeginDate.AddDays(-1);
        }

        public void SetAsCurrentPeriod()
        {
            EndDate = DateTime.Now.Date;
        }
    }
}