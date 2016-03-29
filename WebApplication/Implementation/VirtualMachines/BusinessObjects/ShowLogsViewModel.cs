using SKBKontur.Treller.WebApplication.Implementation.VirtualMachines.Logging;

namespace SKBKontur.Treller.WebApplication.Implementation.VirtualMachines.BusinessObjects
{
    public class ShowLogsViewModel
    {
        public CommandExecuteResult CurrentResult { get; set; }
        public VirtualMachinesExecuteLogModel[] Logs { get; set; }
    }
}