using System;
using SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Extensions;

namespace SKBKontur.Treller.WebApplication.Implementation.RoundDance.BusinessObjects
{
    public class DirectionPeriod
    {
        public string Direction { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string PairName { get; set; }

        public Direction? DefaultDirection
        {
            set
            {
                Direction = value.GetDescription();
            }
        }

        public void SetNextPeriod(DirectionPeriod nextPeriod)
        {
            EndDate = nextPeriod.BeginDate.AddDays(-1);
        }

        public string GetPeriodString()
        {
            return EndDate.HasValue
                ? $"{BeginDate:dd.MM.yyyy} - {EndDate:dd.MM.yyyy}"
                : BeginDate.ToString("dd.MM.yyyy");
        }
    }
}