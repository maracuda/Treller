using OperationalService.Operations;

namespace OperationalService.Launcher
{
    public interface IOperationsLauncher
    {
        void SafeLaunch(IRegularOperation operation);
    }
}