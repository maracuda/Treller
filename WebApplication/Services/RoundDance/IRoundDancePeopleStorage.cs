using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Services.RoundDance
{
    public interface IRoundDancePeopleStorage
    {
        RoundDancePeople[] GetAll();
        Dictionary<Direction, int> GetDirectionResourceNeeds();
    }
}