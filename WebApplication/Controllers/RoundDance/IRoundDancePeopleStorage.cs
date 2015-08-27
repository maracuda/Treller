using System.Collections.Generic;

namespace SKBKontur.Treller.WebApplication.Controllers.RoundDance
{
    public interface IRoundDancePeopleStorage
    {
        RoundDancePeople[] GetAll();
        Dictionary<Direction, int> GetDirectionResourceNeeds();
    }
}