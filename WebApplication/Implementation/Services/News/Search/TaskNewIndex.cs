using System.Linq;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Domain;
using SKBKontur.Treller.WebApplication.Implementation.Services.News.Storage;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.News.Search
{
    public class TaskNewIndex : ITaskNewIndex
    {
        private readonly ITaskNewStorage taskNewStorage;
        private readonly ITaskNewConverter taskNewConverter;

        public TaskNewIndex(
            ITaskNewStorage taskNewStorage,
            ITaskNewConverter taskNewConverter)
        {
            this.taskNewStorage = taskNewStorage;
            this.taskNewConverter = taskNewConverter;
        }

        public TaskNewModel[] SelectCurrentNews()
        {
            return taskNewStorage.ReadAll()
                                 .Select(x => taskNewConverter.Build(x))
                                 .ToArray();
        }
    }
}