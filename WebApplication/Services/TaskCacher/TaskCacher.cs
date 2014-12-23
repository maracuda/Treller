using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Web;
using Newtonsoft.Json;
using SKBKontur.TaskManagerClient;
using SKBKontur.TaskManagerClient.BusinessObjects;

namespace SKBKontur.Treller.WebApplication.Services.TaskCacher
{
    public class TaskCacher : ITaskCacher, IDisposable
    {
        private readonly ITaskManagerClient taskManagerClient;
        private readonly ConcurrentDictionary<CacheKey, CacheResult> cache;
        private bool isTimerInProgress;
        private readonly Timer timer;
        private DateTime lastUpdateUtc = DateTime.UtcNow.AddDays(-2);
        private readonly HashSet<ActionType> checklistActions = new HashSet<ActionType>(new []{ ActionType.AddChecklistToCard, ActionType.ConvertToCardFromCheckItem, ActionType.RemoveChecklistFromCard, ActionType.UpdateCheckItemStateOnCard, ActionType.UpdateChecklist });
        private readonly Dictionary<TaskCacherStoredTypes, Type> storKeys = new Dictionary<TaskCacherStoredTypes, Type>
                                                                                {
                                                                                    {TaskCacherStoredTypes.BoardActions, typeof(StoredObject<CardAction[]>)},
                                                                                    {TaskCacherStoredTypes.BoardCards, typeof(StoredObject<BoardCard[]>)},
                                                                                    {TaskCacherStoredTypes.BoardChecklists, typeof(StoredObject<CardChecklist[]>)},
                                                                                    {TaskCacherStoredTypes.BoardLists, typeof(StoredObject<BoardList[]>)},
                                                                                    {TaskCacherStoredTypes.BoardUsers, typeof(StoredObject<User[]>)},
                                                                                    {TaskCacherStoredTypes.Boards, typeof(StoredObject<Board[]>)}
                                                                                };
        private static readonly string StorFilePattern = Path.Combine(HttpRuntime.AppDomainAppPath, "TrellerCache{0}.json");
        
        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<string, dynamic>> buildedEntities = new ConcurrentDictionary<Type, ConcurrentDictionary<string, dynamic>>(new []
                                                                                                               { 
                                                                                                                   new KeyValuePair<Type, ConcurrentDictionary<string, dynamic>>(typeof(BoardCard[]), new ConcurrentDictionary<string, dynamic>(3, 100)),
                                                                                                                   new KeyValuePair<Type, ConcurrentDictionary<string, dynamic>>(typeof(User[]), new ConcurrentDictionary<string, dynamic>(3, 50)),
                                                                                                                   new KeyValuePair<Type, ConcurrentDictionary<string, dynamic>>(typeof(BoardList[]), new ConcurrentDictionary<string, dynamic>(3, 50)),
                                                                                                                   new KeyValuePair<Type, ConcurrentDictionary<string, dynamic>>(typeof(CardChecklist[]), new ConcurrentDictionary<string, dynamic>(3, 500)),
                                                                                                                   new KeyValuePair<Type, ConcurrentDictionary<string, dynamic>>(typeof(CardAction[]), new ConcurrentDictionary<string, dynamic>(3, 5000)),
                                                                                                                   new KeyValuePair<Type, ConcurrentDictionary<string, dynamic>>(typeof(Board[]), new ConcurrentDictionary<string, dynamic>(3, 10)),
                                                                                                               });

        public TaskCacher(ITaskManagerClient taskManagerClient)
        {
            this.taskManagerClient = taskManagerClient;
            cache = new ConcurrentDictionary<CacheKey, CacheResult>(3, 6);

            foreach (var storKey in storKeys)
            {
                var fileName = string.Format(StorFilePattern, storKey.Key);
                if (File.Exists(fileName))
                {
                    var result = (IStoredObject)JsonConvert.DeserializeObject(File.ReadAllText(fileName, Encoding.UTF8), storKey.Value);
                    var key = new CacheKey(storKey.Key, result.BoardIds);
                    cache.TryAdd(key, new CacheResult(storKey.Key, null, result.Result));

                    foreach (var entity in result.Result)
                    {
                        buildedEntities[storKey.Value.GenericTypeArguments[0]][entity.Id] = entity;
                    }
                }
            }

            timer = new Timer(60000);
            timer.Elapsed += TimerOnElapsed;
            timer.Start();
        }

        public interface IStoredObject
        {
            dynamic Result { get; }
            string[] BoardIds { get; }
        }

