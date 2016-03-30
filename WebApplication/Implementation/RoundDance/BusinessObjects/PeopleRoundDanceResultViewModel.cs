using System;

namespace SKBKontur.Treller.WebApplication.Implementation.RoundDance.BusinessObjects
{
    public class PeopleRoundDanceResultViewModel
    {
        public string Name { get; set; }
        public string OldDirection { get; set; }
        public string FutureDirection { get; set; }
        public DateTime RoundDanceDate { get; set; }
        public string When { get; set; }
    }
}