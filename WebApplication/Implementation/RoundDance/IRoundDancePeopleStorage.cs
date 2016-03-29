using SKBKontur.Treller.WebApplication.Implementation.RoundDance.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Implementation.RoundDance
{
    public interface IRoundDancePeopleStorage
    {
        RoundDancePeople[] GetAll();
    }
}