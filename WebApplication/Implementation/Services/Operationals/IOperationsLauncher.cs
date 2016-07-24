using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals
{
    public interface IOperationsLauncher
    {
        void SafeLaunch(IRegularOperation operation);
    }
}