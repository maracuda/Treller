using System;
using System.Threading.Tasks;
using SKBKontur.BlocksMapping.Abstrations;

namespace SKBKontur.Treller.WebApplication.Implementation.Infrastructure.Abstractions
{
    public class AsyncRunner : IAsyncRunner
    {
        public Task<TResult> Run<T1, T2, TResult>(Func<T1, T2, TResult> func, T1 firstArg, T2 secondArg, TaskCreationOptions runOption)
        {
            return Task.Factory.StartNew(() => func(firstArg, secondArg), runOption);
        }
    }
}