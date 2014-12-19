using System;
using System.Threading.Tasks;
using SKBKontur.Billy.Core.BlocksMapping.Abstrations;

namespace SKBKontur.Treller.WebApplication.Services.Abstractions
{
    public class AsyncRunner : IAsyncRunner
    {
        public Task<TResult> Run<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> func, T1 firstArg, T2 secondArg, T3 thirdArg, TaskCreationOptions runOption)
        {
            return Task.Factory.StartNew(() => func(firstArg, secondArg, thirdArg), runOption);
        }
    }
}