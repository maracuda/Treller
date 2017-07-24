using WebApplication.Implementation.VirtualMachines.Logging;

namespace WebApplication.Implementation.VirtualMachines.BusinessObjects
{
    public class ShowLogsViewModel
    {
        public CommandExecuteResult CurrentResult { get; set; }
        public VirtualMachinesExecuteLogModel[] Logs { get; set; }
    }
}