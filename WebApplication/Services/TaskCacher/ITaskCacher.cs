using System;

namespace SKBKontur.Treller.WebApplication.Services.TaskCacher
{
    public interface ITaskCacher
    {
        T GetCached<T>(string[] boardIds, Func<string[], T> loadAction, TaskCacherStoredTypes storedType);
        T[] GetBuilded<T>();
        bool TryActualize(DateTime timestamp);
    }
}