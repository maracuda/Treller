using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace SKBKontur.TaskManagerClient.Extensions
{
    public static class CommonExtensions
    {
        private static async Task<TResult[]> Await<T, TResult>(this IEnumerable<Task<T>> tasksCollection, Func<T, TResult> converter)
        {
            var tasks = tasksCollection.ToArray();
            var length = tasks.Length;
            var result = new TResult[length];

            for (var taskIndex = 0; taskIndex < length; taskIndex++)
            {
                result[taskIndex] = converter(await tasks[taskIndex]);
            }

            return result;
        }

        public static async Task<TResult> Await<T, TResult>(this Task<T> task, Func<T, TResult> converter)
        {
            var result = await Await(new[] {task}, converter);
            return result.FirstOrDefault();
        }

        public static async Task<TResult[]> Await<T, TResult>(this IEnumerable<Task<T[]>> tasksCollection, Func<T, TResult> converter, Func<IEnumerable<TResult>, IEnumerable<TResult>> resultFilter = null)
        {
            var tasks = tasksCollection.ToArray();
            var result = new LinkedList<TResult>();

            foreach (var task in tasks)
            {
                var values = await task;
                foreach (var value in values)
                {
                    result.AddLast(converter(value));
                }
            }
            return resultFilter != null ? resultFilter(result).ToArray() : result.ToArray();
        }

        public static Task<TResult[]> Await<T, TResult>(this Task<T[]> task, Func<T, TResult> converter, Func<IEnumerable<TResult>, IEnumerable<TResult>> resultFilter = null)
        {
            return Await(new[] {task}, converter, resultFilter);
        }
    }
}