        public class StoredObject<T> : IStoredObject
        {
            public T LastResult { get; set; }
            public string[] BoardIds { get; set; }

            public dynamic Result { get { return LastResult; } }
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            if (isTimerInProgress)
            {
                return;
            }

            isTimerInProgress = true;

            try
            {
                var keys = cache.Select(x => x.Key).ToArray();

                var boardIds = keys.SelectMany(x => x.GetBoardIds()).Distinct().ToArray();

                var actions = taskManagerClient.GetActionsForBoardCards(boardIds, lastUpdateUtc).ToArray();
                

                var isSuccessUpdate = UpdateWhenExists(actions, action => action.Type < ActionType.CreateList, keys.Where(x => x.StoredType == TaskCacherStoredTypes.BoardCards));
                isSuccessUpdate &= UpdateWhenExists(actions, action => action.Type < ActionType.CreateBoard, keys.Where(x => x.StoredType == TaskCacherStoredTypes.BoardActions));
                isSuccessUpdate &= UpdateWhenExists(actions, action => action.Type == ActionType.AddMemberToBoard, keys.Where(x => x.StoredType == TaskCacherStoredTypes.BoardUsers));
                isSuccessUpdate &= UpdateWhenExists(actions, action => action.Type == ActionType.CreateList
                                                    || action.Type == ActionType.UpdateList, keys.Where(x => x.StoredType == TaskCacherStoredTypes.BoardLists));
                isSuccessUpdate &= UpdateWhenExists(actions, action => action.Type == ActionType.UpdateBoard, keys.Where(x => x.StoredType == TaskCacherStoredTypes.Boards));
                isSuccessUpdate &= UpdateWhenExists(actions, action => checklistActions.Contains(action.Type), keys.Where(x => x.StoredType == TaskCacherStoredTypes.BoardChecklists));
                
                if (isSuccessUpdate)
                {
                    lastUpdateUtc = elapsedEventArgs.SignalTime;
                }
            }
            finally
            {
                isTimerInProgress = false;
            }
        }

        private bool UpdateWhenExists(IEnumerable<CardAction> actions, Func<CardAction, bool> anyPredicate, IEnumerable<CacheKey> keys)
        {
            if (actions.Any(anyPredicate))
            {
                foreach (var key in keys)
                {
                    CacheResult result;
                    if (cache.TryGetValue(key, out result) && result.Loader != null)
                    {
                        result.LastResult = result.Loader();
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public T GetCached<T>(string[] boardIds, Func<string[], T> loadAction, TaskCacherStoredTypes storedType)
        {
            var cacheKey = new CacheKey(storedType, boardIds);
            var result = cache.GetOrAdd(cacheKey, key => new CacheResult(key.StoredType, () => Load(loadAction, key), Load(loadAction, key)));
            if (result.Loader == null)
            {
                result.Loader = () => Load(loadAction, cacheKey);
            }
            return result.LastResult;
        }

        public T[] GetBuilded<T>()
        {
            return buildedEntities[typeof (T).MakeArrayType()].Select(x => (T)x.Value).ToArray();
        }

        private T Load<T>(Func<string[], T> loadAction, CacheKey cacheKey)
        {
            var result = (dynamic)loadAction(cacheKey.GetBoardIds());
            var contents = JsonConvert.SerializeObject(new StoredObject<T> {BoardIds = cacheKey.GetBoardIds(), LastResult = result});
            File.WriteAllText(string.Format(StorFilePattern, cacheKey.StoredType), contents);

            foreach (var entity in result)
            {
                buildedEntities[typeof (T)][entity.Id] = entity;
            }
            return result;
        }

        private struct CacheKey
        {
            public CacheKey(TaskCacherStoredTypes storedType, string[] boardIds) : this()
            {
                StoredType = storedType;
                BoardIds = string.Join(";", boardIds);
            }

            public string BoardIds { get; set; }
            public TaskCacherStoredTypes StoredType { get; set; }

            public string[] GetBoardIds()
            {
                return BoardIds.Split(';');
            }
        }

        private class CacheResult
        {
            public CacheResult(TaskCacherStoredTypes storedType, Func<dynamic> loader, dynamic lastResult)
            {
                StoredType = storedType;
                Loader = loader;
                LastResult = lastResult;
            }

            public TaskCacherStoredTypes StoredType { get; private set; }
            public Func<dynamic> Loader { get; set; }
            public dynamic LastResult { get; set; }
        }

        public void Dispose()
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
        }
    }
}