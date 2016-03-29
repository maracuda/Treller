namespace SKBKontur.Treller.WebApplication.Fan.Activities
{
    public interface IDepartureEventStorage
    {
        EventViewModel GetNextEvent();
    }
}