using SKBKontur.Treller.OperationalService.Operations;

namespace SKBKontur.Treller.OperationalService.Launcher
{
    public interface IOperationsLauncher
    {
        void SafeLaunch(IRegularOperation operation);
    }
}