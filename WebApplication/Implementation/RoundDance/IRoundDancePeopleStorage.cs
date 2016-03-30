using System;
using SKBKontur.Treller.WebApplication.Implementation.RoundDance.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Implementation.RoundDance
{
    public interface IRoundDancePeopleStorage
    {
        RoundDancePeople[] GetAll();
        void AddNew(string name, string direction, DateTime beginDate, string email = null, string pairName = null);
        void Delete(string name, string direction, DateTime beginDate);
    }
}