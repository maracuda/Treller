using System;
using SKBKontur.Infrastructure.Sugar;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations
{
    public class SimpleOperation : RegularOperation
    {
        public SimpleOperation(string name, TimeSpan runPeriod, Action action) : base(name, runPeriod, action)
        {
        }

        public override Maybe<Exception> Run()
        {
            try
            {
                action.Invoke();
                return null;
            }
            catch (Exception e)
            {
                return e;
            }
        }
    }
}