using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Implementation.RoundDance.BusinessObjects
{
    public class RoundDancePeople
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public List<DirectionPeriod> WorkPeriods { get; set; }
    }
}