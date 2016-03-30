using System;
using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Implementation.RoundDance.BusinessObjects
{
    public class RoundDanceViewModel
    {
//        public Dictionary<string, RoundDancePeopleViewModel[]> DirectionPeoples { get; set; }
//        public Dictionary<string, List<RoundDancePeopleViewModel[]>> AnotherDirectionPeoples { get; set; }

        public Dictionary<string, PeopleRoundDanceViewModel[]> PeoplesByDirections { get; set; }
        public PeopleRoundDanceResultViewModel[] OldRoundDances { get; set; }
        public PeopleRoundDanceResultViewModel[] FutureRoundDances { get; set; }
        public string[] AllActualDirections { get; set; }
    }

    public class PeopleRoundDanceViewModel
    {
        public string Name { get; set; }
        public decimal CurrentWeight { get; set; }
        public decimal SpeedyWeight { get; set; }
        public decimal InfrastructureWeight { get; set; }
        public decimal DutyWeight { get; set; }
        public decimal FeatureWeight { get; set; }
    }

    public class PeopleRoundDanceResultViewModel
    {
        public string Name { get; set; }
        public string OldDirection { get; set; }
        public string FutureDirection { get; set; }
        public DateTime RoundDanceDate { get; set; }
        public string When { get; set; }
    }

    public class DutyViewModel
    {
        public PeopleDutyViewModel[] Peoples { get; set; }
    }

    public class PeopleDutyViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}