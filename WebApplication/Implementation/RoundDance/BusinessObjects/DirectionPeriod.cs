using System;
using SKBKontur.Infrastructure.CommonExtenssions;

namespace SKBKontur.Treller.WebApplication.Implementation.RoundDance.BusinessObjects
{
    public class DirectionPeriod
    {
        public string Direction { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; private set; }
        public string PairName { get; set; }

        public Direction? DefaultDirection
        {
            set
            {
                Direction = value.GetEnumDescription();
            }
        }

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