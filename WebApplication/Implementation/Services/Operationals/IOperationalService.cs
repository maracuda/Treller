using System;
using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals
{
    public interface IOperationalService : IDisposable
    {
        void Register(IRegularOperation operation);
    }
}