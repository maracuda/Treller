using System;
using System.Collections.Generic;
using System.Linq;
using SKBKontur.Infrastructure.CommonExtenssions;

namespace SKBKontur.Treller.WebApplication.Implementation.RoundDance.BusinessObjects
{
    public class RoundDancePeople
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public List<DirectionPeriod> WorkPeriods { get; set; }

        public string GetCurrentDirection
        {
            get
            {
                var now = DateTime.Now;
                var current = WorkPeriods.FirstOrDefault(x => now > x.BeginDate && x.EndDate.HasValue && now < x.EndDate) ?? WorkPeriods.LastOrDefault(x => x.BeginDate <= now);
                return current != null ? current.Direction : Direction.Leave.GetEnumDescription();
            }
        }
    }
}