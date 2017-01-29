using SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Operations;

namespace SKBKontur.Treller.WebApplication.Implementation.Services.Operationals.Launcher
{
    public interface IOperationsLauncher
    {
        void SafeLaunch(IRegularOperation operation);
    }
}