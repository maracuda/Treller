using SKBKontur.Treller.WebApplication.Implementation.Services.News.Search;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Domain
{
    public interface ITaskNewConverter
    {
        TaskNewModel Build(TaskNew taskNew);
        TaskNew Project(TaskNewModel taskNewModel);
    }
}