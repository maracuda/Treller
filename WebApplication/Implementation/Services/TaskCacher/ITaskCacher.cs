using System;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.TaskCacher
{
    public interface ITaskCacher
    {
        T GetCached<T>(string[] boardIds, Func<string[], T> loadAction, TaskCacherStoredTypes storedType);
        DateTime Actualize(DateTime timestamp);
    }
}