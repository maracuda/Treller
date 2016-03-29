using SKBKontur.Treller.WebApplication.Implementation.Activities.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Implementation.Activities
{
    public interface IDepartureEventStorage
    {
        EventViewModel GetNextEvent();
    }
}