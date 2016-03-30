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

        public string GetPeriodString()
        {
            return EndDate.HasValue
                ? string.Format("{0:dd.MM.yyyy} - {1:dd.MM.yyyy}", BeginDate, EndDate)
                : BeginDate.ToString("dd.MM.yyyy");
        }
    }
}